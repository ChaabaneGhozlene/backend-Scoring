using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class LsTemplateItemGroup
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public int Coef { get; set; }

    public int Order { get; set; }

    public int TemplateId { get; set; }

    public virtual ICollection<LsTemplateItem> LsTemplateItems { get; set; } = new List<LsTemplateItem>();

    public virtual LsTemplate Template { get; set; } = null!;
}
