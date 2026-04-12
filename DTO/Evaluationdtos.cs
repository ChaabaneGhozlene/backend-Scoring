// ============================================================
// DTO/EvaluationDTOs.cs
// Tous les DTOs nécessaires pour le module Évaluation
// ============================================================

namespace scoring_Backend.DTO
{
      // ── Agent ──
    public class AgentDto
    {
        public string Oid   { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty; // "Prénom Nom"
    }
 // ── Campagne qualité ──
    public class CampaignQualityDto
    {
        public int    Id          { get; set; }
        public string Description { get; set; } = string.Empty;
    }
    // ── Résultat d'un enregistrement (ligne grille) ──
    public class RecordDto
    {
        public int      Id                          { get; set; }
        public DateTime RecordDate                  { get; set; }
        public string?  CustomerDescription         { get; set; }
        public string?  CampaignDescription         { get; set; }
        public int      LastAgent                   { get; set; }
        public string?  NomAgent                    { get; set; }
        public string?  PrenomAgent                 { get; set; }
        public string?  CallTypeDescription         { get; set; }
        public string?  NumeroTel                   { get; set; }
        public string?  RecordTime                  { get; set; }
        public string?  AgentOid                    { get; set; }
        public string?  CallStatusGroupDescription  { get; set; }
        public string?  CallStatusNumDescription    { get; set; }
        public string?  CallStatusDetailDescription { get; set; }
        public int      RecIdLink                   { get; set; }
        public string?  RecComment                  { get; set; }
        public string?  RecFilename                 { get; set; }
        public int      CustomerId                  { get; set; }
        public DateTime? CallLocalTime              { get; set; }
        public int      CallDuration                { get; set; }
        public int      ConvDuration                { get; set; }
        public int      WaitDuration                { get; set; }
        public int      TotalWaitDuration           { get; set; }
        public int      CallStatusGroup             { get; set; }
        public int      CallStatusNum               { get; set; }
        public int      CallStatusDetail            { get; set; }
        public int      StatusGroupeRequal          { get; set; }
        public int      StatusNumRequal             { get; set; }
        public int      StatusDetailRequal          { get; set; }
        public string?  StatusRequal                { get; set; }
        public string?  RecordSource                { get; set; }
        public string?  RecordArchive               { get; set; }
        public string?  ScreenSource                { get; set; }
        public bool     AlreadyEvaluated            { get; set; }
    }

    
    // ── Ouverture d'une évaluation ──
    public class OpenEvaluationRequestDto
    {
        public int RecordId { get; set; }
    }
// ── Fichier audio d'un enregistrement ──
public class RecordFileDto
{
    public int     Id       { get; set; }
    public string? FilePath { get; set; }
}
    // ── Réponse lors de l'ouverture ──
    public class OpenEvaluationResponseDto
    {
        public int      SurveyId     { get; set; }
        public string?  RecordDate   { get; set; }
        public string?  EvalDate     { get; set; }
        public string?  Auditor      { get; set; }
        public string?  CallIndex    { get; set; }
        public bool     AlreadyDone  { get; set; }
        public string?  ErrorMessage { get; set; }
        public List<EvalGridRowDto> GridRows { get; set; } = new();
        public List<CategoryDto>   Categories { get; set; } = new();
        public List<CallReasonDto> CallReasons  { get; set; } = new();
    }

    // ── Ligne de la grille d'évaluation ──
    public class EvalGridRowDto
    {
        public int     Id           { get; set; }
        public int     SurveyId     { get; set; }
        public int     TemplateItemId { get; set; }
        public int     GroupId      { get; set; }
        public string? GroupName    { get; set; }
        public int     GroupOrder   { get; set; }
        public string? Question     { get; set; }
        public string? Definition   { get; set; }
        public float   Value        { get; set; }
        public string? Memo         { get; set; }
        public int     ScaleMax     { get; set; }
        public int     ScaleMin     { get; set; }
        public bool    IsNA         { get; set; }
        public int     ItemOrder    { get; set; }
    }

    // ── Sauvegarde d'une évaluation ──
    public class SaveEvaluationDto
    {
        public int     SurveyId     { get; set; }
        public int?    CategoryId   { get; set; }
        public int?    CallReasonId { get; set; }
        public string? Memo         { get; set; }
        public string? MemoAction   { get; set; }
        public string? CcEmail      { get; set; }
        public List<SurveyItemValueDto> Items { get; set; } = new();
    }

    public class SurveyItemValueDto
    {
        public int    ItemId { get; set; }
        public float  Value  { get; set; }
        public string? Memo  { get; set; }
    }

    // ── Résultat de sauvegarde ──
    public class SaveEvaluationResultDto
    {
        public double Score       { get; set; }
        public string? Message    { get; set; }
        public bool    Success    { get; set; }
    }

    // ── Catégories & raisons d'appel ──


    // ── Statut Hermess pour requalification ──
    public class CallStatusItemDto
    {
        public string Param       { get; set; } = string.Empty; // "group,detail,code"
        public string Description { get; set; } = string.Empty;
    }
    public class MultiSurveyResponseDto
{
    public List<SurveyColumnDto>   Surveys { get; set; } = new();
    public List<MultiSurveyRowDto> Rows    { get; set; } = new();
}

public class SurveyColumnDto
{
    public string Label    { get; set; } = "";
    public int    SurveyId { get; set; }
    public float  Score    { get; set; }
}

public class MultiSurveyRowDto
{
    public int         TemplateItemId { get; set; }
    public int         GroupId        { get; set; }
    public string      GroupName      { get; set; } = "";
    public int         GroupOrder     { get; set; }
    public string      Question       { get; set; } = "";
    public string      Definition     { get; set; } = "";
    public float       ScaleMax       { get; set; }
    public float       ScaleMin       { get; set; }
    public bool        IsNA           { get; set; }
    public int         ItemOrder      { get; set; }
    public List<float> Values         { get; set; } = new(); // E1, E2, E3...
}
public class RecordScreenDto
{
    public int     Id           { get; set; }
    public string? ScreenSource { get; set; }
}
}