using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class TListAgentEmail
{
    public int Id { get; set; }

    public int IdAgent { get; set; }

    public string Oidagent { get; set; } = null!;

    public string? Email { get; set; }
}
