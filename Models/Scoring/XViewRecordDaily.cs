using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class XViewRecordDaily
{
    public string RecDate { get; set; } = null!;

    public string RecTime { get; set; } = null!;

    public string RecIdent { get; set; } = null!;

    public string RecCampType { get; set; } = null!;

    public int RecNumCamp { get; set; }

    public string? CampaignDescription { get; set; }

    public int RecIdLink { get; set; }

    public int RecType { get; set; }

    public string? CallTypeDescription { get; set; }

    public string RecComment { get; set; } = null!;

    public string RecFilename { get; set; } = null!;

    public int? CustomerId { get; set; }

    public string? CustomerDescription { get; set; }

    public int? LastAgent { get; set; }

    public string? NomAgent { get; set; }

    public string? PrenomAgent { get; set; }

    public string? AgentOid { get; set; }

    public string? NumeroTel { get; set; }

    public string? CallLocalTimeString { get; set; }

    public int? Duration { get; set; }

    public int? CallDuration { get; set; }

    public int? ConvDuration { get; set; }

    public int? WaitDuration { get; set; }

    public int? TotalWaitDuration { get; set; }

    public int? CallStatusGroup { get; set; }

    public int? CallStatusNum { get; set; }

    public int? CallStatusDetail { get; set; }

    public string? StatusText { get; set; }

    public int? StatusGroupeRequal { get; set; }

    public int? StatusNumRequal { get; set; }

    public int? StatusDetailRequal { get; set; }

    public int? StatusRequal { get; set; }

    public int TypeRequalif { get; set; }

    public string? DateImport { get; set; }

    public int TypeImport { get; set; }

    public string? Ani { get; set; }

    public string? Dnis { get; set; }

    public string? Memo { get; set; }

    public int? RecordSource { get; set; }

    public int? RecordArchive { get; set; }

    public int RecordStatusId { get; set; }

    public int? DateTransfert { get; set; }

    public int? DateArchive { get; set; }

    public DateTime? RecordDate { get; set; }

    public string? RecordTime { get; set; }

    public DateTime? CallLocalTime { get; set; }
}
