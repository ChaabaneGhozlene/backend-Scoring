using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class Profil
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public string Com { get; set; } = null!;

    public int ApplicationId { get; set; }
}
