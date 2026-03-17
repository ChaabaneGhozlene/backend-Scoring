using System;
using System.Collections.Generic;

namespace scoring_Backend.DTO
{
    // ── Filtre envoyé par le frontend ──
    public class ColumnFilterDto
{
    public string Id { get; set; } = "";
    public string Value { get; set; } = "";
}
    public class RecordFilterDto
{
    public DateTime? DateDebut       { get; set; }
    public DateTime? DateFin         { get; set; }
    public List<string>? AgentOids   { get; set; }
    public int? NbRecords            { get; set; }
    public int Page                  { get; set; } = 1;
    public int PageSize              { get; set; } = 50;
    public List<ColumnFilterDto>? ColumnFilters { get; set; }
    public int? FilterId             { get; set; }   // ← AJOUTER CETTE LIGNE
}

    // ── Données d'un enregistrement retournées à la grille ──
    public class RecordDataDto
    {
        public int      Id                  { get; set; }
        public string?  CampaignDescription { get; set; }   // RecordDatum.CampaignDescription
        public string?  AgentOid            { get; set; }   // RecordDatum.AgentOid
    public int? AgentId { get; set; }    // ← ajouter

        public string?  NomAgent            { get; set; }   // RecordDatum.NomAgent
        public string?  PrenomAgent         { get; set; }   // RecordDatum.PrenomAgent
        public DateTime? CallLocalTime      { get; set; }   // RecordDatum.CallLocalTime
        public string?  CallLocalTimeString { get; set; }   // RecordDatum.CallLocalTimeString
        public string?  StatusRequal        { get; set; }   // RecordDatum.StatusRequal
        public string?  StatusDescription   { get; set; }   // RecordDatum.StatusDescription
        public string?  CallTypeDescription { get; set; }   // RecordDatum.CallTypeDescription
        public string?  NumeroTel           { get; set; }   // RecordDatum.NumeroTel
        public int?     Duration            { get; set; }   // RecordDatum.Duration
        public bool     HasHistory          { get; set; }   // FullRecFilename != null
        public bool     HasEvaluation       { get; set; }   // LsSurvey existe pour ce record
        public bool HasHistoryScreen { get; set; }
        public int?     LsId               { get; set; }   // RecordDatum.LsId
        public int?     TypeRequalif        { get; set; }   // RecordDatum.TypeRequalif
    }

    // ── Résultat paginé retourné par /search ──
        public class ListenHistoryDto
    {
        public string Login     { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName  { get; set; } = "";
        public string Profil    { get; set; } = "";
        public string Action    { get; set; } = "";
        public string Position  { get; set; } = "";
        public string Date      { get; set; } = "";
        public string Statut    { get; set; } = ""; // = Duration dans ap_action
    }

    public class PagedRecordsDto
    {
        public List<RecordDataDto> Records { get; set; } = new();
        public int TotalCount              { get; set; }
        public int Page                    { get; set; }
        public int PageSize                { get; set; }
        public int TotalPages              => PageSize > 0
            ? (int)Math.Ceiling((double)TotalCount / PageSize)
            : 0;
    }
     // ── Traçabilité écoute (Play / Pause / Stop / …) ──────────────────────────
    public class TraceActionDto
    {
        /// <summary>ID de l'enregistrement écouté.</summary>
        public int     RecordId  { get; set; }
 
        /// <summary>
        /// Type d'événement :
        /// "Loaded" | "Play" | "Pause" | "Stop" |
        /// "Mute"   | "Unmute" | "SeekForward" | "SeekReversed" | "Ended"
        /// </summary>
        public string  EventType { get; set; } = "Play";
 
        /// <summary>Position courante (ex : "00:01:23").</summary>
        public string? Position  { get; set; }
 
        /// <summary>Durée totale du fichier.</summary>
        public string? Duration  { get; set; }
 
        /// <summary>Nom du fichier audio (stocké dans ap_action).</summary>
        public string? FileName  { get; set; }
    }

}