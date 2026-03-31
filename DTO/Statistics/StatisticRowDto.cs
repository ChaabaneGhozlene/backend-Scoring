namespace scoring_Backend.DTO.Statistics
{
    public class StatisticRowDto
    {
        public int RecordId { get; set; }
        public int SurveyId { get; set; }
        public int? RecordDataId { get; set; }
        public decimal? Score { get; set; }

        public string Campaign { get; set; } = "";
        public string NomAgent { get; set; } = "";
        public string PrenomAgent { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string AgentOid { get; set; } = "";
        public string TeamName { get; set; } = "";
        public int? TeamId { get; set; }

        public DateTime? CallLocalTime { get; set; }
        public DateTime? DateEval { get; set; }
        public string Comment { get; set; } = "";
    }
}