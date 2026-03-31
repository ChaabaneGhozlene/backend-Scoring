using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using scoring_Backend.DTO.Dashboard;

namespace scoring_Backend.Services
{
    public interface IDashboardService
    {
        Task<DashboardResponseDto> GetDashboardAsync(DashboardFilterDto filter);
    }

    public class DashboardService : IDashboardService
    {
        private readonly IConfiguration _configuration;

        public DashboardService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("SqrScoring"));
        }

        public async Task<DashboardResponseDto> GetDashboardAsync(DashboardFilterDto filter)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();
            var whereClauses = new List<string>();

            if (!string.IsNullOrWhiteSpace(filter.StartDate) &&
                DateTime.TryParse(filter.StartDate, out var startDate))
            {
                whereClauses.Add("r.CallLocalTime >= @StartDate");
                parameters.Add("@StartDate", startDate);
            }

            if (!string.IsNullOrWhiteSpace(filter.EndDate) &&
                DateTime.TryParse(filter.EndDate, out var endDate))
            {
                whereClauses.Add("r.CallLocalTime < @EndDatePlusOne");
                parameters.Add("@EndDatePlusOne", endDate.AddDays(1));
            }

            var whereSql = whereClauses.Count > 0
                ? "WHERE " + string.Join(" AND ", whereClauses)
                : "";

            var totalRecordingsSql = $@"
                SELECT COUNT(*)
                FROM dbo.RecordData r
                {whereSql};
            ";

            var totalEvaluationsSql = $@"
                SELECT COUNT(*)
                FROM dbo.RecordData r
                INNER JOIN dbo.Ls_survey s ON r.ID = s.RecordDataId
                {whereSql};
            ";

            var averageScoreSql = $@"
                SELECT ISNULL(AVG(CAST(s.Score AS decimal(10,2))), 0)
                FROM dbo.RecordData r
                INNER JOIN dbo.Ls_survey s ON r.ID = s.RecordDataId
                {whereSql};
            ";

   var evaluationsByAuditorSql = $@"
    SELECT TOP 10
        CAST(e.IdAuditor AS varchar(50)) AS Label,
        COUNT(*) AS Value
    FROM dbo.RecordData r
    INNER JOIN dbo.Ls_survey s ON r.ID = s.RecordDataId
    LEFT JOIN dbo.Ls_EvalAuditor e ON s.Id = e.NumberEval
    {whereSql}
    GROUP BY e.IdAuditor
    ORDER BY Value DESC;
";


            var listeningsBySupervisorSql = $@"
                SELECT TOP 10
                    LTRIM(RTRIM(ISNULL(r.NomAgent,'') + ' ' + ISNULL(r.PrenomAgent,''))) AS Label,
                    COUNT(*) AS Value
                FROM dbo.RecordData r
                {whereSql}
                GROUP BY r.NomAgent, r.PrenomAgent
                ORDER BY Value DESC;
            ";

            var top5AgentsSql = $@"
                SELECT TOP 5
                    LTRIM(RTRIM(ISNULL(r.NomAgent,'') + ' ' + ISNULL(r.PrenomAgent,''))) AS Label,
                    ISNULL(AVG(CAST(s.Score AS decimal(10,2))), 0) AS Value
                FROM dbo.RecordData r
                INNER JOIN dbo.Ls_survey s ON r.ID = s.RecordDataId
                {whereSql}
                GROUP BY r.NomAgent, r.PrenomAgent
                ORDER BY Value ASC;
            ";

            var customerConcernsSql = $@"
    SELECT TOP 10
        ISNULL(r.CampaignDescription, 'Unknown') AS Label,
        COUNT(*) AS Value
    FROM dbo.RecordData r
    INNER JOIN dbo.Ls_survey s ON r.ID = s.RecordDataId
    {whereSql}
    GROUP BY r.CampaignDescription
    ORDER BY Value DESC;
";

            return new DashboardResponseDto
            {
                Stats = new DashboardSummaryDto
                {
                    TotalRecordings = await connection.ExecuteScalarAsync<int>(totalRecordingsSql, parameters),
                    TotalEvaluations = await connection.ExecuteScalarAsync<int>(totalEvaluationsSql, parameters),
                    AverageScore = Math.Round(await connection.ExecuteScalarAsync<decimal>(averageScoreSql, parameters), 2)
                },
                EvaluationsByAuditor = (await connection.QueryAsync<DashboardRowDto>(evaluationsByAuditorSql, parameters)).ToList(),
                ListeningsBySupervisor = (await connection.QueryAsync<DashboardRowDto>(listeningsBySupervisorSql, parameters)).ToList(),
                Top5Agents = (await connection.QueryAsync<DashboardRowDto>(top5AgentsSql, parameters)).ToList(),
                CustomerConcerns = (await connection.QueryAsync<DashboardRowDto>(customerConcernsSql, parameters)).ToList()
            };
        }
    }
}