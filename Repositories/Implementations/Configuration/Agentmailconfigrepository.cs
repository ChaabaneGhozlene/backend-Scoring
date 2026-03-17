using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using scoring_Backend.DTO.Configuration;
using scoring_Backend.Repositories.Interfaces.Configuration;
using System.Data;

namespace scoring_Backend.Repositories.Implementations.Configuration
{
    /// <summary>
    /// Repository pour la configuration email des agents (Notification Setting).
    /// Connection string : SqrScoring → Database=SQR_REC
    ///
    /// Visibilité par rôle (alignée avec JwtService.MapRole) :
    ///   SuperAdmin  (Role=1) → tous les agents
    ///   AdminSite   (Role=2) → tous les agents
    ///   Superviseur (Role=3) → agents assignés via UsersAgent
    ///   Agent       (Role=4) → agents assignés via UsersAgent
    /// </summary>
    public class AgentMailConfigRepository : IAgentMailConfigRepository
    {
        private readonly string _connectionString;

        public AgentMailConfigRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqrScoring")
                ?? throw new InvalidOperationException("Connection string 'SqrScoring' not found.");
        }

        // ──────────────────────────────────────────────────────────────────────
        // GET liste agents
        // ─── CHANGEMENT : paramètre string userRole (au lieu de int) ──────────
        // SuperAdmin / AdminSite → tous les agents (pas de filtre UserId)
        // Superviseur / Agent   → agents liés via UsersAgent WHERE UserId=@UserId
        // ──────────────────────────────────────────────────────────────────────
        public async Task<IEnumerable<AgentMailConfigDto>> GetAgentsWithEmailAsync(
            int userId, string userRole)   // ← string, correspond à l'interface
        {
            var result = new List<AgentMailConfigDto>();

            // SuperAdmin et AdminSite voient tous les agents sans filtre
            bool seeAll = userRole == "SuperAdmin" || userRole == "AdminSite";

            string sql = seeAll
                ? @"SELECT la.Ident AS Id,
                           la.Oid   AS Oid,
                           CONCAT(la.Prenom, ' ', la.Nom) AS Agent,
                           lae.Email AS Email
                    FROM   [dbo].[tListAgents] la
                    LEFT JOIN [dbo].[tListAgentEmail] lae
                           ON la.Oid   = lae.OIDAgent
                          AND la.Ident = lae.IdAgent"

                : @"SELECT DISTINCT
                           la.Ident AS Id,
                           la.Oid   AS Oid,
                           CONCAT(la.Prenom, ' ', la.Nom) AS Agent,
                           lae.Email AS Email
                    FROM   [dbo].[tListAgents] la
                    LEFT JOIN [dbo].[tListAgentEmail] lae
                           ON la.Oid   = lae.OIDAgent
                          AND la.Ident = lae.IdAgent
                    INNER JOIN [SQR_Admin].[dbo].[UsersAgent] ua
                           ON la.Ident = ua.AgentId
                    WHERE  ua.UserId = @UserId";

            await using var conn = new SqlConnection(_connectionString);
            await using var cmd  = new SqlCommand(sql, conn);

            if (!seeAll)
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

            await conn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new AgentMailConfigDto
                {
                    Id    = reader.GetInt32(reader.GetOrdinal("Id")),
                    Oid   = reader.GetString(reader.GetOrdinal("Oid")),
                    Agent = reader.GetString(reader.GetOrdinal("Agent")),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("Email")).Trim()
                });
            }

            return result;
        }

        // ──────────────────────────────────────────────────────────────────────
        // GET détail agent pour popup édition (inchangé)
        // ──────────────────────────────────────────────────────────────────────
        public async Task<AgentMailEditDetailDto?> GetAgentEditDetailAsync(string oid)
        {
            const string sqlAgent = @"
                SELECT Ident,
                       CONCAT(Prenom, ' ', Nom) AS FullName
                FROM   [dbo].[tListAgents]
                WHERE  Oid = @Oid";

            const string sqlEmail = @"
                SELECT Email
                FROM   [dbo].[tListAgentEmail]
                WHERE  OIDAgent = @Oid
                  AND  IdAgent  = @AgentId";

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            // 1) Récupère l'agent
            await using var cmdAgent = new SqlCommand(sqlAgent, conn);
            cmdAgent.Parameters.Add("@Oid", SqlDbType.NVarChar, 100).Value = oid;

            await using var readerAgent = await cmdAgent.ExecuteReaderAsync();
            if (!await readerAgent.ReadAsync())
                return null;

            var detail = new AgentMailEditDetailDto
            {
                Oid      = oid,
                FullName = readerAgent.GetString(readerAgent.GetOrdinal("FullName")),
                Ident    = readerAgent.GetInt32(readerAgent.GetOrdinal("Ident"))
            };
            await readerAgent.CloseAsync();

            // 2) Récupère l'email associé s'il existe
            await using var cmdEmail = new SqlCommand(sqlEmail, conn);
            cmdEmail.Parameters.Add("@Oid",     SqlDbType.NVarChar, 100).Value = oid;
            cmdEmail.Parameters.Add("@AgentId", SqlDbType.Int).Value           = detail.Ident;

            await using var readerEmail = await cmdEmail.ExecuteReaderAsync();
            if (await readerEmail.ReadAsync() && !readerEmail.IsDBNull(0))
                detail.Email = readerEmail.GetString(0).Trim();

            return detail;
        }

        // ──────────────────────────────────────────────────────────────────────
        // UPSERT email agent (inchangé)
        // ──────────────────────────────────────────────────────────────────────
        public async Task UpsertAgentEmailAsync(UpdateAgentEmailDto dto)
        {
            const string sqlCheck = @"
                SELECT COUNT(1)
                FROM   [dbo].[tListAgentEmail]
                WHERE  IdAgent  = @AgentId
                  AND  OIDAgent = @Oid";

            const string sqlUpdate = @"
                UPDATE [dbo].[tListAgentEmail]
                SET    Email = @Email
                WHERE  IdAgent  = @AgentId
                  AND  OIDAgent = @Oid";

            const string sqlInsert = @"
                INSERT INTO [dbo].[tListAgentEmail] (IdAgent, OIDAgent, Email)
                VALUES (@AgentId, @Oid, @Email)";

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var transaction = conn.BeginTransaction();

            try
            {
                await using var cmdCheck = new SqlCommand(sqlCheck, conn, transaction);
                cmdCheck.Parameters.Add("@AgentId", SqlDbType.Int).Value           = dto.AgentId;
                cmdCheck.Parameters.Add("@Oid",     SqlDbType.NVarChar, 100).Value = dto.Oid;

                int    count = (int)(await cmdCheck.ExecuteScalarAsync() ?? 0);
                string sql   = count > 0 ? sqlUpdate : sqlInsert;

                await using var cmdUpsert = new SqlCommand(sql, conn, transaction);
                cmdUpsert.Parameters.Add("@AgentId", SqlDbType.Int).Value           = dto.AgentId;
                cmdUpsert.Parameters.Add("@Oid",     SqlDbType.NVarChar, 100).Value = dto.Oid;
                cmdUpsert.Parameters.Add("@Email",   SqlDbType.NVarChar, 256).Value = dto.Email;

                await cmdUpsert.ExecuteNonQueryAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}