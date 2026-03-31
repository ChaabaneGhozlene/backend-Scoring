using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class EvalUser
{
    public int Id { get; set; }

    public int? Idauditor { get; set; }

    public int? NumberEval { get; set; }

    public string? DateEval { get; set; }
}
