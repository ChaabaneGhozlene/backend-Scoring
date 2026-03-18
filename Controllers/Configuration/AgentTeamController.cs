using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO;
using scoring_Backend.Repositories.Interfaces.Configuration;

namespace scoring_Backend.Controllers.Configuration
{   [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AgentTeamController : ControllerBase
    {
        private readonly IAgentTeamRepository         _repo;
        private readonly ILogger<AgentTeamController> _logger;

        public AgentTeamController(
            IAgentTeamRepository         repo,
            ILogger<AgentTeamController> logger)
        {
            _repo   = repo;
            _logger = logger;
        }

        // ── Lookups ──────────────────────────────────────────────────────────
 private (int userId, string userRole) GetUserContext()
        {
            // Log tous les claims pour déboguer
            foreach (var claim in User.Claims)
                _logger.LogInformation("Claim: {Type} = {Value}", claim.Type, claim.Value);

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue("sub")
                         ?? User.FindFirstValue("userId");

            var userRole  = User.FindFirstValue(ClaimTypes.Role)
                         ?? User.FindFirstValue("role")
                         ?? User.FindFirstValue("userRole")
                         ?? "Agent";

            if (userIdStr is null || !int.TryParse(userIdStr, out var userId))
                throw new UnauthorizedAccessException("Claim userId introuvable ou invalide.");

            return (userId, userRole);
        }
        [HttpGet("sites")]public async Task<IActionResult> GetSites()
{
    var (userId, userRole) = GetUserContext();
    return Ok(await _repo.GetSitesAsync(userId, userRole));
}

        [HttpGet("available-agents")]
        public async Task<IActionResult> GetAvailableAgents(
            [FromQuery] int  customerId,
            [FromQuery] int? excludeTeamId = null)
            => Ok(await _repo.GetAvailableAgentsAsync(customerId, excludeTeamId));

        // ── Groupes ───────────────────────────────────────────────────────────

        [HttpGet]
public async Task<IActionResult> GetAllTeams()
{try
            {
                var (userId, userRole) = GetUserContext();
                return Ok(await _repo.GetAllTeamsAsync(userId, userRole));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur GetAllTeams");
                return StatusCode(500, new { message = ex.Message, detail = ex.InnerException?.Message });
            }
}

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTeam(int id)
        {
            var data = await _repo.GetTeamByIdAsync(id);
            return data is null ? NotFound() : Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] CreateAgentTeamDto dto)
        {
            try
            {
                if (await _repo.TeamExistsByDescriptionAsync(dto.Description))
                    return Conflict(new { message = "Un groupe avec ce nom existe déjà." });

                var id = await _repo.CreateTeamAsync(dto);
                return CreatedAtAction(nameof(GetTeam), new { id }, new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur CreateTeam");
                return StatusCode(500, new { message = ex.Message, detail = ex.InnerException?.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] UpdateAgentTeamDto dto)
        {
            try
            {
                if (await _repo.GetTeamByIdAsync(id) is null)
                    return NotFound();

                await _repo.UpdateTeamAsync(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur UpdateTeam id={Id}", id);
                return StatusCode(500, new { message = ex.Message, detail = ex.InnerException?.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            try
            {
                if (await _repo.GetTeamByIdAsync(id) is null)
                    return NotFound();

                await _repo.DeleteTeamAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur DeleteTeam id={Id}", id);
                return StatusCode(500, new { message = ex.Message, detail = ex.InnerException?.Message });
            }
        }

        // ── Membres ───────────────────────────────────────────────────────────

        [HttpGet("{teamId:int}/members")]
        public async Task<IActionResult> GetMembers(int teamId)
        {
            if (await _repo.GetTeamByIdAsync(teamId) is null)
                return NotFound();

            return Ok(await _repo.GetMembersByTeamAsync(teamId));
        }

        // ── NOUVEAU : Supprimer des membres spécifiques d'un groupe ──────────

        /// <summary>
        /// Supprime des agents spécifiques d'un groupe (tAgentTeams uniquement).
        /// Les agents restent dans tListAgents — seule la liaison est supprimée.
        /// </summary>
        [HttpDelete("{teamId:int}/members")]
        public async Task<IActionResult> RemoveMembers(
            int teamId,
            [FromBody] RemoveMembersDto dto)
        {
            try
            {
                if (await _repo.GetTeamByIdAsync(teamId) is null)
                    return NotFound(new { message = "Groupe introuvable." });

                if (dto.AgentOids == null || dto.AgentOids.Count == 0)
                    return BadRequest(new { message = "Aucun agent spécifié." });

                await _repo.RemoveMembersAsync(teamId, dto.AgentOids);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur RemoveMembers teamId={TeamId}", teamId);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}