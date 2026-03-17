using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class XViewDailyImport
{
    public string RecDate { get; set; } = null!;

    public string RecTime { get; set; } = null!;

    public string RecIdent { get; set; } = null!;

    public string RecCampType { get; set; } = null!;

    public int RecNumCamp { get; set; }

    public string? Did { get; set; }

    public int RecIdLink { get; set; }

    public int RecType { get; set; }

    public string? Description { get; set; }

    public string RecComment { get; set; } = null!;

    public string RecFilename { get; set; } = null!;

    public int? CustomerId { get; set; }

    public string? Site { get; set; }

    public int? LastAgent { get; set; }

    public string? Nom { get; set; }

    public string? Prénom { get; set; }

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

    public string? Status { get; set; }

    public string? DateImport { get; set; }

    public int TypeImport { get; set; }

    public string? Ani { get; set; }

    public string? Dnis { get; set; }

    public string? Memo { get; set; }

    public string RecordSource { get; set; } = null!;

    public int? RecordArchive { get; set; }
}
