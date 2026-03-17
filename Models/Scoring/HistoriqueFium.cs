using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class HistoriqueFium
{
    public int Id { get; set; }

    public DateTime DateStart { get; set; }

    public DateTime DateEnd { get; set; }

    public int Status { get; set; }

    public int? ImportCount { get; set; }

    public int? ImportMax { get; set; }
}
