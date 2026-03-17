using scoring_Backend.DTO;

namespace scoring_Backend.Repositories.Interfaces.Configuration
{
    public interface IAgentTeamRepository
    {
        // ── Groupes ──────────────────────────────────────────────────────────
        Task<IEnumerable<AgentTeamDto>>       GetAllTeamsAsync();
        Task<AgentTeamDto?>                   GetTeamByIdAsync(int id);
        Task<bool>                            TeamExistsByDescriptionAsync(string description);
        Task<int>                             CreateTeamAsync(CreateAgentTeamDto dto);
        Task                                  UpdateTeamAsync(int id, UpdateAgentTeamDto dto);
        Task<bool>                            CanDeleteTeamAsync(int id);
        Task                                  DeleteTeamAsync(int id);

        // ── Membres ──────────────────────────────────────────────────────────
        Task<IEnumerable<AgentTeamMemberDto>> GetMembersByTeamAsync(int teamId);

        // ── Lookups ──────────────────────────────────────────────────────────

        /// <summary>
        /// Agents du site <paramref name="customerId"/> qui ne sont PAS encore
        /// dans le groupe <paramref name="excludeTeamId"/> (null = aucun filtre).
        /// </summary>
        Task<IEnumerable<AvailableAgentDto>>  GetAvailableAgentsAsync(
            int customerId, int? excludeTeamId = null);

        /// <summary>Sites distincts (customerId + customer) depuis tListAgents.</summary>
        Task<IEnumerable<AgentSiteDto>>       GetSitesAsync();
        Task RemoveMembersAsync(int teamId, IEnumerable<string> agentOids);
    }
}