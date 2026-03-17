using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class TagEcoute
{
    public long Id { get; set; }

    public int? IdRecord { get; set; }

    public int? IdUser { get; set; }

    public int? TagListened { get; set; }
}
