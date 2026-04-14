using scoring_Backend.DTO.Statistique;

namespace scoring_Backend.Repositories.Interfaces.Statistique
{
    public interface IStatistiqueRepository2
    {
        /// <summary>
        /// Requête principale : retourne les lignes plates depuis Ls_survey + Ls_surveyItem.
        /// Tous les anciens rapports (NoteGlobale, SuiviColab, SuiviSuperviseur, ProgramLevel…)
        /// consomment cet unique endpoint avec des filtres différents.
        /// </summary>
        Task<IEnumerable<StatistiqueRowDto>> GetStatistiqueDataAsync(StatistiqueFilterDto filter);

        Task<IEnumerable<AgentDto>> GetAgentsAsync(int userId, int userRole, int siteId, bool allSupervisors);

        Task<IEnumerable<CampaignDto>> GetCampaignsAsync(int userId, int siteId);

        Task<byte[]> ExportAsync(StatistiqueExportDto request);
    }
}