using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class ViewListHermesSalesMan
{
    public int CustomerId { get; set; }

    public string Customer { get; set; } = null!;

    public string Oid { get; set; } = null!;

    public string? Id { get; set; }

    public string? Login { get; set; }

    public string Expr1 { get; set; } = null!;

    public int Expr2 { get; set; }

    public string Expr3 { get; set; } = null!;

    public string? Password { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Address { get; set; }

    public string? Cp { get; set; }

    public string? City { get; set; }

    public string? Email { get; set; }

    public string? Phone1 { get; set; }

    public string? Phone2 { get; set; }

    public string? IsPrimaryLastChange { get; set; }

    public bool? IsDisabled { get; set; }
}
