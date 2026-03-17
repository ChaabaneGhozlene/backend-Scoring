using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class UsersSite
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int SiteId { get; set; }

    public string SiteOid { get; set; } = null!;
}
