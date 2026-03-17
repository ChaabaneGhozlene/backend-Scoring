using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class ViewListHermesAgent
{
    public int CustomerId { get; set; }

    public string Customer { get; set; } = null!;

    public int IsSupervisor { get; set; }

    public string Oid { get; set; } = null!;

    public int Ident { get; set; }

    public string? Prenom { get; set; }

    public string? Nom { get; set; }

    public string? ReceptionGrp { get; set; }

    public int? Queues { get; set; }

    public int? EntreCom { get; set; }

    public string? Options { get; set; }

    public string? LoginName { get; set; }

    public string? Profile { get; set; }

    public string Rights { get; set; } = null!;

    public string? Password { get; set; }

    public int? GroupMember { get; set; }

    public string? ScriptFramesetUrl { get; set; }

    public string? ScriptFramesetName { get; set; }

    public int MaxChatSessions { get; set; }

    public int MaxEmailSessions { get; set; }

    public bool AlwaysOnTop { get; set; }

    public bool FullScreen { get; set; }

    public int WorkspaceWidth { get; set; }

    public int WorkspaceHeight { get; set; }

    public int? ContextOptions { get; set; }

    public bool ReadyAtLogin { get; set; }

    public string WorkspaceOid { get; set; } = null!;

    public string? Ctiskills { get; set; }

    public string? OutCampaigns { get; set; }

    public string CustomerOid { get; set; } = null!;

    public string? Createat { get; set; }

    public string? Deleteat { get; set; }
}
