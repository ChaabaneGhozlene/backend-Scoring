using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class TListCallStatus
{
    public int CustomerId { get; set; }

    public string Customer { get; set; } = null!;

    public string Oid { get; set; } = null!;

    public int StatusGroup { get; set; }

    public int StatusCode { get; set; }

    public int StatusDetail { get; set; }

    public string StatusText { get; set; } = null!;

    public string? Comment { get; set; }

    public bool Positive { get; set; }

    public bool Argued { get; set; }

    public bool Defaut { get; set; }

    public double? Cost { get; set; }

    public string? Currency { get; set; }

    public bool? ValidQuota { get; set; }

    public string CustomerOid { get; set; } = null!;

    public string? Createat { get; set; }

    public string? Deleteat { get; set; }
}
