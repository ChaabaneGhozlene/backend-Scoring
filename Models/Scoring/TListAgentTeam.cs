using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class TListAgentTeam
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int? IdSite { get; set; }

    public virtual ICollection<TAgentTeam> TAgentTeams { get; set; } = new List<TAgentTeam>();
}
