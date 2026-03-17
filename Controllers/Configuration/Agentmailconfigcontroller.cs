using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO.Configuration;
using scoring_Backend.Repositories.Interfaces.Configuration;
using System.Security.Claims;

namespace scoring_Backend.Controllers.Configuration
{
    /// <summary>
    /// Controller Notification Setting — gestion des emails de notification des agents.
    ///
    /// GET    /api/configuration/agent-mail          => liste agents + email
    /// GET    /api/configuration/agent-mail/{oid}    => détail agent pour popup édition
    /// PUT    /api/configuration/agent-mail          => upsert email agent
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/configuration/agent-mail")]
    public class AgentMailConfigController : ControllerBase
    {
        private readonly IAgentMailConfigRepository _repository;
        private readonly ILogger<AgentMailConfigController> _logger;

        public AgentMailConfigController(
            IAgentMailConfigRepository repository,
            ILogger<AgentMailConfigController> logger)
        {
            _repository = repository;
            _logger     = logger;
        }

        // ──────────────────────────────────────────────────────────────────────
        // GET /api/configuration/agent-mail
        // ──────────────────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetAgentsWithEmail()
        {
            try
            {
                var (userId, userRole) = GetUserContext();
                var agents = await _repository.GetAgentsWithEmailAsync(userId, userRole);
                return Ok(agents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur GetAgentsWithEmail");
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner   = ex.InnerException?.Message
                });
            }
        }

        // ──────────────────────────────────────────────────────────────────────
        // GET /api/configuration/agent-mail/{oid}
        // ──────────────────────────────────────────────────────────────────────
        [HttpGet("{oid}")]
        public async Task<IActionResult> GetAgentEditDetail(string oid)
        {
            if (string.IsNullOrWhiteSpace(oid))
                return BadRequest(new { message = "L'identifiant OID est requis." });

            try
            {
                var detail = await _repository.GetAgentEditDetailAsync(oid);
                if (detail == null)
                    return NotFound(new { message = "Agent introuvable." });

                return Ok(detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur GetAgentEditDetail OID={Oid}", oid);
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner   = ex.InnerException?.Message
                });
            }
        }

        // ──────────────────────────────────────────────────────────────────────
        // PUT /api/configuration/agent-mail
        // ──────────────────────────────────────────────────────────────────────
        [HttpPut]
        public async Task<IActionResult> UpsertAgentEmail([FromBody] UpdateAgentEmailDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(dto.Oid))
                return BadRequest(new { message = "L'OID de l'agent est requis." });

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "L'adresse email est requise." });

            try
            {
                await _repository.UpsertAgentEmailAsync(dto);
                return Ok(new { message = "Email mis à jour avec succès." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur UpsertAgentEmail OID={Oid}", dto.Oid);
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner   = ex.InnerException?.Message
                });
            }
        }

        // ──────────────────────────────────────────────────────────────────────
        // Helper — extrait userId et userRole (string) depuis le token JWT
        //
        // Le token contient ClaimTypes.Role = "SuperAdmin" | "AdminSite" |
        //                                     "Superviseur" | "Agent"
        // Ces valeurs sont générées par JwtService.MapRole(user.Role)
        // et lues dans AgentMailConfigRepository.GetAgentsWithEmailAsync()
        // ──────────────────────────────────────────────────────────────────────
        private (int userId, string userRole) GetUserContext()
        {
            var userIdClaim = User.FindFirst("userId")?.Value
                           ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Lire le rôle string directement depuis le claim JWT
            // Valeurs possibles : "SuperAdmin", "AdminSite", "Superviseur", "Agent"
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "Agent";

            int userId = int.TryParse(userIdClaim, out var uid) ? uid : 0;

            return (userId, userRole);
        }
    }
}