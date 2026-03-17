using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class LsTemplate
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public int Min { get; set; }

    public int Max { get; set; }

    public int PeriodeId { get; set; }

    public int TypeId { get; set; }

    public string? ScriptUrl { get; set; }

    public int? Type { get; set; }

    public string? UrlScript { get; set; }

    public int? ActiveMinMax { get; set; }

    public int? Site { get; set; }

    public string? CampagneDid { get; set; }

    public string? CampaignDescription { get; set; }

    public int? Status { get; set; }

    public int? RelatedTemplate { get; set; }

    public int? Version { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual ICollection<LsCalledCampaign> LsCalledCampaigns { get; set; } = new List<LsCalledCampaign>();

    public virtual ICollection<LsTemplateItemGroup> LsTemplateItemGroups { get; set; } = new List<LsTemplateItemGroup>();
}
