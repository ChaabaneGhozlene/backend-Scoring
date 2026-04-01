// ============================================================
// Controllers/Evaluation/EvaluationController.cs
// ============================================================

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO;
using scoring_Backend.Repositories.Interfaces.Evaluation;

namespace scoring_Backend.Controllers.Evaluation
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EvaluationController : ControllerBase
    {
        private readonly IEvaluationRepository _repo;

        public EvaluationController(IEvaluationRepository repo) => _repo = repo;

        // ── Claims ──────────────────────────────────────────────
        private int    CurrentUserId    => int.Parse(User.FindFirstValue("UserId")   ?? "0");
        private string CurrentUserLogin => User.FindFirstValue(ClaimTypes.Name)      ?? string.Empty;
        private int    CurrentUserRole  => int.Parse(User.FindFirstValue("UserRole") ?? "0");
        private int    CurrentUserSite  => int.Parse(User.FindFirstValue("UserSite") ?? "0");

        // ══════════════════════════════════════════════════════
        // POST /api/evaluation/open
        // ══════════════════════════════════════════════════════
        /// <summary>
        /// Ouvre une fiche d'évaluation pour un enregistrement.
        /// Crée automatiquement la fiche Ls + LsSurvey si nécessaire.
        /// Retourne la grille de notation pré-remplie.
        /// </summary>
        [HttpPost("open")]
public async Task<IActionResult> Open([FromBody] OpenEvaluationRequestDto dto)
{
    if (dto == null || dto.RecordId <= 0)
        return BadRequest("RecordId invalide.");

    try
    {
        var result = await _repo.OpenEvaluationAsync(
            dto.RecordId, CurrentUserId, CurrentUserLogin);

        if (result.SurveyId == 0)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(result);
    }
    catch (Exception ex)
    {
        // Retourner le détail complet temporairement
        return StatusCode(500, new { 
            message    = ex.Message,
            inner      = ex.InnerException?.Message,
            stackTrace = ex.StackTrace   // ← à retirer en prod
        });
    }
}

        // ══════════════════════════════════════════════════════
        // POST /api/evaluation/save
        // ══════════════════════════════════════════════════════
        /// <summary>
        /// Sauvegarde les notes de l'évaluation, recalcule le score
        /// et envoie l'e-mail récapitulatif à l'agent.
        /// </summary>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SaveEvaluationDto dto)
        {
            if (dto == null || dto.SurveyId <= 0)
                return BadRequest("SurveyId invalide.");

            try
            {
                var result = await _repo.SaveEvaluationAsync(
                    dto, CurrentUserId, CurrentUserLogin);

                if (!result.Success)
                    return BadRequest(new { message = result.Message });

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════
        // POST /api/evaluation/requalify
        // ══════════════════════════════════════════════════════
        /// <summary>Requalifie un enregistrement (Hermess ou client).</summary>
        [HttpPost("requalify")]
        public async Task<IActionResult> Requalify([FromBody] RequalificationDto dto)
        {
            if (dto == null || dto.RecordId <= 0)
                return BadRequest("RecordId invalide.");

            try
            {
                await _repo.RequalifyRecordAsync(dto, CurrentUserId);
                return Ok(new { message = "Requalification effectuée." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════
        // GET /api/evaluation/categories
        // ══════════════════════════════════════════════════════
        /// <summary>Retourne la liste des catégories d'évaluation.</summary>
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var list = await _repo.GetCategoriesAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════
        // GET /api/evaluation/call-reasons
        // ══════════════════════════════════════════════════════
        /// <summary>Retourne la liste des raisons d'appel.</summary>
        [HttpGet("call-reasons")]
        public async Task<IActionResult> GetCallReasons()
        {
            try
            {
                var list = await _repo.GetCallReasonsAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════
        // GET /api/evaluation/agents
        // ══════════════════════════════════════════════════════
        /// <summary>Retourne la liste des agents visibles selon le rôle.</summary>
        [HttpGet("agents")]
        public async Task<IActionResult> GetAgents()
        {
            try
            {
                var list = await _repo.GetAgentsAsync(
                    CurrentUserId, CurrentUserRole, CurrentUserSite);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════
        // GET /api/evaluation/campaign-qualities
        // ══════════════════════════════════════════════════════
        /// <summary>Retourne les campagnes qualité actives visibles par l'utilisateur.</summary>
        [HttpGet("campaign-qualities")]
        public async Task<IActionResult> GetCampaignQualities()
        {
            try
            {
                var list = await _repo.GetCampaignQualitiesAsync(
                    CurrentUserId.ToString(), CurrentUserSite, CurrentUserRole);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════
        // GET /api/evaluation/call-status/{customerId}/{campaignId}/{callType}
        // ══════════════════════════════════════════════════════
        /// <summary>
        /// Retourne les statuts d'appel Hermess disponibles
        /// pour une campagne et un type d'appel donnés.
        /// </summary>
        [HttpGet("call-status/{customerId}/{campaignId:int}/{callType:int}")]
        public async Task<IActionResult> GetCallStatus(
            string customerId, int campaignId, int callType)
        {
            try
            {
                var list = await _repo.GetCallStatusItemsAsync(
                    customerId, campaignId.ToString(), callType);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        // GET /api/evaluation/surveys/{recordId}
[HttpGet("surveys/{recordId:int}")]
public async Task<IActionResult> GetAllSurveys(int recordId)
{
    try
    {
        var result = await _repo.GetAllSurveysForRecordAsync(recordId);
        return Ok(result);
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = ex.Message });
    }
}
    }
}