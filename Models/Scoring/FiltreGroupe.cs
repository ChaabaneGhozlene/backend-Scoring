using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class FiltreGroupe
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Filtre> Filtres { get; set; } = new List<Filtre>();
}
