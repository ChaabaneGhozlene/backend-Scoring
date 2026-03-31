using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class TtOdcall
{
    public string Id { get; set; } = null!;

    public decimal? Indice { get; set; }

    public int? CustomerId { get; set; }

    public int? CallType { get; set; }

    public string? CallLocalTimeString { get; set; }

    public int? Duration { get; set; }

    public int? CallDuration { get; set; }

    public int? ConvDuration { get; set; }

    public int? WaitDuration { get; set; }

    public int? TotalWaitDuration { get; set; }

    public string? Ani { get; set; }

    public string? Dnis { get; set; }

    public string? Memo { get; set; }

    public DateTime? CallLocalTime { get; set; }

    public int? CallStatusGroup { get; set; }

    public int? CallStatusNum { get; set; }

    public int? CallStatusDetail { get; set; }

    public string? LastCampaign { get; set; }

    public string? FirstCampaign { get; set; }

    public int? LastAgent { get; set; }

    public string? OutTel { get; set; }

    public int? FirstAgent { get; set; }

    public string? CallUniversalTimeString { get; set; }
}
