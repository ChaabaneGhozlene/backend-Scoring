using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class Connection
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Type { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? Base { get; set; }

    public string? Server { get; set; }

    public string? DatabaseConnection { get; set; }

    public int? ApplicationId { get; set; }
}
