using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class StLevelConversion
{
    public int Id { get; set; }

    public string LevelConversion { get; set; } = null!;
}
