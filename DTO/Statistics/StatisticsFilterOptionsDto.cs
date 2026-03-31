namespace scoring_Backend.DTO.Statistics
{
    public class StatisticsFilterOptionsDto
    {
        public List<TeamOptionDto> Teams { get; set; } = new();
        public List<string> Campaigns { get; set; } = new();
        public List<AgentOptionDto> Agents { get; set; } = new();
    }

    public class TeamOptionDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = "";
    }

    public class AgentOptionDto
    {
        public string AgentOid { get; set; } = "";
        public string FullName { get; set; } = "";
    }
}