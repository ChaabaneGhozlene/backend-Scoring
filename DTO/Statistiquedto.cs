namespace scoring_Backend.DTO.Statistique
{
    public class StatFilterDto
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
            public bool AllSupervisors { get; set; } = false;
                public int?     SupervisorId  { get; set; }  // ← ajouter


    }
     public class SupervisorDto
    {
        public int    Id   { get; set; }
        public string Name { get; set; } = string.Empty;
    }

public class SectionStatsDto
    {
        public int    SectionId    { get; set; }
        public int    SectionOrder { get; set; }
        public string Section      { get; set; } = string.Empty;
        public string Agent        { get; set; } = string.Empty;
        public int    AgentId      { get; set; }
        public string Campaign     { get; set; } = string.Empty;

        /// <summary>Benchmark = nombre total de réponses (Count legacy)</summary>
        public int Reference { get; set; }

        /// <summary>Sum des scores positifs — type float comme en base</summary>
        public float ScoreGroup { get; set; }

        /// <summary>Pourcentage = (ScoreGroup / Reference) * 100</summary>
        public double Percentage =>
            Reference > 0 ? Math.Round((ScoreGroup / (double)Reference) * 100, 2) : 0d;

        public List<SectionQuestionDto> Questions { get; set; } = new();
    }

    public class SectionQuestionDto
    {
        public int    QuestionId    { get; set; }
        public int    QuestionOrder { get; set; }
        public string Description   { get; set; } = string.Empty;
        public int    GroupId       { get; set; }
    }
    public class AgentScoreDto
    {
        public string Agent { get; set; } = string.Empty;
        public double Score { get; set; }
    }
public class SectionRawRow
{
    public int     GroupId      { get; set; }
    public int     SectionOrder { get; set; }
    public string? Section      { get; set; }
    public string? Agent        { get; set; }
    public int     AgentId      { get; set; }
    public string? Campaign     { get; set; }
    public float   ScoreGroup   { get; set; }
}
    public class ProgramLevelDto
    {
        public string Agent { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public double Score { get; set; }
    }

    public class CoachingSheetDto
    {
        public int Id { get; set; }
        public string CallIndex { get; set; } = string.Empty;
        public double EvaluationScore { get; set; }
        public string Question { get; set; } = string.Empty;
        public double ItemScore { get; set; }
        public string Comment { get; set; } = string.Empty;
    }

    public class CoachingAnalysisDto
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public string Section { get; set; } = string.Empty;
        public string ErrorType { get; set; } = string.Empty;
        public int Occurrence { get; set; }
        public int PositiveAnswers { get; set; }
        public double LoseRate { get; set; }
        public double Value { get; set; }
    }

    public class CoachingSummaryDto
    {
        public int Id { get; set; }
        public string CallIndex { get; set; } = string.Empty;
        public double Score { get; set; }
        public string Comment { get; set; } = string.Empty;
    }

    public class AgentListDto
    {
        public int Id { get; set; }
        public string Agent { get; set; } = string.Empty;
    }

    public class ExportRequestDto
    {
        public string ReportType { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public StatFilterDto Filter { get; set; } = new();
        public int? AgentId { get; set; }
        public bool AllSupervisors { get; set; } = true;
        public string SortDirection { get; set; } = "Descending";
            public int?         SupervisorId   { get; set; }  // ← ajouter

    }
}