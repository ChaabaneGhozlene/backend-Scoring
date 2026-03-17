using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class Recordcheck
{
    public int Id { get; set; }

    public int? Recordstatusid { get; set; }

    public string? Recordsource { get; set; }

    public string? Vlastagent { get; set; }

    public int? Lastagent { get; set; }

    public int C1 { get; set; }

    public string? V { get; set; }

    public string? RecDate { get; set; }

    public int C2 { get; set; }

    public string? VrecIdlink { get; set; }

    public int? RecIdlink { get; set; }

    public int C3 { get; set; }

    public string? VNumeroTel { get; set; }

    public string? NumeroTel { get; set; }

    public int C4 { get; set; }

    public string? Vagent { get; set; }

    public string? Agent { get; set; }

    public int C5 { get; set; }

    public string? Vcampaignid { get; set; }

    public string? Campaignid { get; set; }

    public int C6 { get; set; }
}
