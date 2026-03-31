using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO.Statistics;
using scoring_Backend.Services;

namespace scoring_Backend.Controllers.Statistique
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("filters")]
        public async Task<ActionResult<StatisticsFilterOptionsDto>> GetFilters()
        {
            var result = await _statisticsService.GetFilterOptionsAsync();
            return Ok(result);
        }

        [HttpPost("search")]
        public async Task<ActionResult<StatisticsResponseDto>> Search([FromBody] StatisticsFilterDto filter)
        {
            var result = await _statisticsService.SearchAsync(filter);
            return Ok(result);
        }

        [HttpPost("export/csv")]
        public async Task<IActionResult> ExportCsv([FromBody] StatisticsFilterDto filter)
        {
            var result = await _statisticsService.SearchAsync(filter);

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("RecordId,SurveyId,RecordDataId,Score,Campaign,NomAgent,PrenomAgent,PhoneNumber,AgentOid,TeamId,TeamName,CallLocalTime,DateEval,Comment");

            foreach (var row in result.Rows)
            {
                sb.AppendLine(string.Join(",", new[]
                {
                    Csv(row.RecordId.ToString()),
                    Csv(row.SurveyId.ToString()),
                    Csv(row.RecordDataId?.ToString() ?? ""),
                    Csv(row.Score?.ToString() ?? ""),
                    Csv(row.Campaign),
                    Csv(row.NomAgent),
                    Csv(row.PrenomAgent),
                    Csv(row.PhoneNumber),
                    Csv(row.AgentOid),
                    Csv(row.TeamId?.ToString() ?? ""),
                    Csv(row.TeamName),
                    Csv(row.CallLocalTime?.ToString("s") ?? ""),
                    Csv(row.DateEval?.ToString("s") ?? ""),
                    Csv(row.Comment)
                }));
            }

            return File(
                System.Text.Encoding.UTF8.GetBytes(sb.ToString()),
                "text/csv",
                "statistics.csv"
            );
        }

        private static string Csv(string value)
        {
            return $"\"{(value ?? "").Replace("\"", "\"\"")}\"";
        }
    }
}