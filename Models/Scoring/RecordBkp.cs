using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class RecordBkp
{
    public int RecordStatusId { get; set; }

    public string RecDate { get; set; } = null!;

    public string? RecTime { get; set; }

    public string RecIdent { get; set; } = null!;

    public string RecCampType { get; set; } = null!;

    public string RecNumCamp { get; set; } = null!;

    public int RecIdLink { get; set; }

    public string RecFilename { get; set; } = null!;

    public string RecComment { get; set; } = null!;

    public string RecCallId { get; set; } = null!;

    public string? RecFilenametmp { get; set; }

    public int RecCustomerId { get; set; }

    public string RecCampId { get; set; } = null!;

    public string RecC { get; set; } = null!;

    public int RecAgentId { get; set; }

    public string RecDate1 { get; set; } = null!;
}
