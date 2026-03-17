using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class LsCategory
{
    public int Id { get; set; }

    public string? DesCategories { get; set; }

    public int? SiteId { get; set; }

    public virtual ICollection<LsSurvey> LsSurveys { get; set; } = new List<LsSurvey>();
}
