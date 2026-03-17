// ════════════════════════════════════════════════════════════════════
//  Ajoutez ces classes dans votre fichier DTO existant
//  (EvaluationAndFilterDtos.cs ou RecordDtos.cs selon votre structure)
// ════════════════════════════════════════════════════════════════════

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace scoring_Backend.DTO
{
    // ══════════════════════════════════════════════
    //  FILTER DTOs
    // ══════════════════════════════════════════════

    public class UserFilterDto
    {
        public int     Id         { get; set; }
        public string  Name       { get; set; } = string.Empty;
        public string? Expression { get; set; }
        public string? SqlWhere   { get; set; }
        public int     Type       { get; set; }
        public int     Status     { get; set; }
    }

    public class CreateFilterDto
    {
        [Required] public string  Name       { get; set; } = string.Empty;
        public          string? Expression { get; set; }
        public          string? SqlWhere   { get; set; }
        public          int     Type       { get; set; } = 1;
    }

    // ══════════════════════════════════════════════
    //  VIEW CONFIG DTOs
    // ══════════════════════════════════════════════

    public class ViewConfigDto
    {
        public int    Id         { get; set; }
        public string Name       { get; set; } = string.Empty;
        public string LayoutJson { get; set; } = string.Empty;
    }

    public class CreateViewConfigDto
    {
        [Required] public string Name       { get; set; } = string.Empty;
        [Required] public string LayoutJson { get; set; } = string.Empty;
    }

    public class UpdateLayoutDto
    {
        [Required] public string LayoutJson { get; set; } = string.Empty;
    }

    // ══════════════════════════════════════════════
    //  FILE DTOs
    // ══════════════════════════════════════════════

    public class FileUrlDto
    {
        public bool    Found    { get; set; }
        public string  Url      { get; set; } = string.Empty;
        public string? FileName { get; set; }
        public string? Error    { get; set; }
    }

    // ══════════════════════════════════════════════
    //  EVALUATION DTOs
    // ══════════════════════════════════════════════

    public class EvaluationInitDto
    {
        public int      SurveyId       { get; set; }
        public int      RecordId       { get; set; }
        public string?  Memo           { get; set; }
        public string?  MemoActionTaken{ get; set; }
        public int?     CategoryId     { get; set; }
        public int?     CallReasonId   { get; set; }
        public decimal? Score          { get; set; }
        public string?  AuditorName    { get; set; }
        public string?  RecordDate     { get; set; }
        public string?  EvalDate       { get; set; }
        public string?  CallIndex      { get; set; }

        public List<SurveyItemDto>  Items       { get; set; } = new();
        public List<CategoryDto>    Categories  { get; set; } = new();
        public List<CallReasonDto>  CallReasons { get; set; } = new();
    }

    public class EvaluationSaveDto
    {
        public int     SurveyId         { get; set; }
        public int     RecordId         { get; set; }
        public string? Memo             { get; set; }
        public string? MemoActionTaken  { get; set; }
        public int?    CategoryId       { get; set; }
        public int?    CallReasonId     { get; set; }
        public string? CcEmail         { get; set; }
        public List<SurveyItemSaveDto> Items { get; set; } = new();
    }

    public class EvaluationSaveResultDto
    {
        public bool    Success  { get; set; }
        public decimal Score    { get; set; }
        public string? Message  { get; set; }
    }

    public class SurveyItemSaveDto
    {
        public int     Id    { get; set; }
        public float   Value { get; set; }
        public string? Memo  { get; set; }
    }

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

    public class CategoryDto
    {
        public int    Id      { get; set; }
        public string Libelle { get; set; } = string.Empty;
    }

    public class CallReasonDto
    {
        public int    Id      { get; set; }
        public string Libelle { get; set; } = string.Empty;
    }
}