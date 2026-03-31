namespace scoring_Backend.DTO.Statistics
{
    public class StatisticsResponseDto
    {
        public List<StatisticRowDto> Rows { get; set; } = new();
        public List<ChartPointDto> Chart { get; set; } = new();
        public int Total { get; set; }
    }
}