// ============================================================
// Repositories/Interfaces/Evaluation/IEvaluationRepository.cs
// ============================================================

using scoring_Backend.DTO;

namespace scoring_Backend.Repositories.Interfaces.Evaluation
{
    public interface IEvaluationRepository
    {
        // ── Ouverture / création d'une évaluation ──────────────
        /// <summary>
        /// Ouvre ou crée une fiche d'évaluation pour un enregistrement.
        /// Retourne la grille pré-remplie (ou un message d'erreur).
        /// </summary>
        Task<OpenEvaluationResponseDto> OpenEvaluationAsync(
            int recordId, int userId, string userLogin);

        // ── Sauvegarde d'une évaluation ────────────────────────
        /// <summary>
        /// Sauvegarde les notes, recalcule le score et envoie
        /// l'e-mail à l'agent si configuré.
        /// </summary>
        Task<SaveEvaluationResultDto> SaveEvaluationAsync(
            SaveEvaluationDto dto, int userId, string userLogin);

        // ── Référentiels ───────────────────────────────────────
        Task<List<CategoryDto>>   GetCategoriesAsync();
        Task<List<CallReasonDto>> GetCallReasonsAsync();

        // ── Agents visibles par l'utilisateur ─────────────────
        Task<List<AgentDto>> GetAgentsAsync(int userId, string userRole, int userSite);
   
        // ── Campagnes qualité visibles par l'utilisateur ──────
        Task<List<CampaignQualityDto>> GetCampaignQualitiesAsync(
            string userId, int userSite, string userRole);

        // ── Statuts Hermess pour requalification ──────────────
        Task<List<CallStatusItemDto>> GetCallStatusItemsAsync(
            string customerId, string campaignId, int callType);

        // ── Requalification ────────────────────────────────────
        Task RequalifyRecordAsync(RequalificationDto dto, int userId);
            Task<byte[]?> BuildZipAsync(List<int> recordIds);
Task<MultiSurveyResponseDto> GetAllSurveysForRecordAsync(int recordId);
 Task<RecordFileDto?> GetRecordFilePathAsync(int recordId);
Task<RecordScreenDto?> GetRecordScreenPathAsync(int recordId);
    }
}