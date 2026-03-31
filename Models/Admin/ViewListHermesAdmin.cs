using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class ViewListHermesAdmin
{
    public int CustomerId { get; set; }

    public string Customer { get; set; } = null!;

    public int IsSuperAdmin { get; set; }

    public string Oid { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string? Password { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string CustomerOid { get; set; } = null!;

    public string? Createat { get; set; }

    public string? Deleteat { get; set; }
}
