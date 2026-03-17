using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class MgrhConfDock
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Layout { get; set; }

    public int? UserCreated { get; set; }

    public DateTime? DateTimeCreated { get; set; }

    public int? UserUpdated { get; set; }

    public DateTime? DateTimeUpdated { get; set; }
}
