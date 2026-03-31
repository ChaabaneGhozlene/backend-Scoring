using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using scoring_Backend.DTO;
using scoring_Backend.Models.Scoring;
using scoring_Backend.Repositories.Interfaces.Configuration;

namespace scoring_Backend.Repositories.Implementations.Configuration
{
    /// <summary>
    /// Implémentation de IAgentTeamRepository.
    /// Toutes les requêtes ciblent la base SQR_REC (SqrScoringContext).
    /// Tables concernées :
    ///   - dbo.tListAgentTeam  : groupes d'agents
    ///   - dbo.tAgentTeams     : membres d'un groupe
    ///   - dbo.tListAgents     : référentiel agents
    /// </summary>
    public class AgentTeamRepository : IAgentTeamRepository
    {
        private readonly SqrScoringContext _db;

        public AgentTeamRepository(SqrScoringContext db)
        {
            _db = db;
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private SqlConnection OpenConnection()
        {
            var conn = new SqlConnection(_db.Database.GetConnectionString());
            return conn;
        }

        // ════════════════════════════════════════════════════════════════════
        // GROUPES (tListAgentTeam)
        // ════════════════════════════════════════════════════════════════════

   public async Task<IEnumerable<AgentTeamDto>> GetAllTeamsAsync(int userId, string userRole)
{
    bool seeAll = userRole == "SuperAdmin";

    string sql = seeAll
        ? @"SELECT  t.Id,
                    t.Description,
                    t.IdSite,
                    ISNULL(s.customer, '') AS SiteDescription
            FROM    dbo.tListAgentTeam t
            LEFT JOIN (
                SELECT DISTINCT customerId, customer
                FROM   dbo.tListAgents
            ) s ON s.customerId = t.IdSite
            ORDER BY t.Description"

        : @"SELECT  t.Id,
                    t.Description,
                    t.IdSite,
                    ISNULL(s.customer, '') AS SiteDescription
            FROM    dbo.tListAgentTeam t
            LEFT JOIN (
                SELECT DISTINCT customerId, customer
                FROM   dbo.tListAgents
            ) s ON s.customerId = t.IdSite
            INNER JOIN [SQR_Admin].[dbo].[Users] u
                    ON u.Id     = @UserId
                   AND u.SiteId = t.IdSite
            ORDER BY t.Description";

    var list = new List<AgentTeamDto>();
    await using var conn = OpenConnection();
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(sql, conn);

    if (!seeAll)
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

    await using var rdr = await cmd.ExecuteReaderAsync();
    while (await rdr.ReadAsync())
        list.Add(MapTeam(rdr));

    return list;
}
        public async Task<AgentTeamDto?> GetTeamByIdAsync(int id)
        {
            const string sql = @"
                SELECT  t.Id,
                        t.Description,
                        t.IdSite,
                        ISNULL(s.customer, '') AS SiteDescription
                FROM    dbo.tListAgentTeam t
                LEFT JOIN (
                    SELECT DISTINCT customerId, customer
                    FROM   dbo.tListAgents
                ) s ON s.customerId = t.IdSite
                WHERE t.Id = @Id";

            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd  = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await using var rdr  = await cmd.ExecuteReaderAsync();
            return await rdr.ReadAsync() ? MapTeam(rdr) : null;
        }

        public async Task<bool> TeamExistsByDescriptionAsync(string description)
        {
            const string sql = @"
                SELECT COUNT(1) FROM dbo.tListAgentTeam
                WHERE Description = @Description";

            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd  = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Description", description);
            return (int)(await cmd.ExecuteScalarAsync())! > 0;
        }

        /// <summary>
        /// Crée un groupe puis insère les membres dans tAgentTeams.
        /// Reprend exactement la logique de CPLNewModel_Callback / NewModel.
        /// </summary>
        public async Task<int> CreateTeamAsync(CreateAgentTeamDto dto)
        {
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var tran = conn.BeginTransaction();
            try
            {
                // 1. Insérer le groupe
                const string insertTeam = @"
                    INSERT INTO dbo.tListAgentTeam (Description, IdSite)
                    VALUES (@Description, @IdSite);
                    SELECT SCOPE_IDENTITY();";

                int teamId;
                await using (var cmd = new SqlCommand(insertTeam, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@Description", dto.Description);
                    cmd.Parameters.AddWithValue("@IdSite",      dto.IdSite);
                    teamId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }

                // 2. Insérer les membres sélectionnés
                foreach (var oid in dto.AgentOids)
                    await InsertMemberAsync(conn, tran, teamId, oid);

                await tran.CommitAsync();
                return teamId;
            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Met à jour la description du groupe puis remplace TOUS ses membres.
        /// Reprend la logique de GvCalledCampaign_CustomCallback / "EditModel".
        /// </summary>
        public async Task UpdateTeamAsync(int id, UpdateAgentTeamDto dto)
        {
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var tran = conn.BeginTransaction();
            try
            {
                // 1. Mettre à jour la description
                const string updateTeam = @"
                    UPDATE dbo.tListAgentTeam
                    SET    Description = @Description
                    WHERE  Id = @Id";

                await using (var cmd = new SqlCommand(updateTeam, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@Id",          id);
                    cmd.Parameters.AddWithValue("@Description", dto.Description);
                    await cmd.ExecuteNonQueryAsync();
                }

                // 2. Supprimer les anciens membres
                const string deleteMembers = @"
                    DELETE FROM dbo.tAgentTeams WHERE TeamId = @TeamId";

                await using (var cmd = new SqlCommand(deleteMembers, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@TeamId", id);
                    await cmd.ExecuteNonQueryAsync();
                }

                // 3. Ré-insérer les membres sélectionnés
                foreach (var oid in dto.AgentOids)
                    await InsertMemberAsync(conn, tran, id, oid);

                await tran.CommitAsync();
            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Un groupe peut être supprimé s'il n'a aucun membre actif dans tAgentTeams.
        /// (Adapté depuis CPlDeleteModel_Callback)
        /// </summary>
        public async Task<bool> CanDeleteTeamAsync(int id)
        {
            const string sql = @"
                SELECT COUNT(1) FROM dbo.tAgentTeams WHERE TeamId = @Id";

            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd  = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            return (int)(await cmd.ExecuteScalarAsync())! == 0;
        }

        /// <summary>
        /// Supprime d'abord les membres (tAgentTeams) puis le groupe.
        /// Reprend la logique de GvCalledCampaign_CustomCallback / "DeleteModel".
        /// </summary>
        public async Task DeleteTeamAsync(int id)
        {
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var tran = conn.BeginTransaction();
            try
            {
                // 1. Supprimer les membres
                const string deleteMembers = @"
                    DELETE FROM dbo.tAgentTeams WHERE TeamId = @Id";

                await using (var cmd = new SqlCommand(deleteMembers, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    await cmd.ExecuteNonQueryAsync();
                }

                // 2. Supprimer le groupe
                const string deleteTeam = @"
                    DELETE FROM dbo.tListAgentTeam WHERE Id = @Id";

                await using (var cmd = new SqlCommand(deleteTeam, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    await cmd.ExecuteNonQueryAsync();
                }

                await tran.CommitAsync();
            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }
        }

        // ════════════════════════════════════════════════════════════════════
        // MEMBRES (tAgentTeams)
        // ════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Retourne les membres d'un groupe avec leurs informations agents.
        /// Reprend la requête de GetGvLs().
        /// </summary>
        public async Task<IEnumerable<AgentTeamMemberDto>> GetMembersByTeamAsync(int teamId)
        {
            // COLLATE French_CI_AS résout le conflit entre French_CI_AS et French_BIN
            // sur la colonne Oid (French_BIN) vs AgentOid (French_CI_AS)
            const string sql = @"
                SELECT  ag.Id       AS Id,
                        ag.AgentOid AS AgentOid,
                        ls.Ident    AS AgentId,
                        CONCAT(ls.Prenom, ' ', ls.Nom) AS AgentName
                FROM    dbo.tAgentTeams  ag
                JOIN    dbo.tListAgents  ls
                        ON ls.Oid COLLATE French_CI_AS = ag.AgentOid COLLATE French_CI_AS
                WHERE   ag.TeamId = @TeamId
                ORDER BY AgentName";

            var list = new List<AgentTeamMemberDto>();
            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd  = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@TeamId", teamId);
            await using var rdr  = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
                list.Add(new AgentTeamMemberDto(
                    rdr.GetInt32(0),
                    rdr.GetString(1),
                    rdr.GetInt32(2),
                    rdr.GetString(3)));
            return list;
        }

        // ════════════════════════════════════════════════════════════════════
        // LOOKUPS
        // ════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Agents du site qui ne sont PAS dans le groupe indiqué.
        /// Reprend la requête de CPLEditModel_Callback et lbAvailable_Callback.
        /// </summary>
        public async Task<IEnumerable<AvailableAgentDto>> GetAvailableAgentsAsync(
            int customerId, int? excludeTeamId = null)
        {
            // Si excludeTeamId fourni : exclut les agents déjà dans ce groupe
            // COLLATE French_CI_AS sur les deux colonnes pour éviter le conflit de collation
            const string sqlWithExclude = @"
                SELECT  la.Oid                            AS Oid,
                        CONCAT(la.Prenom, ' ', la.Nom)    AS Name
                FROM    dbo.tListAgents la
                WHERE   la.customerId = @CustomerId
                  AND   la.Oid COLLATE French_CI_AS NOT IN (
                            SELECT at2.AgentOid COLLATE French_CI_AS
                            FROM   dbo.tAgentTeams at2
                            WHERE  at2.TeamId = @TeamId
                              AND  at2.AgentOid IS NOT NULL
                        )
                ORDER BY Name";

            const string sqlAll = @"
                SELECT  la.Oid                            AS Oid,
                        CONCAT(la.Prenom, ' ', la.Nom)    AS Name
                FROM    dbo.tListAgents la
                WHERE   la.customerId = @CustomerId
                ORDER BY Name";

            var sql  = excludeTeamId.HasValue ? sqlWithExclude : sqlAll;
            var list = new List<AvailableAgentDto>();

            await using var conn = OpenConnection();
            await conn.OpenAsync();
            await using var cmd  = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CustomerId", customerId);
            if (excludeTeamId.HasValue)
                cmd.Parameters.AddWithValue("@TeamId", excludeTeamId.Value);

            await using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
                list.Add(new AvailableAgentDto(rdr.GetString(0), rdr.GetString(1)));
            return list;
        }

        /// <summary>
        /// Liste des sites distincts depuis tListAgents.
        /// Reprend la logique de GetSite() (rôle Admin → tous les sites).
        /// </summary>
      public async Task<IEnumerable<AgentSiteDto>> GetSitesAsync(int userId, string userRole)
{
    bool seeAll = userRole == "SuperAdmin";

    string sql = seeAll
        ? @"SELECT DISTINCT customerId AS Id, customer AS Description
            FROM   dbo.tListAgents
            ORDER BY Description"

        : @"SELECT DISTINCT la.customerId AS Id, la.customer AS Description
            FROM   dbo.tListAgents la
            INNER JOIN [SQR_Admin].[dbo].[Users] u
                    ON u.Id     = @UserId
                   AND u.SiteId = la.customerId
            ORDER BY Description";

    var list = new List<AgentSiteDto>();
    await using var conn = OpenConnection();
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(sql, conn);

    if (!seeAll)
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

    await using var rdr = await cmd.ExecuteReaderAsync();
    while (await rdr.ReadAsync())
        list.Add(new AgentSiteDto(rdr.GetInt32(0), rdr.GetString(1)));

    return list;
}

        // ════════════════════════════════════════════════════════════════════
        // HELPERS PRIVÉS
        // ════════════════════════════════════════════════════════════════════

        private static AgentTeamDto MapTeam(SqlDataReader r) => new(
            r.GetInt32(0),
            r.GetString(1),
            r.GetInt32(2),
            r.GetString(3));

        /// <summary>
        /// Insère un membre dans tAgentTeams et résout tListAgents via l'Oid.
        /// Reprend exactement la logique foreach lbChoosen.Items dans l'ancien code.
        /// </summary>
        private static async Task InsertMemberAsync(
            SqlConnection conn, SqlTransaction tran,
            int teamId, string agentOid)
        {
            // Récupérer l'Ident (AgentId numérique) depuis l'Oid
            // COLLATE French_CI_AS pour éviter le conflit sur la colonne Oid (French_BIN)
            const string getIdent = @"
                SELECT Ident FROM dbo.tListAgents
                WHERE Oid COLLATE French_CI_AS = @Oid COLLATE French_CI_AS";

            int agentIdent;
            await using (var cmd = new SqlCommand(getIdent, conn, tran))
            {
                cmd.Parameters.AddWithValue("@Oid", agentOid);
                var result = await cmd.ExecuteScalarAsync();
                if (result == null || result == DBNull.Value) return; // agent introuvable → skip
                agentIdent = Convert.ToInt32(result);
            }

            const string insertMember = @"
                INSERT INTO dbo.tAgentTeams (TeamId, AgentOid, AgentId)
                VALUES (@TeamId, @AgentOid, @AgentId)";

            await using (var cmd = new SqlCommand(insertMember, conn, tran))
            {
                cmd.Parameters.AddWithValue("@TeamId",   teamId);
                cmd.Parameters.AddWithValue("@AgentOid", agentOid);
                cmd.Parameters.AddWithValue("@AgentId",  agentIdent);
                await cmd.ExecuteNonQueryAsync();
            }

        }
        public async Task RemoveMembersAsync(int teamId, IEnumerable<string> agentOids)
{
    await using var conn = OpenConnection();
    await conn.OpenAsync();
    await using var tran = conn.BeginTransaction();
    try
    {
        foreach (var oid in agentOids)
        {
            const string sql = @"
                DELETE FROM dbo.tAgentTeams
                WHERE TeamId = @TeamId
                AND AgentOid COLLATE French_CI_AS = @AgentOid COLLATE French_CI_AS";

            await using var cmd = new SqlCommand(sql, conn, tran);
            cmd.Parameters.AddWithValue("@TeamId",   teamId);
            cmd.Parameters.AddWithValue("@AgentOid", oid);
            await cmd.ExecuteNonQueryAsync();
        }
        await tran.CommitAsync();
    }
    catch
    {
        await tran.RollbackAsync();
        throw;
    }
}
    }
    
}