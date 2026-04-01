// ============================================================
// IEvaluationListRepository
// Interfaces — Module ListeEvaluation
// ============================================================

using scoring_Backend.DTO;

namespace scoring_Backend.Repositories.Interfaces.Evaluation
{
    public interface IEvaluationListRepository
    {
        // ── Liste des fiches Ls ─────────────────────────────
        Task<(List<LsFicheDto> Items, int TotalCount)> GetLsFichesAsync(
            int userId, int userRole, int userType, int userSiteId,string userLogin,
            EvaluationListFilterDto filter);

        // ── Surveys d'une fiche ─────────────────────────────
        Task<List<LsSurveyDto>> GetSurveysByLsIdAsync(int lsId);

        // ── Chargement d'une survey pour édition ────────────
        Task<List<SurveyItemDto>> GetSurveyItemsAsync(int surveyId);

        // ── Mise à jour d'une survey ────────────────────────
        Task<LsSurveyDto> UpdateSurveyAsync(int surveyId, int auditorId, UpdateSurveyDto dto);

        // ── Suppression d'une survey individuelle ───────────
        Task DeleteSurveyAsync(int surveyId);

        // ── Suppression d'une fiche complète ────────────────
        Task DeleteLsFicheAsync(int lsId);

        // ── Fiche agent (rapport de synthèse) ───────────────
        Task<AgentReportDto> GetAgentReportAsync(int lsId,int recordDataId = 0);

        // ── Vérification validité période ───────────────────
        Task<bool> IsLsPeriodValidAsync(int lsId);
    }
}