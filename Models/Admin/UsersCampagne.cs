using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class UsersCampagne
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string CampagneId { get; set; } = null!;

    public string CampagneOid { get; set; } = null!;

    public int SiteId { get; set; }

    public string SiteOid { get; set; } = null!;
}
