// DTO/DashboardDto.cs
namespace scoring_Backend.DTO.Dashboard
{
    public class DashboardFilterDto
    {
        public DateTime DateFrom { get; set; } = DateTime.Today.AddDays(-30);
        public DateTime DateTo   { get; set; } = DateTime.Today;
    }

    public class DashboardKpiDto
    {
        public int    NbListened    { get; set; }
        public int    NbEvaluated  { get; set; }
        public double AverageScore  { get; set; }
        public double EvalRate      { get; set; }
    }

    public class DashboardCampaignBarDto
    {
        public string Campaign { get; set; } = "";
        public int    Count    { get; set; }
    }

    public class DashboardEvolutionDto
    {
        public string Date  { get; set; } = "";
        public double Score { get; set; }
    }

    public class DashboardAgentScoreDto
    {
        public string Agent { get; set; } = "";
        public double Score { get; set; }
    }

    public class DashboardCategoryDto
    {
        public string Description { get; set; } = "";
        public int    Number      { get; set; }
    }

    public class DashboardFullDto
    {
        public DashboardKpiDto              Kpi                { get; set; } = new();
        public List<DashboardCampaignBarDto> ListenByCampaign  { get; set; } = new();
        public List<DashboardCampaignBarDto> EvalByCampaign    { get; set; } = new();
        public List<DashboardEvolutionDto>   Evolution          { get; set; } = new();
        public List<DashboardAgentScoreDto>  TopAgents          { get; set; } = new();
        public List<DashboardAgentScoreDto>  BottomAgents       { get; set; } = new();
        public List<DashboardCategoryDto>    TopCategories      { get; set; } = new();
        public List<DashboardAgentScoreDto>  EvalByAuditor      { get; set; } = new();
        public List<DashboardAgentScoreDto>  ListenBySupervisor { get; set; } = new();
    }
}