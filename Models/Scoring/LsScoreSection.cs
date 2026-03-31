using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class LsScoreSection
{
    public int Id { get; set; }

    public int IdLsSurvey { get; set; }

    public int IdLsTemplateItemGroup { get; set; }

    public float ScoreGroup { get; set; }
}
