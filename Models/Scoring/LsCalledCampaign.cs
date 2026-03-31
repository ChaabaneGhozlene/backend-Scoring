using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class LsCalledCampaign
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int? Site { get; set; }

    public string? CampagneDid { get; set; }

    public string? CampagneDescription { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? Status { get; set; }

    public int? IdLsTemplate { get; set; }

    public virtual LsTemplate? IdLsTemplateNavigation { get; set; }
}
