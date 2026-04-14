using Microsoft.Data.SqlClient;
using scoring_Backend.Repositories.Interfaces.Statistique;
using scoring_Backend.DTO.Statistique;
using System.Text;
using System.Text.Json;

namespace scoring_Backend.Repositories.Implementations.Statistique
{
    /// <summary>
    /// Remplace les 6 pages ASPX redondantes par une seule requête paramétrée.
    /// La logique de pivot est déléguée au frontend React.
    /// </summary>
    public class StatistiqueRepository2 : IStatistiqueRepository2
    {
        private readonly string _connectionString;

        public StatistiqueRepository2(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("SQR_REC")
                ?? throw new InvalidOperationException("Connection string 'SQR_REC' not found.");
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Données principales
        // ─────────────────────────────────────────────────────────────────────────
        public async Task<IEnumerable<StatistiqueRowDto>> GetStatistiqueDataAsync(StatistiqueFilterDto f)
        {
            var rows = new List<StatistiqueRowDto>();

            // La requête joint Ls_survey et Ls_surveyItem (LEFT JOIN) pour couvrir
            // tous les anciens rapports en un seul appel.
            var sql = new StringBuilder(@"
                SELECT
                    s.Id                        AS SurveyId,
                    s.CreateDate,
                    s.Score,
                    s.Memo,
                    s.Is_saved,
                    l.AgentId,
                    l.Agent,
                    l.Auditor                   AS AuditorId,
                    u.FirstName + ' ' + u.LastName AS AuditorName,
                    l.Ls_CalledCampaign_Id      AS CampaignId,
                    c.Description               AS Campaign,
                    s.FullPeriode,
                    r.Rec_IdLink                AS RecordLink,
                    si.Id                       AS ItemId,
                    si.Value                    AS ItemValue,
                    si.Memo                     AS ItemMemo,
                    ti.Question,
                    ti.Id                       AS QuestionId,
                    tg.Description              AS Section,
                    tg.Id                       AS SectionId
                FROM [SQR_REC].[dbo].[Ls_survey] s
                INNER JOIN [SQR_REC].[dbo].[Ls] l ON l.Id = s.Ls_Id
                LEFT JOIN [SQR_Admin].[dbo].[Users] u ON u.ID = l.Auditor
                LEFT JOIN [SQR_REC].[dbo].[Ls_CalledCampaign] c ON c.Id = l.Ls_CalledCampaign_Id
                LEFT JOIN [SQR_REC].[dbo].[recordData] r ON r.Id = l.recordData_Id
                LEFT JOIN [SQR_REC].[dbo].[Ls_surveyItem] si ON si.Ls_survey_Id = s.Id
                LEFT JOIN [SQR_REC].[dbo].[Ls_templateItem] ti ON ti.Id = si.Ls_templateItem_Id
                LEFT JOIN [SQR_REC].[dbo].[Ls_templateItemGroup] tg ON tg.Id = ti.Ls_templateItemGroup_Id
                WHERE s.Is_saved = 1
                  AND s.CreateDate >= @DateDebut
                  AND s.CreateDate <= @DateFin
            ");

            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();

            cmd.Parameters.AddWithValue("@DateDebut",
                new DateTime(f.DateDebut.Year, f.DateDebut.Month, f.DateDebut.Day, 0, 0, 0));
            cmd.Parameters.AddWithValue("@DateFin",
                new DateTime(f.DateFin.Year, f.DateFin.Month, f.DateFin.Day, 23, 59, 59));

            // Filtre campagne
            if (f.CampaignId.HasValue)
            {
                sql.Append(" AND l.Ls_CalledCampaign_Id = @CampaignId");
                cmd.Parameters.AddWithValue("@CampaignId", f.CampaignId.Value);
            }

            // Filtre agent
            if (f.AgentId.HasValue)
            {
                sql.Append(" AND l.AgentId = @AgentId");
                cmd.Parameters.AddWithValue("@AgentId", f.AgentId.Value);
            }

            // Filtre auditeur / superviseur
            if (!f.AllSupervisors && f.AuditorId.HasValue)
            {
                sql.Append(" AND l.Auditor = @AuditorId");
                cmd.Parameters.AddWithValue("@AuditorId", f.AuditorId.Value);
            }

            // Restriction par rôle
            if (f.UserRole == 2)
            {
                // Manager : campagnes autorisées de son site
                sql.Append(@"
                  AND l.Ls_CalledCampaign_Id IN (
                      SELECT uc.CampagneId FROM [SQR_Admin].[dbo].[UsersCampagne] uc
                      WHERE uc.UserId = @UserId AND uc.SiteId = @SiteId
                  )");
                cmd.Parameters.AddWithValue("@UserId", f.UserId);
                cmd.Parameters.AddWithValue("@SiteId", f.SiteId);
            }
            else if (f.UserRole == 3 && !f.AllSupervisors)
            {
                // Superviseur : uniquement ses agents
                sql.Append(@"
                  AND l.AgentId IN (
                      SELECT ua.AgentId FROM [SQR_Admin].[dbo].[UsersAgent] ua
                      WHERE ua.UserId = @UserId
                  )");
                if (!cmd.Parameters.Contains("@UserId"))
                    cmd.Parameters.AddWithValue("@UserId", f.UserId);
            }

            cmd.CommandText = sql.ToString();
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                rows.Add(new StatistiqueRowDto
                {
                    SurveyId    = reader.GetInt32(reader.GetOrdinal("SurveyId")),
                    CreateDate  = reader.GetDateTime(reader.GetOrdinal("CreateDate")),
                    Score       = reader.IsDBNull(reader.GetOrdinal("Score")) ? 0 : reader.GetDouble(reader.GetOrdinal("Score")),
                    Memo        = reader.IsDBNull(reader.GetOrdinal("Memo")) ? null : reader.GetString(reader.GetOrdinal("Memo")),
                    AgentId     = reader.GetInt32(reader.GetOrdinal("AgentId")),
                    Agent       = reader.IsDBNull(reader.GetOrdinal("Agent")) ? "" : reader.GetString(reader.GetOrdinal("Agent")),
                    AuditorId   = reader.IsDBNull(reader.GetOrdinal("AuditorId")) ? 0 : reader.GetInt32(reader.GetOrdinal("AuditorId")),
                    Auditor     = reader.IsDBNull(reader.GetOrdinal("AuditorName")) ? "" : reader.GetString(reader.GetOrdinal("AuditorName")),
                    CampaignId  = reader.IsDBNull(reader.GetOrdinal("CampaignId")) ? null : reader.GetInt32(reader.GetOrdinal("CampaignId")),
                    Campaign    = reader.IsDBNull(reader.GetOrdinal("Campaign")) ? null : reader.GetString(reader.GetOrdinal("Campaign")),
                    FullPeriode = reader.IsDBNull(reader.GetOrdinal("FullPeriode")) ? null : reader.GetString(reader.GetOrdinal("FullPeriode")),
                    RecordLink  = reader.IsDBNull(reader.GetOrdinal("RecordLink")) ? null : reader.GetString(reader.GetOrdinal("RecordLink")),
                    ItemId      = reader.IsDBNull(reader.GetOrdinal("ItemId")) ? null : reader.GetInt32(reader.GetOrdinal("ItemId")),
                    ItemValue   = reader.IsDBNull(reader.GetOrdinal("ItemValue")) ? null : reader.GetDouble(reader.GetOrdinal("ItemValue")),
                    ItemMemo    = reader.IsDBNull(reader.GetOrdinal("ItemMemo")) ? null : reader.GetString(reader.GetOrdinal("ItemMemo")),
                    Question    = reader.IsDBNull(reader.GetOrdinal("Question")) ? null : reader.GetString(reader.GetOrdinal("Question")),
                    Section     = reader.IsDBNull(reader.GetOrdinal("Section")) ? null : reader.GetString(reader.GetOrdinal("Section")),
                    SectionId   = reader.IsDBNull(reader.GetOrdinal("SectionId")) ? null : reader.GetInt32(reader.GetOrdinal("SectionId")),
                });
            }

            return rows;
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Agents
        // ─────────────────────────────────────────────────────────────────────────
        public async Task<IEnumerable<AgentDto>> GetAgentsAsync(int userId, int userRole, int siteId, bool allSupervisors)
        {
            var agents = new List<AgentDto>();
            string sql = userRole switch
            {
                1 => "SELECT DISTINCT AgentId AS Id, Agent AS Name FROM [SQR_REC].[dbo].[Ls] ORDER BY Agent",
                2 => @"SELECT DISTINCT l.AgentId AS Id, l.Agent AS Name
                       FROM [SQR_REC].[dbo].[Ls] l
                       INNER JOIN [SQR_Admin].[dbo].[UsersAgent] ua ON ua.AgentId = l.AgentId
                       WHERE ua.UserId = @UserId ORDER BY l.Agent",
                3 when allSupervisors => @"SELECT DISTINCT l.AgentId AS Id, l.Agent AS Name
                       FROM [SQR_REC].[dbo].[Ls] l
                       INNER JOIN [SQR_Admin].[dbo].[UsersAgent] ua ON ua.AgentId = l.AgentId
                       WHERE ua.SiteId = @SiteId ORDER BY l.Agent",
                _ => @"SELECT DISTINCT l.AgentId AS Id, l.Agent AS Name
                       FROM [SQR_REC].[dbo].[Ls] l
                       INNER JOIN [SQR_Admin].[dbo].[UsersAgent] ua ON ua.AgentId = l.AgentId
                       WHERE ua.UserId = @UserId ORDER BY l.Agent"
            };

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@SiteId", siteId);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                agents.Add(new AgentDto { Id = reader.GetInt32(0), Name = reader.GetString(1) });

            return agents;
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Campagnes
        // ─────────────────────────────────────────────────────────────────────────
        public async Task<IEnumerable<CampaignDto>> GetCampaignsAsync(int userId, int siteId)
        {
            const string sql = @"
                SELECT c.Id, c.Description
                FROM [SQR_REC].[dbo].[Ls_CalledCampaign] c
                INNER JOIN [SQR_Admin].[dbo].[UsersCampagne] uc ON uc.CampagneId = c.CampagneDID
                WHERE uc.UserId = @UserId AND uc.SiteId = @SiteId AND c.Status = 1
                ORDER BY c.Description";

            var list = new List<CampaignDto>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@SiteId", siteId);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(new CampaignDto { Id = reader.GetInt32(0), Description = reader.GetString(1) });

            return list;
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Export (CSV simple — pour PDF/XLS utiliser une lib dédiée)
        // ─────────────────────────────────────────────────────────────────────────
        public async Task<byte[]> ExportAsync(StatistiqueExportDto request)
        {
            var data = (await GetStatistiqueDataAsync(request.Filter)).ToList();

            if (request.Format == "CSV")
            {
                var sb = new StringBuilder();
                sb.AppendLine("Date,Agent,Auditeur,Campagne,Score,Commentaire,Question,Section,Valeur item");
                foreach (var r in data)
                {
                    sb.AppendLine(string.Join(",",
                        r.CreateDate.ToString("dd/MM/yyyy"),
                        Quote(r.Agent), Quote(r.Auditor), Quote(r.Campaign ?? ""),
                        r.Score.ToString("F2"),
                        Quote(r.Memo ?? ""),
                        Quote(r.Question ?? ""), Quote(r.Section ?? ""),
                        r.ItemValue?.ToString("F2") ?? ""));
                }
                return Encoding.UTF8.GetBytes(sb.ToString());
            }

            // Pour les autres formats, retourner JSON encodé (à remplacer par une lib PDF/XLS)
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
        }

        private static string Quote(string s) => $"\"{s.Replace("\"", "\"\"")}\"";
    }
}