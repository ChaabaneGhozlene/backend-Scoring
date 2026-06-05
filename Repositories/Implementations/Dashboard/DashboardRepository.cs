using Microsoft.Data.SqlClient;
using scoring_Backend.DTO.Dashboard;
using scoring_Backend.Repositories.Interfaces.Dashboard;
using System.Data;

namespace scoring_Backend.Repositories.Implementations.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly string _connStr;

        public DashboardRepository(IConfiguration cfg)
        {
            _connStr = cfg.GetConnectionString("SqrScoring")
                ?? throw new InvalidOperationException("SqrScoring connection string missing.");
        }

        private SqlConnection Open() => new(_connStr);

        public async Task<DashboardFullDto> GetDashboardAsync(
            DashboardFilterDto f, int userId, string userRole, int siteId)
        {
            var from = f.DateFrom.ToString("yyyyMMdd");
            var to   = f.DateTo.ToString("yyyyMMdd");

            var result = new DashboardFullDto
            {
                Kpi                = await GetKpiAsync(userId, userRole, siteId, from, to),
                ListenByCampaign   = await GetListenByCampaignAsync(userId, userRole, siteId, from, to),
                EvalByCampaign     = await GetEvalByCampaignAsync(userId, userRole, siteId, from, to),
                Evolution          = await GetEvolutionAsync(userId, from, to),
                TopAgents          = await GetTopAgentsAsync(userId, userRole, siteId, from, to, "desc"),
                BottomAgents       = await GetTopAgentsAsync(userId, userRole, siteId, from, to, "asc"),
                TopCategories      = await GetTopCategoriesAsync(userId, userRole, siteId, from, to),
                EvalByAuditor      = await GetEvalByAuditorAsync(userId, userRole, siteId, from, to),
                ListenBySupervisor = await GetListenBySupervisorAsync(userId, userRole, siteId, from, to),
            };
            return result;
        }

        // ── KPI ───────────────────────────────────────────────────────────────
        private async Task<DashboardKpiDto> GetKpiAsync(
            int userId, string role, int siteId, string from, string to)
        {
            string sql = $"SELECT * FROM [SQR_REC].[dbo].[Fn_Ls_Kpi] ({userId},{from},{to})";
            await using var conn = Open();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            await using var rdr = await cmd.ExecuteReaderAsync();
            if (await rdr.ReadAsync())
            {
                return new DashboardKpiDto
                {
                    NbListened   = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0),
                    NbEvaluated  = rdr.IsDBNull(1) ? 0 : rdr.GetInt32(1),
                    AverageScore = rdr.IsDBNull(2) ? 0 : Math.Round(Convert.ToDouble(rdr.GetValue(2)), 2),
                    EvalRate     = rdr.IsDBNull(3) ? 0 : Math.Round(Convert.ToDouble(rdr.GetValue(3)), 2),
                };
            }
            return new DashboardKpiDto();
        }

        // ── Listens by Campaign ───────────────────────────────────────────────
        private async Task<List<DashboardCampaignBarDto>> GetListenByCampaignAsync(
            int userId, string role, int siteId, string from, string to)
        {
            string roleFilter = role switch
            {
                "SuperAdmin" => "",
                "Admin"      => $"AND r.CustomerID = {siteId}",
                _            => $"AND b.UserId = {userId}",
            };

            string sql = $@"
                SELECT COUNT(DISTINCT b.Id) AS nombre, r.[CampaignDescription]
                FROM [dbo].[ap_action] b, [dbo].RecordData r
                WHERE r.ID = RecordedId AND CodeEvent = 0
                  AND convert(varchar,b.CreateDate,112) >= {from}
                  AND convert(varchar,b.CreateDate,112) <= {to}
                  {roleFilter}
                GROUP BY r.[CampaignDescription]
                HAVING COUNT(*) > 0
                ORDER BY 2 ASC";

            return await ReadBarDataAsync(sql, "CampaignDescription", "nombre");
        }

        // ── Evals by Campaign ─────────────────────────────────────────────────
        private async Task<List<DashboardCampaignBarDto>> GetEvalByCampaignAsync(
            int userId, string role, int siteId, string from, string to)
        {
            string roleFilter = role switch
            {
                "SuperAdmin" => "",
                "Admin"      => $"AND r.CustomerID = {siteId}",
                _            => $"AND s.Auditor = {userId}",
            };

            string sql = $@"
                SELECT COUNT(r.LsId), [CampaignDescription]
                FROM [SQR_REC].[dbo].[RecordData] r, [SQR_REC].[dbo].[Ls_survey] l
                WHERE CampaignDescription IS NOT NULL AND l.Id = r.LsId
                  AND convert(varchar,l.CreateDate,112) >= {from}
                  AND convert(varchar,l.CreateDate,112) <= {to}
                  {roleFilter}
                GROUP BY [CampaignDescription]
                HAVING COUNT(*) > 0
                ORDER BY 2 ASC";

            return await ReadBarDataAsync(sql, "CampaignDescription", null);
        }

        // ── Evolution (score moyen dans le temps) ─────────────────────────────
        private async Task<List<DashboardEvolutionDto>> GetEvolutionAsync(
            int userId, string from, string to)
        {
            string sql = $"SELECT * FROM [SQR_REC].[dbo].[Fn_Ls_Kpi] ({userId},{from},{to})";
            var list = new List<DashboardEvolutionDto>();

            await using var conn = Open();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            await using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new DashboardEvolutionDto
                {
                    Date  = rdr.IsDBNull(4) ? "" : rdr.GetValue(4).ToString()!,
                    Score = rdr.IsDBNull(2) ? 0  : Math.Round(Convert.ToDouble(rdr.GetValue(2)), 2),
                });
            }
            return list;
        }

        // ── Top / Bottom Agents ───────────────────────────────────────────────
        private async Task<List<DashboardAgentScoreDto>> GetTopAgentsAsync(
            int userId, string role, int siteId, string from, string to, string order)
        {
            string roleJoin = role switch
            {
                "SuperAdmin" => "",
                "Admin"      => $@",[SQR_Admin].[dbo].[Users] AS u,
                                    [SQR_REC].[dbo].[Ls_CalledCampaign] AS c
                                    WHERE ls.CalledCampaignId = c.Id AND c.Site = u.SiteID
                                    AND u.SiteId = {siteId}
                                    AND convert(varchar,ls.CreateDate,112) >= {from}
                                    AND convert(varchar,ls.CreateDate,112) <= {to}",
                _            => $@",[SQR_Admin].[dbo].[Users] AS u,
                                    [SQR_REC].[dbo].[Ls_CalledCampaign] AS c
                                    WHERE ls.CalledCampaignId = c.Id AND c.Site = u.SiteID
                                    AND u.ID = {userId}
                                    AND convert(varchar,ls.CreateDate,112) >= {from}
                                    AND convert(varchar,ls.CreateDate,112) <= {to}",
            };

            string whereClause = role == "SuperAdmin"
                ? $"WHERE convert(varchar,CreateDate,112) >= {from} AND convert(varchar,CreateDate,112) <= {to}"
                : "";

            string sql = role == "SuperAdmin"
                ? $@"SELECT TOP 5 Agent, ROUND(Avg(Score),2) AS Score
                     FROM [SQR_REC].[dbo].[Ls] ls
                     {whereClause}
                     GROUP BY Agent HAVING Avg(Score) IS NOT NULL
                     ORDER BY Avg(Score) {(order == "desc" ? "DESC" : "ASC")}"
                : $@"SELECT TOP 5 Agent, ROUND(Avg(Score),2) AS Score
                     FROM [SQR_REC].[dbo].[Ls] AS ls
                     {roleJoin}
                     GROUP BY Agent HAVING Avg(Score) IS NOT NULL
                     ORDER BY Avg(Score) {(order == "desc" ? "DESC" : "ASC")}";

            return await ReadAgentScoreAsync(sql);
        }

        // ── Top Categories ────────────────────────────────────────────────────
        private async Task<List<DashboardCategoryDto>> GetTopCategoriesAsync(
            int userId, string role, int siteId, string from, string to)
        {
            string roleFilter = role switch
            {
                "SuperAdmin" => "",
                "Admin"      => $@"INNER JOIN [SQR_REC].[dbo].[Ls] AS ls
                                    ON lssu.LsId = ls.Id
                                    INNER JOIN [SQR_REC].[dbo].[Ls_CalledCampaign] AS c
                                    ON ls.CalledCampaignId = c.Id
                                    INNER JOIN [SQR_Admin].[dbo].[Users] AS u
                                    ON c.Site = u.SiteID AND u.SiteId = {siteId}",
                _            => $@"INNER JOIN [SQR_REC].[dbo].[Ls] AS ls
                                    ON lssu.LsId = ls.Id
                                    WHERE ls.Auditor = {userId} AND",
            };

            string sql = role switch
            {
                "SuperAdmin" => $@"
                    SELECT cat.Des_Categories AS Description,
                           COUNT(lssu.Id_Categories) AS Number
                    FROM [SQR_REC].[dbo].[Ls_categories] cat,
                         [SQR_REC].[dbo].[Ls_survey] lssu
                    WHERE lssu.Is_saved = 1 AND lssu.Id_Categories = cat.Id
                      AND convert(varchar,lssu.CreateDate,112) >= {from}
                      AND convert(varchar,lssu.CreateDate,112) <= {to}
                    GROUP BY cat.Des_Categories ORDER BY Number DESC",
                _ => $@"
                    SELECT cat.Des_Categories AS Description,
                           COUNT(ls.Id_Categories) AS Number
                    FROM [SQR_REC].[dbo].[Ls] ls,
                         [SQR_Admin].[dbo].[Users] u,
                         [SQR_REC].[dbo].[Ls_CalledCampaign] c,
                         [SQR_REC].[dbo].[Ls_categories] cat,
                         [SQR_REC].[dbo].[Ls_survey] lssu
                    WHERE lssu.Is_saved = 1
                      AND convert(varchar,lssu.CreateDate,112) >= {from}
                      AND convert(varchar,lssu.CreateDate,112) <= {to}
                      AND ls.CalledCampaignId = c.Id
                      AND c.Site = u.SiteID
                      AND lssu.Id_Categories = cat.Id
                      AND lssu.LsId = ls.Id
                      AND u.{(role == "Admin" ? $"SiteId = {siteId}" : $"ID = {userId}")}
                    GROUP BY Des_Categories ORDER BY Number DESC",
            };

            var list = new List<DashboardCategoryDto>();
            await using var conn = Open();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            await using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new DashboardCategoryDto
                {
                    Description = rdr.IsDBNull(0) ? "" : rdr.GetValue(0).ToString()!,
                    Number      = rdr.IsDBNull(1) ? 0  : Convert.ToInt32(rdr.GetValue(1)),
                });
            }
            return list;
        }

        // ── Evals by Auditor ──────────────────────────────────────────────────
        private async Task<List<DashboardAgentScoreDto>> GetEvalByAuditorAsync(
            int userId, string role, int siteId, string from, string to)
        {
            string roleFilter = role switch
            {
                "SuperAdmin" => "",
                "Admin"      => $"AND SiteID = {siteId}",
                _            => $"AND ID = {userId}",
            };

            string sql = $@"
                SELECT [LastName] + ' ' + ISNULL([FirstName],'') AS Auditor,
                       (SELECT COUNT(*) FROM SQR_REC.[dbo].[Ls] l,
                                             SQR_REC.[dbo].[Ls_survey] s
                        WHERE l.Id = s.LsId AND s.Is_saved = 1
                          AND Auditor = u.ID
                          AND convert(varchar,s.CreateDate,112) >= {from}
                          AND convert(varchar,s.CreateDate,112) <= {to}) AS Nombre
                FROM [SQR_Admin].[dbo].[Users] u
                WHERE (SELECT COUNT(*) FROM SQR_REC.[dbo].[Ls] l,
                                             SQR_REC.[dbo].[Ls_survey] s
                       WHERE l.Id = s.LsId AND s.Is_saved = 1
                         AND Auditor = u.ID
                         AND convert(varchar,s.CreateDate,112) >= {from}
                         AND convert(varchar,s.CreateDate,112) <= {to}) > 0
                {roleFilter}
                ORDER BY 2 DESC";

            return await ReadAgentScoreAsync(sql);
        }

        // ── Listens by Supervisor ─────────────────────────────────────────────
        private async Task<List<DashboardAgentScoreDto>> GetListenBySupervisorAsync(
            int userId, string role, int siteId, string from, string to)
        {
            string roleFilter = role switch
            {
                "SuperAdmin" => "",
                "Admin"      => $"AND SiteID = {siteId}",
                _            => $"AND ID = {userId}",
            };

            string sql = $@"
                SELECT [LastName] + ' ' + ISNULL([FirstName],'') AS Supervisor,
                       (SELECT COUNT(DISTINCT b.Id)
                        FROM SQR_REC.[dbo].[ap_action] b, SQR_REC.[dbo].RecordData r
                        WHERE r.ID = RecordedId AND UserId = u.ID AND CodeEvent = 0
                          AND convert(varchar,b.CreateDate,112) >= {from}
                          AND convert(varchar,b.CreateDate,112) <= {to}) AS Nombre
                FROM [SQR_Admin].[dbo].[Users] u
                WHERE (SELECT COUNT(DISTINCT b.Id)
                       FROM SQR_REC.[dbo].[ap_action] b, SQR_REC.[dbo].RecordData r
                       WHERE r.ID = RecordedId AND UserId = u.ID AND CodeEvent = 0
                         AND convert(varchar,b.CreateDate,112) >= {from}
                         AND convert(varchar,b.CreateDate,112) <= {to}) > 0
                {roleFilter}
                ORDER BY 2 DESC";

            return await ReadAgentScoreAsync(sql);
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private async Task<List<DashboardCampaignBarDto>> ReadBarDataAsync(
            string sql, string nameCol, string? countCol)
        {
            var list = new List<DashboardCampaignBarDto>();
            await using var conn = Open();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            await using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new DashboardCampaignBarDto
                {
                    Campaign = rdr.IsDBNull(1) ? "" : rdr.GetValue(1).ToString()!,
                    Count    = rdr.IsDBNull(0) ? 0  : Convert.ToInt32(rdr.GetValue(0)),
                });
            }
            return list;
        }

        private async Task<List<DashboardAgentScoreDto>> ReadAgentScoreAsync(string sql)
        {
            var list = new List<DashboardAgentScoreDto>();
            await using var conn = Open();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            await using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new DashboardAgentScoreDto
                {
                    Agent = rdr.IsDBNull(0) ? "" : rdr.GetValue(0).ToString()!,
                    Score = rdr.IsDBNull(1) ? 0  : Math.Round(Convert.ToDouble(rdr.GetValue(1)), 2),
                });
            }
            return list;
        }
    }
}