namespace scoring_Backend.DTO.Dashboard
{
    public class DashboardResponseDto
    {
        public DashboardSummaryDto Stats { get; set; } = new();
        public List<DashboardRowDto> EvaluationsByAuditor { get; set; } = new();
        public List<DashboardRowDto> ListeningsBySupervisor { get; set; } = new();
        public List<DashboardRowDto> Top5Agents { get; set; } = new();
        public List<DashboardRowDto> CustomerConcerns { get; set; } = new();
    }
}