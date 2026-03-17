using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class StFtp
{
    public int Id { get; set; }

    public int Active { get; set; }

    public string Sever { get; set; } = null!;

    public string Destination { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Port { get; set; }
}
