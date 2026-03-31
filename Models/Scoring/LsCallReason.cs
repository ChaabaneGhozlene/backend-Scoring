using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class LsCallReason
{
    public int Id { get; set; }

    public string? DesCallReason { get; set; }

    public int? SiteId { get; set; }

    public virtual ICollection<LsSurvey> LsSurveys { get; set; } = new List<LsSurvey>();
}
