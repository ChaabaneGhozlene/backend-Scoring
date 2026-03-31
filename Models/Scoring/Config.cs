using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class Config
{
    public int Id { get; set; }

    public string NomConfig { get; set; } = null!;

    public string Layout { get; set; } = null!;

    public string UserLogin { get; set; } = null!;

    public int Groupe { get; set; }
}
