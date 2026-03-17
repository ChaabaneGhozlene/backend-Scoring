using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class StNotificationEvent
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;
}
