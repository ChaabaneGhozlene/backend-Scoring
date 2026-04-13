using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO.Statistique;
using scoring_Backend.Repositories.Interfaces.Statistique;
using System.Security.Claims;

namespace scoring_Backend.Controllers.Statistique
{
    [Authorize]
    [ApiController]
    [Route("api/statistique/[controller]")]
    public class SectionStatController : ControllerBase
    {
        private readonly ISectionStatRepository _repo;

        public SectionStatController(ISectionStatRepository repo)
        {
            _repo = repo;
        }

        // POST api/statistique/sectionstat/search
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SectionStatFilterDto filter)
        {
            if (filter.DateFin < filter.DateDebut)
                return BadRequest(new { message = "La date de fin doit être >= à la date de début." });

            // Récupérer l'utilisateur connecté depuis le JWT
            var userId   = int.TryParse(User.FindFirstValue("userId"),   out var uid)  ? uid  : (int?)null;
            var userRole = int.TryParse(User.FindFirstValue("userRole"),  out var role) ? role : (int?)null;
            var siteId   = int.TryParse(User.FindFirstValue("siteId"),    out var site) ? site : (int?)null;

            var result = await _repo.GetSectionStatsAsync(filter, userId, userRole, siteId);
            return Ok(result);
        }

        // POST api/statistique/sectionstat/export
        // Renvoie un fichier CSV simple (le front peut aussi exporter via xlsx-js / jsPDF)
        [HttpPost("export")]
        public async Task<IActionResult> Export(
            [FromBody] SectionStatFilterDto filter,
            [FromQuery] string format = "csv")
        {
            var userId   = int.TryParse(User.FindFirstValue("userId"),   out var uid)  ? uid  : (int?)null;
            var userRole = int.TryParse(User.FindFirstValue("userRole"),  out var role) ? role : (int?)null;
            var siteId   = int.TryParse(User.FindFirstValue("siteId"),    out var site) ? site : (int?)null;

            var result = await _repo.GetSectionStatsAsync(filter, userId, userRole, siteId);

            // Export CSV simple
            var csv = new System.Text.StringBuilder();
            csv.AppendLine("Section ID;Section;Agent;Agent ID;Campaign;Score (%)");
            foreach (var row in result.Rows)
                csv.AppendLine($"{row.SectionId};{row.Section};{row.Agent};{row.AgentId};{row.Campaign};{row.ScorePercent}");

            var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "SectionStats.csv");
        }
    }
}