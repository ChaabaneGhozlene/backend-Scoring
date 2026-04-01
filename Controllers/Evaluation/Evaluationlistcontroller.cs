// ============================================================
// EvaluationListController
// Routes : /api/evaluation-list
// Migre : ListeEvaluation.aspx.cs
// ============================================================
using System.Text;
using scoring_Backend.Models.Scoring;                    // ← add this
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO;
using scoring_Backend.Repositories.Interfaces.Evaluation;

namespace scoring_Backend.Controllers.Evaluation
{
    [ApiController]
    [Route("api/evaluation-list")]
    [Authorize]
    public class EvaluationListController : ControllerBase
    {
        private readonly IEvaluationListRepository _repo;

        public EvaluationListController(IEvaluationListRepository repo) => _repo = repo;

        // ── Claims helpers ──────────────────────────────────
        private int    UserId    => int.Parse(User.FindFirstValue("UserId")    ?? "0");
        private int    UserRole  => int.Parse(User.FindFirstValue("UserRole")  ?? "0");
        private int    UserType  => int.Parse(User.FindFirstValue("UserType")  ?? "0");
        private int    UserSite  => int.Parse(User.FindFirstValue("UserSite")  ?? "0");

        // ────────────────────────────────────────────────────
        // GET /api/evaluation-list
        // Liste des fiches Ls (paginée, filtrée par rôle)
        // ────────────────────────────────────────────────────
        /// <summary>
        /// Retourne la liste paginée des fiches d'écoute
        /// selon le rôle et la période.
        /// </summary>
            private string UserLogin => User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

        [HttpPost("search")]

public async Task<IActionResult> GetLsFiches([FromBody] EvaluationListFilterDto filter)
{
    try
    {
        var (items, total) = await _repo.GetLsFichesAsync(
    UserId, UserRole, UserType, UserSite, UserLogin, filter);

        return Ok(new
        {
            TotalCount = total,
            Page       = filter.Page,
            PageSize   = filter.PageSize,
            Items      = items
        });
    }
    catch (Exception ex)
    {
        // ← Remplacer l'ancien catch par celui-ci
        return StatusCode(500, new
        {
            message    = ex.Message,
            stackTrace = ex.StackTrace,
            inner      = ex.InnerException?.Message
        });
    }
}

