using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class StFileTemplateKey
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string? Value { get; set; }

    public string? Description { get; set; }

    public int Evaluate { get; set; }
}
