using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class ListCustomer
{
    public string Oid { get; set; } = null!;

    public int CustomerId { get; set; }

    public string Description { get; set; } = null!;

    public int MaxQueues { get; set; }

    public int MaxActiveOutboundCampaigns { get; set; }

    public int MaxAdmin { get; set; }

    public int MaxAgents { get; set; }

    public int MaxReporting { get; set; }

    public int MaxSuperviseurs { get; set; }

    public string? RangeQueues { get; set; }

    public string? RangeAgents { get; set; }

    public string? RangeDids { get; set; }

    public string? RangeStations { get; set; }

    public int Type { get; set; }

    public int Enabled { get; set; }

    public string? ScripterOid { get; set; }

    public string? ProxyOid { get; set; }

    public string? ReportingOid { get; set; }

    public string? SupervisionOid { get; set; }

    public string? ScriptFramesetUrl { get; set; }

    public string? ScriptFramesetName { get; set; }

    public string? VmcOid { get; set; }

    public string? CrmOid { get; set; }

    public string? FlashMediaOid { get; set; }

    public int? TrunkType { get; set; }

    public string? RecordsPathVideo { get; set; }

    public string? RfbServerOid { get; set; }

    public string? OnMediaServiceOid { get; set; }

    public string? DatabaseManagerOid { get; set; }

    public string? AcdOid { get; set; }

    public string? ProxySipOid { get; set; }

    public string? DefaultPhoneNumber { get; set; }

    public int? ForcePhoneDisplay { get; set; }

    public string? ManagerName { get; set; }

    public string? ManagerTel { get; set; }

    public string? ManagerEmail { get; set; }

    public string? OutExclude { get; set; }

    public string? RecordsFileTemplate { get; set; }

    public int MaxAutoLines { get; set; }

    public int? CustomerOid { get; set; }

    public string? Createat { get; set; }

    public string? Deleteat { get; set; }
}
