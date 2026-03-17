using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class Application
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Com { get; set; }
}
