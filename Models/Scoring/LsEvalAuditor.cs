using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class LsEvalAuditor
{
    public int? Id { get; set; }

    public int? IdAuditor { get; set; }

    public int? NumberEval { get; set; }

    public string? DateEval { get; set; }
}
