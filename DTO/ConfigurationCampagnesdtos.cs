namespace scoring_Backend.DTO
{
    // ── Template ─────────────────────────────────────────────────────────────

    public record LsTemplateDto(
        int Id,
        string Description,
        int Min,
        int Max,
        int Version,
        int Status,
        int? RelatedTemplate,
        DateTime StartDate,
        DateTime EndDate,
        int? LsTemplatePeriodeId,
        string? PeriodeDescription
    );

    public record CreateLsTemplateDto(
        string Description,
        int Min,
        int Max,
        int LsTemplatePeriodeId,
        string StartDate,
        string EndDate,
        List<ItemGroupDto> ItemGroups,
        List<string> SelectedCampaignParams
    );

    // ✅ FIX : ItemGroups ajouté
    public record UpdateLsTemplateDto(
        string Description,
        int Min,
        int Max,
        int LsTemplatePeriodeId,
        string StartDate,
        string EndDate,
        List<ItemGroupDto> ItemGroups,        // ← AJOUTÉ
        List<string> SelectedCampaignParams
    );

    public record ChangeModelDto(
        string Description,
        int Min,
        int Max,
        int LsTemplatePeriodeId,
        string StartDate,
        string EndDate,
        List<ItemGroupDto> ItemGroups,
        List<string> SelectedCampaignParams
    );

    // ── Item Groups ───────────────────────────────────────────────────────────

    public record ItemGroupDto(
        int Id,
        string Description,
        int Coef,
        int Order,
        List<TemplateItemDto> Items
    );

    public record CreateItemGroupDto(
        string Description,
        int Coef,
        int Order
    );

    public record UpdateItemGroupDto(
        string Description,
        int Coef,
        int Order
    );

    // ── Template Items ────────────────────────────────────────────────────────
// ── Template Items — AVEC Question ───────────────────────────────────────────

public record TemplateItemDto(
    int Id,
    string Description,
    string? Question,        // ← AJOUTÉ
    int Min,
    int Max,
    int Coef,
    int Order,
    int IsNa,
    int IsKillerQuestion,
    int IsKillerSection
);

public record CreateTemplateItemDto(
    string Description,
    string? Question,        // ← AJOUTÉ
    int Min,
    int Max,
    int Coef,
    int Order,
    int IsNa,
    int IsKillerQuestion,
    int IsKillerSection,
    int LsTemplateItemGroupId
);

public record UpdateTemplateItemDto(
    string Description,
    string? Question,        // ← AJOUTÉ
    int Min,
    int Max,
    int Coef,
    int Order,
    int IsNa,
    int IsKillerQuestion,
    int IsKillerSection,
    int LsTemplateItemGroupId
);

    // ── Called Campaigns ─────────────────────────────────────────────────────

    public record LsCalledCampaignDto(
        int Id,
        string Description,
        int Site,
        string CampagneDid,
        string CampagneDescription,
        int Status,
        DateTime StartDate,
        DateTime EndDate,
        int LsTemplateId
    );

    public record CreateCalledCampaignDto(
        string Description,
        int Site,
        string CampagneParam,
        int Status,
        int LsTemplateId
    );

    public record UpdateCalledCampaignDto(
        string Description,
        int Site,
        string CampagneParam,
        int Status,
        int LsTemplateId
    );

    // ── Lookups ───────────────────────────────────────────────────────────────

    public record AvailableCampaignDto(
        string Display,
        string Param
    );

    public record CustomerDto(
        int CustomerId,
        string Description
    );

    public record LsTemplatePeriodeDto(
        int Id,
        string Description
    );
}