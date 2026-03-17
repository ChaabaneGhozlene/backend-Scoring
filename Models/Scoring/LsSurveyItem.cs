using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class LsSurveyItem
{
    public int Id { get; set; }

    public int SurveyId { get; set; }

    public int ItemId { get; set; }

    public float? Value { get; set; }

    public int CreateBy { get; set; }

    public DateTime CreateDate { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? UpdateTime { get; set; }

    public string? Memo { get; set; }

    public virtual LsSurvey Survey { get; set; } = null!;
}
