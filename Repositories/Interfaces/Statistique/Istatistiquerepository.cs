using scoring_Backend.DTO.Statistique;

namespace scoring_Backend.Repositories.Interfaces.Statistique
{
    public interface IStatistiqueRepository
    {
        // ─── Section Stats ─────────────────────────────────────────────────
        Task<IEnumerable<SectionStatsDto>> GetSectionStatsAsync(
            StatFilterDto filter, int userId, int userRole, int siteId);

        // ─── Agent Score ───────────────────────────────────────────────────
        Task<IEnumerable<AgentScoreDto>> GetAgentScoresAsync(
            StatFilterDto filter, int userId, int userRole, int siteId, string sortDirection);

        // ─── Program Level ─────────────────────────────────────────────────
        Task<IEnumerable<ProgramLevelDto>> GetProgramLevelAsync(
            StatFilterDto filter, int userId, int userRole, int siteId, bool allSupervisors);

        // ─── Coaching Sheet ────────────────────────────────────────────────
        Task<IEnumerable<CoachingSheetDto>> GetCoachingSheetAsync(
            StatFilterDto filter, int agentId, int supervisorId, int userRole, bool allSupervisors);

        // ─── Coaching Analysis ─────────────────────────────────────────────
        Task<IEnumerable<CoachingAnalysisDto>> GetCoachingAnalysisAsync(
            StatFilterDto filter, int agentId, int userId, int userRole, bool allSupervisors);

        // ─── Coaching Summary ──────────────────────────────────────────────
        Task<IEnumerable<CoachingSummaryDto>> GetCoachingSummaryAsync(
            StatFilterDto filter, int agentId, int userId, int userRole, bool allSupervisors);
        Task<IEnumerable<SupervisorDto>>      GetSupervisorsAsync(int userId, int userRole, int siteId); // ✅ ajouté

        // ─── Agent List ────────────────────────────────────────────────────
        Task<IEnumerable<AgentListDto>> GetAgentListAsync(
            int userId, int userRole, int siteId, bool allSupervisors);
    }
    
}