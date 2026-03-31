using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class LsTemplateItem
{
    public int Id { get; set; }

    public string? Question { get; set; }

    public string Description { get; set; } = null!;

    public int Min { get; set; }

    public int Max { get; set; }

    public int Coef { get; set; }

    public int Order { get; set; }

    public int GroupId { get; set; }

    public int? IsNa { get; set; }

    public int? IsKillerQuestion { get; set; }

    public int? IsKillerSection { get; set; }

    public virtual LsTemplateItemGroup Group { get; set; } = null!;
}
