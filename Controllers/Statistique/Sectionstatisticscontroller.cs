// StatistiqueController2.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO.Statistique;
using scoring_Backend.Repositories.Interfaces.Statistique;
using System.Security.Claims;

namespace scoring_Backend.Controllers.Statistique
{
    [ApiController]
    [Route("api/statistique2")]
    [Authorize] // ✅ Toutes les routes nécessitent un JWT valide
    public class StatistiqueController2 : ControllerBase
    {
        private readonly IStatistiqueRepository2 _repo;

        public StatistiqueController2(IStatistiqueRepository2 repo)
        {
            _repo = repo;
        }

        // ─────────────────────────────────────────────────────────────────────
        // Helpers : lecture des claims JWT
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Mappe le nom de rôle métier (string) vers l'entier utilisé dans les requêtes SQL.
        /// Inverse de JwtService.MapRole().
        /// </summary>
        private static int MapRoleToInt(string roleName) => roleName.ToLowerInvariant() switch
        {
            "superadmin" => 1,
            "admin"      => 2,
            "supervisor" => 3,
            "agent"      => 4,
            "superuser"  => 5,
            _            => 4   // valeur par défaut sécurisée
        };

        private int GetUserId()
            => int.Parse(User.FindFirstValue("userId")
               ?? throw new UnauthorizedAccessException("Claim 'userId' manquant."));

        private int GetUserRole()
            => MapRoleToInt(User.FindFirstValue("userRole")
               ?? throw new UnauthorizedAccessException("Claim 'userRole' manquant."));

        private int GetSiteId()
            => int.Parse(User.FindFirstValue("siteId")
               ?? throw new UnauthorizedAccessException("Claim 'siteId' manquant."));

        // ─────────────────────────────────────────────────────────────────────
        // Endpoints
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Retourne les données brutes filtrées depuis Ls_survey / Ls_surveyItem.
        /// userId, userRole et siteId sont lus depuis le JWT — ne plus les passer en query.
        /// </summary>
        [HttpGet("data")]
        public async Task<IActionResult> GetData([FromQuery] StatistiqueFilterDto filter)
        {
            if (filter.DateDebut > filter.DateFin)
                return BadRequest("La date de début doit être antérieure à la date de fin.");

            // ✅ Injection des identifiants depuis le token
            filter.UserId   = GetUserId();
            filter.UserRole = GetUserRole();
            filter.SiteId   = GetSiteId();

            var data = await _repo.GetStatistiqueDataAsync(filter);
            return Ok(data);
        }

        /// <summary>
        /// Retourne la liste des agents disponibles selon le rôle de l'utilisateur connecté.
        /// Plus besoin de passer userId / userRole / siteId en query string.
        /// allSupervisors reste un paramètre query optionnel (booléen métier).
        /// </summary>
        [HttpGet("agents")]
        public async Task<IActionResult> GetAgents([FromQuery] bool allSupervisors = false)
        {
            var agents = await _repo.GetAgentsAsync(
                userId:         GetUserId(),
                userRole:       GetUserRole(),
                siteId:         GetSiteId(),
                allSupervisors: allSupervisors
            );
            return Ok(agents);
        }

        /// <summary>
        /// Retourne les campagnes qualité accessibles à l'utilisateur.
        /// userId et siteId proviennent du JWT.
        /// </summary>
        [HttpGet("campaigns")]
        public async Task<IActionResult> GetCampaigns()
        {
            var campaigns = await _repo.GetCampaignsAsync(
                userId: GetUserId(),
                siteId: GetSiteId()
            );
            return Ok(campaigns);
        }

        /// <summary>
        /// Export en CSV / JSON côté serveur.
        /// Les identifiants du filtre sont surchargés depuis le JWT.
        /// </summary>
        [HttpPost("export")]
        public async Task<IActionResult> Export([FromBody] StatistiqueExportDto request)
        {
            // ✅ Sécurité : on écrase toujours les champs sensibles du filtre par ceux du token
            request.Filter.UserId   = GetUserId();
            request.Filter.UserRole = GetUserRole();
            request.Filter.SiteId   = GetSiteId();

            var fileBytes = await _repo.ExportAsync(request);

            string contentType = request.Format switch
            {
                "PDF" => "application/pdf",
                "XLS" => "application/vnd.ms-excel",
                "CSV" => "text/csv",
                "RTF" => "application/rtf",
                _     => "application/octet-stream"
            };

            string fileName = $"statistique_{DateTime.Now:yyyyMMdd_HHmmss}.{request.Format.ToLower()}";
            return File(fileBytes, contentType, fileName);
        }
    }
}