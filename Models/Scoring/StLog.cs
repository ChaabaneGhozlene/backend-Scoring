using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class StLog
{
    public int Id { get; set; }

    public int JobId { get; set; }

    public DateTime RunDate { get; set; }

    public DateTime? StartRunDate { get; set; }

    public DateTime? EndRunDate { get; set; }

    public int StatusId { get; set; }

    public string? Memo { get; set; }

    public int CreateBy { get; set; }

    public DateTime CreateDate { get; set; }
}
