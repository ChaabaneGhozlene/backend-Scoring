namespace scoring_Backend.DTO.Dashboard
{
    public class DashboardSummaryDto
    {
        public int TotalRecordings { get; set; }
        public int TotalEvaluations { get; set; }
        public decimal AverageScore { get; set; }
    }
}