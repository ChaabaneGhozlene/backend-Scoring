using Microsoft.Data.SqlClient;
using scoring_Backend.Repositories.Interfaces.Statistique;
using scoring_Backend.DTO.Statistique;
using System.Text;
using System.Text.Json;

namespace scoring_Backend.Repositories.Implementations.Statistique
{
    public class StatistiqueRepository2 : IStatistiqueRepository2
    {
        private readonly string _connectionString;

        public StatistiqueRepository2(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("SqrScoring")
                ?? throw new InvalidOperationException("Connection string 'SqrScoring' not found.");
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Données principales
        // ─────────────────────────────────────────────────────────────────────────
        public async Task<IEnumerable<StatistiqueRowDto>> GetStatistiqueDataAsync(StatistiqueFilterDto f)
        {
            var rows = new List<StatistiqueRowDto>();

            var sql = new StringBuilder(@"
                SELECT
                    s.Id                            AS SurveyId,
                    s.CreateDate,
                    s.Score,
                    s.Memo,
                    l.AgentId,
                    l.Agent,
                    l.Auditor                       AS AuditorId,
                    u.FirstName + ' ' + u.LastName  AS AuditorName,
                    l.CalledCampaignId              AS CampaignId,
                    c.Description                   AS Campaign,
                    l.StartPeriode,
                    l.EndPeriode,
                    si.Id                           AS ItemId,
                    si.Value                        AS ItemValue,
                    si.Memo                         AS ItemMemo,
                    ti.Question,
                    ti.Id                           AS QuestionId,
                    tg.Description                  AS Section,
                    tg.Id                           AS SectionId
                FROM [SQR_REC].[dbo].[Ls_survey] s
                INNER JOIN [SQR_REC].[dbo].[Ls] l           ON l.Id = s.LsId
                LEFT JOIN [SQR_Admin].[dbo].[Users] u        ON u.ID = l.Auditor
                LEFT JOIN [SQR_REC].[dbo].[Ls_CalledCampaign] c ON c.Id = l.CalledCampaignId
                LEFT JOIN [SQR_REC].[dbo].[Ls_surveyItem] si ON si.SurveyId = s.Id
                LEFT JOIN [SQR_REC].[dbo].[Ls_templateItem] ti ON ti.Id = si.ItemId
                LEFT JOIN [SQR_REC].[dbo].[Ls_templateItemGroup] tg ON tg.Id = ti.GroupId
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
                sql.Append(" AND l.CalledCampaignId = @CampaignId");
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
                sql.Append(@"
                  AND l.CalledCampaignId IN (
                      SELECT uc.CampagneId FROM [SQR_Admin].[dbo].[UsersCampagne] uc
                      WHERE uc.UserId = @UserId AND uc.SiteId = @SiteId
                  )");
                cmd.Parameters.AddWithValue("@UserId", f.UserId);
                cmd.Parameters.AddWithValue("@SiteId", f.SiteId);
            }
            else if (f.UserRole == 3 && !f.AllSupervisors)
            {
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
                    SurveyId     = reader.GetInt32(reader.GetOrdinal("SurveyId")),
                    CreateDate   = reader.GetDateTime(reader.GetOrdinal("CreateDate")),
                    Score        = reader.IsDBNull(reader.GetOrdinal("Score"))
                                       ? 0
                                       : (double)reader.GetFloat(reader.GetOrdinal("Score")),
                    Memo         = reader.IsDBNull(reader.GetOrdinal("Memo"))
                                       ? null : reader.GetString(reader.GetOrdinal("Memo")),
                    AgentId      = reader.IsDBNull(reader.GetOrdinal("AgentId"))
                                       ? 0 : reader.GetInt32(reader.GetOrdinal("AgentId")),
                    Agent        = reader.IsDBNull(reader.GetOrdinal("Agent"))
                                       ? "" : reader.GetString(reader.GetOrdinal("Agent")),
                    AuditorId    = reader.IsDBNull(reader.GetOrdinal("AuditorId"))
                                       ? 0 : reader.GetInt32(reader.GetOrdinal("AuditorId")),
                    Auditor      = reader.IsDBNull(reader.GetOrdinal("AuditorName"))
                                       ? "" : reader.GetString(reader.GetOrdinal("AuditorName")),
                    CampaignId   = reader.IsDBNull(reader.GetOrdinal("CampaignId"))
                                       ? null : reader.GetInt32(reader.GetOrdinal("CampaignId")),
                    Campaign     = reader.IsDBNull(reader.GetOrdinal("Campaign"))
                                       ? null : reader.GetString(reader.GetOrdinal("Campaign")),
                    StartPeriode = reader.IsDBNull(reader.GetOrdinal("StartPeriode"))
                                       ? null : reader.GetDateTime(reader.GetOrdinal("StartPeriode")),
                    EndPeriode   = reader.IsDBNull(reader.GetOrdinal("EndPeriode"))
                                       ? null : reader.GetDateTime(reader.GetOrdinal("EndPeriode")),
                    ItemId       = reader.IsDBNull(reader.GetOrdinal("ItemId"))
                                       ? null : reader.GetInt32(reader.GetOrdinal("ItemId")),
                    ItemValue    = reader.IsDBNull(reader.GetOrdinal("ItemValue"))
                                       ? null : (double?)reader.GetFloat(reader.GetOrdinal("ItemValue")),
                    ItemMemo     = reader.IsDBNull(reader.GetOrdinal("ItemMemo"))
                                       ? null : reader.GetString(reader.GetOrdinal("ItemMemo")),
                    Question     = reader.IsDBNull(reader.GetOrdinal("Question"))
                                       ? null : reader.GetString(reader.GetOrdinal("Question")),
                    Section      = reader.IsDBNull(reader.GetOrdinal("Section"))
                                       ? null : reader.GetString(reader.GetOrdinal("Section")),
                    SectionId    = reader.IsDBNull(reader.GetOrdinal("SectionId"))
                                       ? null : reader.GetInt32(reader.GetOrdinal("SectionId")),
                });
            }

            return rows;
        }

// ─────────────────────────────────────────────────────────────────────────
// Agents
// ─────────────────────────────────────────────────────────────────────────
public async Task<IEnumerable<AgentDto>> GetAgentsAsync(
    int userId, int userRole, int siteId, bool allSupervisors)
{
    var agents = new List<AgentDto>();

    string sql = userRole switch
    {
        1 => @"SELECT DISTINCT AgentId AS Id, Agent AS Name
               FROM [SQR_REC].[dbo].[Ls]
               WHERE AgentId IS NOT NULL
               ORDER BY Agent",

        2 => @"SELECT DISTINCT l.AgentId AS Id, l.Agent AS Name
               FROM [SQR_REC].[dbo].[Ls] l
               INNER JOIN [SQR_Admin].[dbo].[UsersAgent] ua ON ua.AgentId = l.AgentId
               WHERE ua.UserId = @UserId
               ORDER BY l.Agent",

        3 when allSupervisors => @"SELECT DISTINCT l.AgentId AS Id, l.Agent AS Name
               FROM [SQR_REC].[dbo].[Ls] l
               INNER JOIN [SQR_Admin].[dbo].[UsersAgent] ua ON ua.AgentId = l.AgentId
               WHERE ua.SiteId = @SiteId
               ORDER BY l.Agent",

        _ => @"SELECT DISTINCT l.AgentId AS Id, l.Agent AS Name
               FROM [SQR_REC].[dbo].[Ls] l
               INNER JOIN [SQR_Admin].[dbo].[UsersAgent] ua ON ua.AgentId = l.AgentId
               WHERE ua.UserId = @UserId
               ORDER BY l.Agent"
    };

    using var conn = new SqlConnection(_connectionString);
    using var cmd = new SqlCommand(sql, conn);

    // ✅ Ajouter les paramètres uniquement si nécessaires
    if (sql.Contains("@UserId"))
        cmd.Parameters.AddWithValue("@UserId", userId);

    if (sql.Contains("@SiteId"))
        cmd.Parameters.AddWithValue("@SiteId", siteId);

    await conn.OpenAsync();
    using var reader = await cmd.ExecuteReaderAsync();
    while (await reader.ReadAsync())
        agents.Add(new AgentDto
        {
            Id   = reader.GetInt32(0),
            Name = reader.GetString(1)
        });

    return agents;
}

// ─────────────────────────────────────────────────────────────────────────
// Campagnes — utilise SqrAdminContext + jointure cross-database
// ─────────────────────────────────────────────────────────────────────────
public async Task<IEnumerable<CampaignDto>> GetCampaignsAsync(int userId, int siteId)
{
    // ✅ Log pour déboguer
    Console.WriteLine($"[Campaigns] userId={userId}, siteId={siteId}");

    const string sql = @"
        SELECT DISTINCT c.Id, c.Description
        FROM [SQR_REC].[dbo].[Ls_CalledCampaign] c
        INNER JOIN [SQR_Admin].[dbo].[UsersCampagne] uc
            ON uc.CampagneId = CAST(c.Id AS NVARCHAR(64))
        WHERE uc.UserId = @UserId AND uc.SiteId = @SiteId AND c.Status = 1
        ORDER BY c.Description";

    var list = new List<CampaignDto>();
    using var conn = new SqlConnection(_connectionString);
    using var cmd  = new SqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@UserId", userId);
    cmd.Parameters.AddWithValue("@SiteId", siteId);

    await conn.OpenAsync();
    using var reader = await cmd.ExecuteReaderAsync();
    while (await reader.ReadAsync())
        list.Add(new CampaignDto
        {
            Id          = reader.GetInt32(0),
            Description = reader.GetString(1)
        });

    // ✅ Log résultat
    Console.WriteLine($"[Campaigns] {list.Count} résultats trouvés");
    return list;
}

        // ─────────────────────────────────────────────────────────────────────────
        // Export
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

            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
        }

        private static string Quote(string s) => $"\"{s.Replace("\"", "\"\"")}\"";
    }
}