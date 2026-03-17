using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class ViewListHermesSupervisorAgent
{
    public int SupervisorId { get; set; }

    public string SupervisorOid { get; set; } = null!;

    public int AgentId { get; set; }

    public string AgentOid { get; set; } = null!;

    public int CustomerId { get; set; }

    public string CustomerOid { get; set; } = null!;
}
