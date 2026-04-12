// ============================================================
// DTOs alignés avec votre SurveyItemDto existant
// À FUSIONNER dans votre fichier DTO existant
// (remplace EvaluationListDtos.cs généré précédemment)
// ============================================================

namespace scoring_Backend.DTO
{
    // ── Déjà dans votre projet — NE PAS recréer ─────────────
    /*
    public class SurveyItemDto
    {
        public int     Id             { get; set; }
        public int     SurveyId       { get; set; }
        public int     TemplateItemId { get; set; }
        public float   Value          { get; set; }
        public string? Memo           { get; set; }
        public string? SectionName    { get; set; }
        public int     SectionId      { get; set; }
        public int     SectionOrder   { get; set; }
        public string? Question       { get; set; }
        public string? Description    { get; set; }
        public int     ItemOrder      { get; set; }
        public float   MinValue       { get; set; }
        public float   MaxValue       { get; set; }
        public bool    AllowNA        { get; set; }
    }
    */

    // ── Fiche d'écoute (Ls) ─────────────────────────────────
    public class LsFicheDto
    {
        public int      Id           { get; set; }
        public string   Agent        { get; set; } = string.Empty;
        public string   AgentOid     { get; set; } = string.Empty;
        public int      AgentId      { get; set; }          // LastAgent (numéro)
        public int      Auditor      { get; set; }
        public string   AuditorName  { get; set; } = "";
        public DateTime StartPeriode { get; set; }
        public DateTime EndPeriode   { get; set; }
        public DateTime CreateDate   { get; set; }
        public DateTime UpdateDate   { get; set; }
        public double   Score        { get; set; }
        public string   CampaignName { get; set; } = string.Empty;
        public string   ModeleName   { get; set; } = string.Empty; // Ls_template.Name
        public int      SurveyCount  { get; set; }
                public int      SurveyId     { get; set; }
        public string   RecordDate   { get; set; } = string.Empty;
        public int      RecordDataId { get; set; }
    }

    // ── Survey individuelle ──────────────────────────────────
    public class LsSurveyDto
    {
        public int      Id              { get; set; }
        public int      LsId            { get; set; }
        public DateTime CreateDate      { get; set; }
        public DateTime UpdateDate      { get; set; }
        public double   Score           { get; set; }
        public int      IsSaved         { get; set; }
        public string   Memo            { get; set; } = string.Empty;
        public string   MemoActionTaken { get; set; } = string.Empty;
        public int      RecordDataId    { get; set; }
        public string   RecordDate      { get; set; } = string.Empty;
        public string   CategoryName    { get; set; } = string.Empty;
        public string   CallReasonName  { get; set; } = string.Empty;
        public string   AuditorName     { get; set; } = string.Empty;

        // Items — utilise votre SurveyItemDto existant
        public List<SurveyItemDto> Items { get; set; } = new();
    }

    // ── Mise à jour d'un item (aligned avec SurveyItemDto) ──
    public class SurveyItemUpdateDto
    {
        public int    Id    { get; set; }   // SurveyItemDto.Id
        public float  Value { get; set; }   // SurveyItemDto.Value
        public string Memo  { get; set; } // SurveyItemDto.Memo
    }

    // ── Mise à jour d'une survey ─────────────────────────────
    public class UpdateSurveyDto
    {
        public string?                   Memo            { get; set; }
        public string?                   MemoActionTaken { get; set; }
        public int?                      CategoryId      { get; set; }
        public int?                      CallReasonId    { get; set; }
        public string?                   CcEmail         { get; set; }
        public List<SurveyItemUpdateDto> Items           { get; set; } = new();
    }

    // ── Fiche Agent ──────────────────────────────────────────
    public class AgentReportDto
    {
        public string  CreateDate    { get; set; } = string.Empty;
        public string  AuditorName   { get; set; } = string.Empty;
        public string  AuditorLogin  { get; set; } = string.Empty;
        public string  AgentName     { get; set; } = string.Empty;
        public string  PeriodLabel   { get; set; } = string.Empty;
        public double  TotalScore    { get; set; }

        // Évaluations E1, E2, E3...
        public List<SurveyReportDto>  Surveys       { get; set; } = new();
        // Scores par section pour le graphique
        public List<SectionScoreDto>  SectionScores { get; set; } = new();
    }

    public class SurveyReportDto
    {
        public int    SurveyId    { get; set; }
        public string SurveyLabel { get; set; } = string.Empty; // "E1", "E2"…
        public string Memo        { get; set; } = string.Empty;
        public double Score       { get; set; }

        // Utilise votre SurveyItemDto existant
        public List<SurveyItemDto> Items { get; set; } = new();
    }

    public class SectionScoreDto
    {
        public string SectionName  { get; set; } = string.Empty;
        public double Score        { get; set; }
        public string SurveyLabel  { get; set; } = string.Empty; // "E1", "E2"…
    }

    // ── Filtre de recherche ──────────────────────────────────
    public class EvaluationListFilterDto
    {
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin   { get; set; }
        public int       Page     { get; set; } = 1;
        public int       PageSize { get; set; } = 15;
            public int?      FilterId  { get; set; }   
                public List<ColumnFilterDto>? ColumnFilters { get; set; } 


    }
    
}