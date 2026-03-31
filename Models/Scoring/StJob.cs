using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class StJob
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Memo { get; set; }

    public int FiltreId { get; set; }

    public int FrequencyId { get; set; }

    public DateTime SchedulingTime { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? SourcePath { get; set; }

    public string DestinationPath { get; set; } = null!;

    public string RecordFileTemplate { get; set; } = null!;

    public int Active { get; set; }

    public int? Notification { get; set; }

    public int? NotificationEvent { get; set; }

    public string? Email { get; set; }

    public DateTime? LastRunDate { get; set; }

    public int? LastRunStatus { get; set; }

    public DateTime? NextRunDate { get; set; }

    public int CreateBy { get; set; }

    public DateTime CreateDate { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? MaxRecord { get; set; }

    public int ActiveRandom { get; set; }

    public int ActiveConversion { get; set; }

    public int ActiveFtp { get; set; }

    public int? LevelConversionId { get; set; }

    public string? ServerFtp { get; set; }

    public string? DestinationFtp { get; set; }

    public string? LoginFtp { get; set; }

    public string? PasswordFtp { get; set; }

    public int? PortFtp { get; set; }

    public int? ActiveJournee { get; set; }

    public int? Journee { get; set; }
}
