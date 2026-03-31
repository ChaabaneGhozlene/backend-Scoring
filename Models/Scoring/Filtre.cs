using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class Filtre
{
    public int Id { get; set; }

    public string NomFiltre { get; set; } = null!;

    public string Expression { get; set; } = null!;

    public int Type { get; set; }

    public string UserLogin { get; set; } = null!;

    public int Status { get; set; }

    public string DateCreation { get; set; } = null!;

    public int Groupe { get; set; }

    public string? SqlWhere { get; set; }

    public virtual FiltreGroupe GroupeNavigation { get; set; } = null!;
}
