using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class Odcallsback
{
    public string Id { get; set; } = null!;

    public string? CtiId { get; set; }

    public int? CustomerId { get; set; }

    public decimal? Indice { get; set; }

    public int? CallType { get; set; }

    public DateTime? CallUniversalTime { get; set; }

    public DateTime? CallLocalTime { get; set; }

    public string? CallUniversalTimeString { get; set; }

    public string? CallLocalTimeString { get; set; }

    public int? Duration { get; set; }

    public int? AcceptDuration { get; set; }

    public int? IvrDuration { get; set; }

    public int? WaitDuration { get; set; }

    public int? ConvDuration { get; set; }

    public int? RerouteDuration { get; set; }

    public int? OverflowDuration { get; set; }

    public string? Ani { get; set; }

    public string? Dnis { get; set; }

    public string? FirstCampaign { get; set; }

    public string? LastCampaign { get; set; }

    public string? Uui { get; set; }

    public string? Memo { get; set; }

    public string? AssociatedData { get; set; }

    public string? OutTel { get; set; }

    public string? OutDialed { get; set; }

    public int? Closed { get; set; }

    public int? NoAgent { get; set; }

    public int? Overflow { get; set; }

    public int? Abandon { get; set; }

    public string? FirstIvr { get; set; }

    public string? LastIvr { get; set; }

    public int? FirstQueue { get; set; }

    public int? LastQueue { get; set; }

    public int? InitPriority { get; set; }

    public int? FirstAgent { get; set; }

    public int? LastAgent { get; set; }

    public string? LastTransfer { get; set; }

    public int? CallStatusGroup { get; set; }

    public int? CallStatusNum { get; set; }

    public int? CallStatusDetail { get; set; }

    public string? Comments { get; set; }

    public string? ContactId { get; set; }

    public int? WrapupDuration { get; set; }

    public int? EndByAgent { get; set; }

    public int? AgentListen { get; set; }

    public int? CallDuration { get; set; }

    public int? TotalWaitDuration { get; set; }

    public int? EndReason { get; set; }

    public string? RefId { get; set; }

    public string? ProactiveReason { get; set; }
}
