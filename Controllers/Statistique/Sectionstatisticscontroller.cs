using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO.Statistique;
using scoring_Backend.Repositories.Interfaces.Statistique;

namespace scoring_Backend.Controllers.Statistique
{
    [ApiController]
    [Route("api/statistique2")]
    public class StatistiqueController2 : ControllerBase
    {
        private readonly IStatistiqueRepository2 _repo;

        public StatistiqueController2(IStatistiqueRepository2 repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Retourne les données brutes filtrées depuis Ls_survey / Ls_surveyItem.
        /// Le frontend construit le pivot dynamiquement.
        /// </summary>
        [HttpGet("data")]
        public async Task<IActionResult> GetData([FromQuery] StatistiqueFilterDto filter)
        {
            if (filter.DateDebut > filter.DateFin)
                return BadRequest("La date de début doit être antérieure à la date de fin.");

            var data = await _repo.GetStatistiqueDataAsync(filter);
            return Ok(data);
        }

        /// <summary>
        /// Retourne la liste des agents disponibles selon le rôle de l'utilisateur connecté.
        /// </summary>
        [HttpGet("agents")]
        public async Task<IActionResult> GetAgents([FromQuery] int userId, [FromQuery] int userRole, [FromQuery] int siteId, [FromQuery] bool allSupervisors = true)
        {
            var agents = await _repo.GetAgentsAsync(userId, userRole, siteId, allSupervisors);
            return Ok(agents);
        }

        /// <summary>
        /// Retourne les campagnes qualité accessibles à l'utilisateur.
        /// </summary>
        [HttpGet("campaigns")]
        public async Task<IActionResult> GetCampaigns([FromQuery] int userId, [FromQuery] int siteId)
        {
            var campaigns = await _repo.GetCampaignsAsync(userId, siteId);
            return Ok(campaigns);
        }

        /// <summary>
        /// Export en PDF / XLS / CSV / RTF côté serveur (optionnel si le frontend exporte lui-même).
        /// </summary>
        [HttpPost("export")]
        public async Task<IActionResult> Export([FromBody] StatistiqueExportDto request)
        {
            var fileBytes = await _repo.ExportAsync(request);
            string contentType = request.Format switch
            {
                "PDF" => "application/pdf",
                "XLS" => "application/vnd.ms-excel",
                "CSV" => "text/csv",
                "RTF" => "application/rtf",
                _ => "application/octet-stream"
            };
            string fileName = $"statistique_{DateTime.Now:yyyyMMdd_HHmmss}.{request.Format.ToLower()}";
            return File(fileBytes, contentType, fileName);
        }
    }
}