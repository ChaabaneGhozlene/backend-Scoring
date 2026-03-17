using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class ApEvent
{
    public int Code { get; set; }

    public string Description { get; set; } = null!;
}
