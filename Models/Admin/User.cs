using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string? Password { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public int Type { get; set; }

    public int? Role { get; set; }

    public string? SiteName { get; set; }

    public int? SiteId { get; set; }

    public string? Oid { get; set; }

    public int IsActive { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? SiteRule { get; set; }

    public int? CampagneRule { get; set; }

    public int? AgentRule { get; set; }
}