        // ────────────────────────────────────────────────────
        // GET /api/evaluation-list/{lsId}/surveys
        // Surveys d'une fiche (lignes de détail)
        // ────────────────────────────────────────────────────
        /// <summary>
        /// Retourne toutes les évaluations (Ls_survey)
        /// d'une fiche d'écoute donnée.
        /// </summary>
        [HttpGet("{lsId:int}/surveys")]
        public async Task<IActionResult> GetSurveys(int lsId)
        {
            try
            {
                var surveys = await _repo.GetSurveysByLsIdAsync(lsId);
                return Ok(surveys);
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

        // ────────────────────────────────────────────────────
        // GET /api/evaluation-list/surveys/{surveyId}/items
        // Chargement des items pour édition
        // ────────────────────────────────────────────────────
        /// <summary>
        /// Retourne les items (questions/notes) d'une
        /// évaluation pour permettre sa modification.
        /// </summary>
        [HttpGet("surveys/{surveyId:int}/items")]
        public async Task<IActionResult> GetSurveyItems(int surveyId)
        {
            try
            {
                // Vérifier la validité de la période avant d'ouvrir l'édition
               // var lsId = await GetLsIdFromSurvey(surveyId);
               // bool valid = await _repo.IsLsPeriodValidAsync(lsId);
                /*if (!valid)
                    return BadRequest(new
                    {
                        message = "La période d'évaluation n'est plus valide."
                    });
*/
                var items = await _repo.GetSurveyItemsAsync(surveyId);
                return Ok(items);
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

        // ────────────────────────────────────────────────────
        // PUT /api/evaluation-list/surveys/{surveyId}
        // Mise à jour d'une évaluation existante
        // ────────────────────────────────────────────────────
        /// <summary>
        /// Met à jour les notes, commentaires et métadonnées
        /// d'une évaluation. Recalcule le score automatiquement.
        /// </summary>
        [HttpPut("surveys/{surveyId:int}")]
        public async Task<IActionResult> UpdateSurvey(
            int surveyId, [FromBody] UpdateSurveyDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _repo.UpdateSurveyAsync(surveyId, UserId, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ────────────────────────────────────────────────────
        // DELETE /api/evaluation-list/surveys/{surveyId}
        // Suppression d'une évaluation individuelle
        // ────────────────────────────────────────────────────
        /// <summary>
        /// Supprime une évaluation (Ls_survey) et détache
        /// l'enregistrement audio associé.
        /// </summary>
       
[HttpDelete("surveys/{surveyId:int}")]
public async Task<IActionResult> DeleteSurvey(int surveyId)
{
    try
    {
        await _repo.DeleteSurveyAsync(surveyId);
        return NoContent();
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
    catch (Exception ex)
    {
        // ← Retourner le détail complet de l'erreur
        return StatusCode(500, new
        {
            message    = ex.Message,
            inner      = ex.InnerException?.Message,
            stackTrace = ex.StackTrace
        });
    }
}
        // ────────────────────────────────────────────────────
        // DELETE /api/evaluation-list/{lsId}
        // Suppression d'une fiche complète (cascade)
        // ────────────────────────────────────────────────────
        /// <summary>
        /// Supprime une fiche d'écoute complète avec toutes
        /// ses évaluations et détache les enregistrements.
        /// </summary>
        [HttpDelete("{lsId:int}")]
        public async Task<IActionResult> DeleteLsFiche(int lsId)
        {
            try
            {
                await _repo.DeleteLsFicheAsync(lsId);
                return NoContent();
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

        // ────────────────────────────────────────────────────
        // GET /api/evaluation-list/{lsId}/agent-report
        // Fiche agent — rapport de synthèse complet
        // ────────────────────────────────────────────────────
        /// <summary>
        /// Génère le rapport complet de la fiche agent :
        /// informations d'en-tête, grille pivot, scores
        /// par section et score total.
        /// </summary>
       [HttpGet("{lsId:int}/agent-report")]
public async Task<IActionResult> GetAgentReport(int lsId, [FromQuery] int recordDataId = 0)
{
    try
    {
        var report = await _repo.GetAgentReportAsync(lsId, recordDataId);
        return Ok(report);
    }
    catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
}
// EvaluationListController.cs
[HttpPost("export")]
public async Task<IActionResult> ExportLsFiches([FromBody] EvaluationListFilterDto filter)
{
    filter.Page     = 1;
    filter.PageSize = 99999;

    var (items, _) = await _repo.GetLsFichesAsync(
        UserId, UserRole, UserType, UserSite, UserLogin, filter);

    var csv = new StringBuilder();
    csv.AppendLine("AgentId,Agent,ID,Score,Modèle,Auditeur,Campagne");

    foreach (var item in items)
    {
        csv.AppendLine(
            $"{item.AgentId},{item.Agent},{item.Id}," +
            $"{item.Score},{item.ModeleName},{item.Auditor},{item.CampaignName}"
        );
    }

    var bytes = Encoding.UTF8.GetBytes(csv.ToString());
    return File(bytes, "text/csv", "evaluations.csv");
}
        // ────────────────────────────────────────────────────
        // Helper privé
        // ────────────────────────────────────────────────────
        private async Task<int> GetLsIdFromSurvey(int surveyId)
{
    using var scope = HttpContext.RequestServices.CreateScope();
    var db = scope.ServiceProvider
                  .GetRequiredService<SqrScoringContext>();

    return db.LsSurveys
        .Where(s => s.Id == surveyId)
        .Select(s => s.LsId)
        .FirstOrDefault();
}
[HttpGet("{lsId:int}/period-valid")]
public async Task<IActionResult> CheckPeriodValidity(int lsId)
{
    var isValid = await _repo.IsLsPeriodValidAsync(lsId);
    return Ok(new { isValid });
}






    }
}