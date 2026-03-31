using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;    
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Threading.Tasks;   
namespace Scoring_Backend.DTO
{
    // ══════════════════════════════════════════════
    //  EVALUATION DTOs
    // ══════════════════════════════════════════════

    /// <summary>Retourné par EvaluationController /init/{recordId}</summary>
    public class EvaluationInitDto
    {
        public int SurveyId         { get; set; }
        public int RecordId         { get; set; }
        public string? Memo         { get; set; }
        public string? MemoActionTaken { get; set; }
        public int? CategoryId      { get; set; }
        public int? CallReasonId    { get; set; }
        public decimal? Score       { get; set; }

        public List<SurveyItemDto>  Items       { get; set; } = new();
        public List<CategoryDto>    Categories  { get; set; } = new();
        public List<CallReasonDto>  CallReasons { get; set; } = new();
    }

    /// <summary>Envoyé par le frontend pour sauvegarder l'évaluation</summary>
    public class EvaluationSaveDto
    {
        public int SurveyId         { get; set; }
        public int RecordId         { get; set; }
        public string? Memo         { get; set; }
        public string? MemoActionTaken { get; set; }
        public int? CategoryId      { get; set; }
        public int? CallReasonId    { get; set; }
        public string? CcEmail      { get; set; }   // email agent pour notification

        public List<SurveyItemSaveDto> Items { get; set; } = new();
    }

    public class SurveyItemSaveDto
    {
        public int Id       { get; set; }
        public string? Value { get; set; }   // "Y", "N", "NA"
        public string? Memo  { get; set; }
    }

    /// <summary>Item enrichi avec métadonnées template (lecture grille)</summary>
    public class SurveyItemDto
    {
        public int Id              { get; set; }
        public int SurveyId        { get; set; }
        public int TemplateItemId  { get; set; }
        public string? Value       { get; set; }
        public string? Memo        { get; set; }

        // Métadonnées template
        public string? SectionName { get; set; }
        public int SectionOrder    { get; set; }
        public string? Question    { get; set; }
        public string? Definition  { get; set; }
        public int ItemOrder       { get; set; }
        public int? MinValue       { get; set; }
        public int? MaxValue       { get; set; }
        public bool AllowNA        { get; set; }
    }

    public class CategoryDto
    {
        public int Id        { get; set; }
        public string Libelle { get; set; } = string.Empty;
    }

    public class CallReasonDto
    {
        public int Id        { get; set; }
        public string Libelle { get; set; } = string.Empty;
    }

    // ══════════════════════════════════════════════
    //  FILTER DTOs
    // ══════════════════════════════════════════════

    public class UserFilterDto
    {
        public int Id          { get; set; }
        public string Name     { get; set; } = string.Empty;
        public string? Expression { get; set; }
        public string? SqlWhere   { get; set; }
        public string? Type       { get; set; }
    }

    public class CreateFilterDto
    {
        public string Name     { get; set; } = string.Empty;
        public string? Expression { get; set; }
        public string? SqlWhere   { get; set; }
        public string? Type       { get; set; }
    }

    public class ViewConfigDto
    {
        public int Id            { get; set; }
        public string Name       { get; set; } = string.Empty;
        public string LayoutJson { get; set; } = string.Empty;
        public bool IsDefault    { get; set; }
    }

    public class CreateViewConfigDto
    {
        public string Name       { get; set; } = string.Empty;
        public string LayoutJson { get; set; } = string.Empty;
        public bool IsDefault    { get; set; }
    }
}