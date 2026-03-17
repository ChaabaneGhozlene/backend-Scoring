using System;
using System.Collections.Generic;

namespace scoring_Backend.Models.Scoring;

public partial class RecordDatum
{
    public int Id { get; set; }

    public string? RecDate { get; set; }

    public string? RecTime { get; set; }

    public string? RecIdent { get; set; }

    public string? RecCampType { get; set; }

    public int? RecNumCamp { get; set; }

    public string? CampaignDescription { get; set; }

    public int? RecIdLink { get; set; }

    public int? RecType { get; set; }

    public string? CallTypeDescription { get; set; }

    public string? RecComment { get; set; }

    public string? RecFilename { get; set; }

    public int? CustomerId { get; set; }

    public string? CustomerDescription { get; set; }

    public int? LastAgent { get; set; }

    public string? NomAgent { get; set; }

    public string? PrenomAgent { get; set; }

    public string? AgentOid { get; set; }

    public string? NumeroTel { get; set; }

    public string? CallLocalTimeString { get; set; }

    public int? Duration { get; set; }

    public int? CallDuration { get; set; }

    public int? ConvDuration { get; set; }

    public int? WaitDuration { get; set; }

    public int? TotalWaitDuration { get; set; }

    public int? CallStatusGroup { get; set; }

    public int? CallStatusNum { get; set; }

    public int? CallStatusDetail { get; set; }

    public string? StatusText { get; set; }

    public int? StatusGroupeRequal { get; set; }

    public int? StatusNumRequal { get; set; }

    public int? StatusDetailRequal { get; set; }

    public string? StatusRequal { get; set; }

    public int? TypeRequalif { get; set; }

    public string? DateImport { get; set; }

    public int? TypeImport { get; set; }

    public string? Ani { get; set; }

    public string? Dnis { get; set; }

    public string? Memo { get; set; }

    public string? RecordSource { get; set; }

    public string? RecordArchive { get; set; }

    public int? RecordStatusId { get; set; }

    public string? DateTransfert { get; set; }

    public string? DateArchive { get; set; }

    public DateTime? RecordDate { get; set; }

    public string? RecordTime { get; set; }

    public DateTime? CallLocalTime { get; set; }

    public string? RecordSourceOld { get; set; }

    public string? RecCallId { get; set; }

    public string? RecCampaignDescription { get; set; }

    public string? FullRecFilename { get; set; }

    public string? FullRecFilenameBackup { get; set; }

    public int? RecExec { get; set; }

    public string? StatusDescription { get; set; }

    public string? StatusGroupDescription { get; set; }

    public string? CallStatusGroupDescription { get; set; }

    public string? CallStatusNumDescription { get; set; }

    public string? CallStatusDetailDescription { get; set; }

    public string? CampaignId { get; set; }

    public int? LsId { get; set; }

    public string? RecFilenameTmp { get; set; }

    public string? RecDateLocal { get; set; }
}
