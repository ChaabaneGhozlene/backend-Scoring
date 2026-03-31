namespace scoring_Backend.DTO.Statistics
{
    public class StatisticsFilterDto
    {
        public string? From { get; set; }
        public string? To { get; set; }
        public int? TeamId { get; set; }
        public string? Campaign { get; set; }
        public string? AgentOid { get; set; }
        public string SummaryType { get; set; } = "avg";
        public string GroupBy { get; set; } = "campaign";
    }
}