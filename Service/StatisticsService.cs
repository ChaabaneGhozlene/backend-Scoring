using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using scoring_Backend.DTO.Statistics;

namespace scoring_Backend.Services
{
    public interface IStatisticsService
    {
        Task<StatisticsFilterOptionsDto> GetFilterOptionsAsync();
        Task<StatisticsResponseDto> SearchAsync(StatisticsFilterDto filter);
    }

    public class StatisticsService : IStatisticsService
    {
        private readonly IConfiguration _configuration;

        public StatisticsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("SqrScoring"));
        }

        public async Task<StatisticsFilterOptionsDto> GetFilterOptionsAsync()
        {
            using var connection = CreateConnection();

            var teams = (await connection.QueryAsync<TeamOptionDto>(@"
                SELECT Id, ISNULL(Description, '') AS Description
                FROM dbo.tListAgentTeam
                ORDER BY Description
            ")).ToList();

            var campaigns = (await connection.QueryAsync<string>(@"
                SELECT DISTINCT ISNULL(CampaignDescription, '')
                FROM dbo.RecordData
                WHERE CampaignDescription IS NOT NULL AND CampaignDescription <> ''
                ORDER BY ISNULL(CampaignDescription, '')
            ")).ToList();

            var agents = (await connection.QueryAsync<AgentOptionDto>(@"
                SELECT DISTINCT
                    ISNULL(r.AgentOid, '') AS AgentOid,
                    LTRIM(RTRIM(ISNULL(r.NomAgent, '') + ' ' + ISNULL(r.PrenomAgent, ''))) AS FullName
                FROM dbo.RecordData r
                WHERE r.AgentOid IS NOT NULL AND r.AgentOid <> ''
                ORDER BY FullName
            ")).ToList();

            return new StatisticsFilterOptionsDto
            {
                Teams = teams,
                Campaigns = campaigns,
                Agents = agents
            };
        }

        public async Task<StatisticsResponseDto> SearchAsync(StatisticsFilterDto filter)
        {
            using var connection = CreateConnection();

            var whereClauses = new List<string>
            {
                "s.Score IS NOT NULL"
            };

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(filter.From))
            {
                whereClauses.Add("r.CallLocalTime >= @From");
                parameters.Add("@From", DateTime.Parse(filter.From));
            }

            if (!string.IsNullOrWhiteSpace(filter.To))
            {
                whereClauses.Add("r.CallLocalTime < @ToPlusOne");
                parameters.Add("@ToPlusOne", DateTime.Parse(filter.To).AddDays(1));
            }

            if (filter.TeamId.HasValue)
            {
                whereClauses.Add("at.TeamId = @TeamId");
                parameters.Add("@TeamId", filter.TeamId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Campaign))
            {
                whereClauses.Add("r.CampaignDescription = @Campaign");
                parameters.Add("@Campaign", filter.Campaign);
            }

            if (!string.IsNullOrWhiteSpace(filter.AgentOid))
            {
                whereClauses.Add("r.AgentOid = @AgentOid");
                parameters.Add("@AgentOid", filter.AgentOid);
            }

            var whereSql = "WHERE " + string.Join(" AND ", whereClauses);

            var sql = $@"
    SELECT TOP 1000
        r.ID AS RecordId,
        s.Id AS SurveyId,
        s.RecordDataId,
        CAST(s.Score AS decimal(10,2)) AS Score,
        ISNULL(r.CampaignDescription, '') AS Campaign,
        ISNULL(r.NomAgent, '') AS NomAgent,
        ISNULL(r.PrenomAgent, '') AS PrenomAgent,
        ISNULL(r.NumeroTel, '') AS PhoneNumber,
        ISNULL(r.AgentOid, '') AS AgentOid,
        ISNULL(t.Description, '') AS TeamName,
        at.TeamId,
        r.CallLocalTime,
        e.Date_Eval AS DateEval,
        '' AS Comment
    FROM dbo.RecordData r
    INNER JOIN dbo.Ls_survey s
        ON r.ID = s.RecordDataId
    LEFT JOIN dbo.tAgentTeams at
        ON r.AgentOid = at.AgentOid
    LEFT JOIN dbo.tListAgentTeam t
        ON at.TeamId = t.Id
    LEFT JOIN dbo.Ls_EvalAuditor e
        ON s.Id = e.NumberEval
    {whereSql}
    ORDER BY r.CallLocalTime DESC;
";

            var rows = (await connection.QueryAsync<StatisticRowDto>(sql, parameters)).ToList();

            var chart = BuildChart(rows, filter.GroupBy, filter.SummaryType);

            return new StatisticsResponseDto
            {
                Rows = rows,
                Chart = chart,
                Total = rows.Count
            };
        }

        private List<ChartPointDto> BuildChart(
            List<StatisticRowDto> rows,
            string groupBy,
            string summaryType)
        {
            string labelSelector(StatisticRowDto row) => groupBy?.ToLower() switch
            {
                "agent" => $"{row.NomAgent} {row.PrenomAgent}".Trim(),
                "date" => row.CallLocalTime?.ToString("yyyy-MM-dd") ?? "",
                "team" => string.IsNullOrWhiteSpace(row.TeamName) ? "Sans équipe" : row.TeamName,
                _ => string.IsNullOrWhiteSpace(row.Campaign) ? "Sans campagne" : row.Campaign
            };

            return rows
                .Where(x => x.Score.HasValue)
                .GroupBy(labelSelector)
                .Select(g =>
                {
                    decimal value = summaryType?.ToLower() switch
                    {
                        "sum" => g.Sum(x => x.Score ?? 0),
                        "count" => g.Count(),
                        "min" => g.Min(x => x.Score ?? 0),
                        "max" => g.Max(x => x.Score ?? 0),
                        _ => g.Average(x => x.Score ?? 0)
                    };

                    return new ChartPointDto
                    {
                        Label = g.Key,
                        Value = Math.Round(value, 2)
                    };
                })
                .OrderBy(x => x.Label)
                .ToList();
        }
    }
}