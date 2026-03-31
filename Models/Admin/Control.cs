using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Admin;

public partial class Control
{
    public int Id { get; set; }

    public string PageId { get; set; } = null!;

    public string PageTitle { get; set; } = null!;

    public string PageUrl { get; set; } = null!;

    public string ControlId { get; set; } = null!;

    public string ControlText { get; set; } = null!;

    public string ControlType { get; set; } = null!;

    public int ApplicationId { get; set; }
}
