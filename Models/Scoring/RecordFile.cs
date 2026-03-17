using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class RecordFile
{
    public string RecSourceFilename { get; set; } = null!;

    public DateTime RecSourceCreationDate { get; set; }

    public long? RecSourceSize { get; set; }

    public string? RecDestinationFilename { get; set; }

    public DateTime? RecDestinationCreationDate { get; set; }

    public int? RecDestinationSize { get; set; }

    public int? RecStatusId { get; set; }

    public int? RecExecDuration { get; set; }
}
