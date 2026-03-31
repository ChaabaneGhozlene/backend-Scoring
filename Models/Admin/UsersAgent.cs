using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class UsersAgent
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int AgentId { get; set; }

    public string AgentOid { get; set; } = null!;

    public int SiteId { get; set; }

    public string SiteOid { get; set; } = null!;
}
