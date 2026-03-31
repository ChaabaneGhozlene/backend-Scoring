using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class VpAction
{
    public int Id { get; set; }

    public int CodeEvent { get; set; }

    public string? ScreenFileName { get; set; }

    public string? ScreenPosition { get; set; }

    /// <summary>
    /// audio file Id on RecordingTool
    /// </summary>
    public int? RecordedId { get; set; }

    public int UserId { get; set; }

    public DateTime CreateDate { get; set; }

    public string? Comment { get; set; }

    public string? Duration { get; set; }
}
