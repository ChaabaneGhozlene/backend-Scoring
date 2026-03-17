using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class ProfilMenu
{
    public int Id { get; set; }

    public int ProfilId { get; set; }

    public int MenuId { get; set; }
}
