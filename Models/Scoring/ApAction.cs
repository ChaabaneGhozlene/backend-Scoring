using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class ApAction
{
    public int Id { get; set; }

    public int CodeEvent { get; set; }

    /// <summary>
    /// path of the audio file
    /// </summary>
    public string? RecordFileName { get; set; }

    /// <summary>
    /// current audio position in seconde
    /// </summary>
    public string? RecordPosition { get; set; }

    /// <summary>
    /// audio file Id on RecordingTool
    /// </summary>
    public int? RecordedId { get; set; }

    public int UserId { get; set; }

    public DateTime CreateDate { get; set; }

    public string? Comment { get; set; }

    public string? Duration { get; set; }
}
