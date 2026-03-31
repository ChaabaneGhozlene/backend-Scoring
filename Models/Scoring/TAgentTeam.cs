using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class TAgentTeam
{
    public int Id { get; set; }

    public int? AgentId { get; set; }

    public int? TeamId { get; set; }

    public string AgentOid { get; set; } = null!;

    public virtual TListAgentTeam? Team { get; set; }
}
