
using scoring_Backend.DTO;

namespace scoring_Backend.Repositories.Interfaces.Configuration
{
    public interface IConfigurationCampagnesRepository
    {
        // ── Templates ────────────────────────────────────────────────────────
        Task<IEnumerable<LsTemplateDto>> GetAllTemplatesAsync();
        Task<LsTemplateDto?> GetTemplateByIdAsync(int id);
        Task<bool> TemplateExistsByDescriptionAsync(string description);
        Task<int> CreateTemplateAsync(CreateLsTemplateDto dto);
        Task UpdateTemplateAsync(int id, UpdateLsTemplateDto dto);
        Task<bool> CanDeleteTemplateAsync(int id);
        Task DeleteTemplateAsync(int id);

        // ── Version / Change Model ────────────────────────────────────────────
        Task<bool> HasPendingVersionAsync(int templateId);
        Task<int> ChangeModelAsync(int originalTemplateId, ChangeModelDto dto);

        // ── Item Groups ───────────────────────────────────────────────────────
        Task<IEnumerable<ItemGroupDto>> GetItemGroupsByTemplateAsync(int templateId);
        Task<int> CreateItemGroupAsync(int templateId, CreateItemGroupDto dto);
        Task UpdateItemGroupAsync(int id, UpdateItemGroupDto dto);
        Task DeleteItemGroupAsync(int id);

        // ── Template Items ────────────────────────────────────────────────────
        Task<IEnumerable<TemplateItemDto>> GetItemsByGroupAsync(int groupId);
        Task<int> CreateTemplateItemAsync(CreateTemplateItemDto dto);
        Task UpdateTemplateItemAsync(int id, UpdateTemplateItemDto dto);
        Task DeleteTemplateItemAsync(int id);

        // ── Called Campaigns ──────────────────────────────────────────────────
        Task<IEnumerable<LsCalledCampaignDto>> GetCalledCampaignsByTemplateAsync(int templateId);
        Task<int> CreateCalledCampaignAsync(CreateCalledCampaignDto dto);
        Task UpdateCalledCampaignAsync(int id, UpdateCalledCampaignDto dto);
        Task<bool> CanDeleteCalledCampaignAsync(int id);
        Task DeleteCalledCampaignAsync(int id);

        // ── Lookup data ───────────────────────────────────────────────────────
        Task<IEnumerable<AvailableCampaignDto>> GetAvailableCampaignsAsync(int? excludeTemplateId = null);
Task<IEnumerable<AvailableCampaignDto>> GetAvailableCampaignsBySiteAsync(int customerId, int? templateId = null);        Task<IEnumerable<CustomerDto>> GetCustomersAsync();
        Task<IEnumerable<LsTemplatePeriodeDto>> GetPeriodesAsync();
    }
}