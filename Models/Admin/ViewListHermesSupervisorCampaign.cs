using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class ViewListHermesSupervisorCampaign
{
    public int SupervisorId { get; set; }

    public string SupervisorOid { get; set; } = null!;

    public string CampaignId { get; set; } = null!;

    public string CampaignOid { get; set; } = null!;

    public int CustomerId { get; set; }

    public string CustomerOid { get; set; } = null!;
}
