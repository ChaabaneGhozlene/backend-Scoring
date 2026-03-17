using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class Menu
{
    public int Id { get; set; }

    public string MenuId { get; set; } = null!;

    public string MenuName { get; set; } = null!;

    public int ItemCode { get; set; }

    public int ItemParent { get; set; }

    public string ItemText { get; set; } = null!;

    public string ItemUrl { get; set; } = null!;

    public int ApplicationId { get; set; }
}
