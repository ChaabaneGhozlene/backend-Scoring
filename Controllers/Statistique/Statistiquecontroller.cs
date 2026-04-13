using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO;
using scoring_Backend.Helpers;
using scoring_Backend.DTO.Statistique;  
using scoring_Backend.Repositories.Interfaces.Statistique;
using System.Security.Claims;

namespace scoring_Backend.Controllers.Statistique
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StatistiqueController : ControllerBase
    {
        private readonly IStatistiqueRepository _statistiqueRepository;

        // ─── Helpers ──────────────────────────────────────────────────────────
        private int GetUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        private int GetSiteId() =>
            int.Parse(User.FindFirst("siteId")?.Value ?? "0");

        // Le rôle est stocké comme string dans le token (ex: "SuperAdmin", "Admin"...)
        // → MapRole() dans JwtService fait : 1=SuperAdmin, 2=Admin, 3=Supervisor, 4=Agent, 5=SuperUser
        private int GetUserRole() =>
            User.FindFirst(ClaimTypes.Role)?.Value switch
            {
                "SuperAdmin" => 1,
                "Admin"      => 2,
                "Supervisor" => 3,
                "Agent"      => 4,
                "SuperUser"  => 5,
                _            => 0
            };

        public StatistiqueController(IStatistiqueRepository statistiqueRepository)
        {
            _statistiqueRepository = statistiqueRepository;
        }

        // ─── Debug (à supprimer en production) ────────────────────────────────
        [HttpGet("debug-claims")]
        public IActionResult DebugClaims() =>
            Ok(new {
                userId   = GetUserId(),
                userRole = GetUserRole(),
                siteId   = GetSiteId(),
                claims   = User.Claims.Select(c => new { c.Type, c.Value })
            });

        // ─── Section Stats ────────────────────────────────────────────────────
        [HttpPost("section-stats")]
        public async Task<IActionResult> GetSectionStats([FromBody] StatFilterDto filter)
        {
            if (filter == null)
                return BadRequest(new { message = "Le filtre est requis." });

            if (filter.DateFrom > filter.DateTo)
                return BadRequest(new { message = "La date de début doit être antérieure à la date de fin." });

            try
            {
                var data = await _statistiqueRepository.GetSectionStatsAsync(
                    filter,
                    GetUserId(),
                    GetUserRole(),
                    GetSiteId());
                return Ok(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR: " + ex.ToString());
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner   = ex.InnerException?.Message,
                    stack   = ex.StackTrace
                });
            }
        }

        // ─── Supervisors ──────────────────────────────────────────────────────
        [HttpGet("supervisors")]
        public async Task<IActionResult> GetSupervisors()
        {
            try
            {
                var data = await _statistiqueRepository.GetSupervisorsAsync(
                    GetUserId(),
                    GetUserRole(),
                    GetSiteId());
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ─── Agent Scores ─────────────────────────────────────────────────────
        [HttpPost("agent-scores")]
        public async Task<IActionResult> GetAgentScores(
            [FromBody] StatFilterDto filter,
            [FromQuery] string sortDirection = "Descending")
        {
            try
            {
                var data = await _statistiqueRepository.GetAgentScoresAsync(
                    filter,
                    GetUserId(),
                    GetUserRole(),
                    GetSiteId(),
                    sortDirection);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ─── Program Level ────────────────────────────────────────────────────
        [HttpPost("program-level")]
        public async Task<IActionResult> GetProgramLevel(
            [FromBody] StatFilterDto filter,
            [FromQuery] bool allSupervisors = true)
        {
            try
            {
                var data = await _statistiqueRepository.GetProgramLevelAsync(
                    filter, GetUserId(), GetUserRole(), GetSiteId(), allSupervisors);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ─── Coaching Sheet ───────────────────────────────────────────────────
        [HttpPost("coaching-sheet")]
        public async Task<IActionResult> GetCoachingSheet(
            [FromBody] StatFilterDto filter,
            [FromQuery] int agentId = 0,
            [FromQuery] bool allSupervisors = false)
        {
            try
            {
                var data = await _statistiqueRepository.GetCoachingSheetAsync(
                    filter, agentId, GetUserId(), GetUserRole(), allSupervisors);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ─── Coaching Analysis ────────────────────────────────────────────────
        [HttpPost("coaching-analysis")]
        public async Task<IActionResult> GetCoachingAnalysis(
            [FromBody] StatFilterDto filter,
            [FromQuery] int agentId = 0,
            [FromQuery] bool allSupervisors = true)
        {
            try
            {
                var data = await _statistiqueRepository.GetCoachingAnalysisAsync(
                    filter, agentId, GetUserId(), GetUserRole(), allSupervisors);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ─── Coaching Summary ─────────────────────────────────────────────────
        [HttpPost("coaching-summary")]
        public async Task<IActionResult> GetCoachingSummary(
            [FromBody] StatFilterDto filter,
            [FromQuery] int agentId = 0,
            [FromQuery] bool allSupervisors = true)
        {
            try
            {
                var data = await _statistiqueRepository.GetCoachingSummaryAsync(
                    filter, agentId, GetUserId(), GetUserRole(), allSupervisors);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ─── Agent List ───────────────────────────────────────────────────────
        [HttpGet("agents")]
        public async Task<IActionResult> GetAgentList([FromQuery] bool allSupervisors = true)
        {
            try
            {
                var data = await _statistiqueRepository.GetAgentListAsync(
                    GetUserId(), GetUserRole(), GetSiteId(), allSupervisors);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ─── Export ───────────────────────────────────────────────────────────
       
[HttpPost("export")]
public async Task<IActionResult> Export([FromBody] ExportRequestDto request)
{
    try
    {
        if (request == null)
            return BadRequest(new { message = "Corps de la requête manquant." });
 
        // Récupérer les données selon le type de rapport
        object? data = request.ReportType switch
        {
            "section-stats"     => await _statistiqueRepository.GetSectionStatsAsync(
                                       request.Filter, GetUserId(), GetUserRole(), GetSiteId()),
            "agent-scores"      => await _statistiqueRepository.GetAgentScoresAsync(
                                       request.Filter, GetUserId(), GetUserRole(), GetSiteId(),
                                       request.SortDirection ?? "Descending"),
            "program-level"     => await _statistiqueRepository.GetProgramLevelAsync(
                                       request.Filter, GetUserId(), GetUserRole(), GetSiteId(),
                                       request.AllSupervisors),
            "coaching-sheet"    => await _statistiqueRepository.GetCoachingSheetAsync(
                                       request.Filter, request.AgentId ?? 0,
                                       GetUserId(), GetUserRole(), request.AllSupervisors),
            "coaching-analysis" => await _statistiqueRepository.GetCoachingAnalysisAsync(
                                       request.Filter, request.AgentId ?? 0,
                                       GetUserId(), GetUserRole(), request.AllSupervisors),
            "coaching-summary"  => await _statistiqueRepository.GetCoachingSummaryAsync(
                                       request.Filter, request.AgentId ?? 0,
                                       GetUserId(), GetUserRole(), request.AllSupervisors),
            _ => null
        };
 
        if (data == null)
            return BadRequest(new { message = $"Type de rapport inconnu : {request.ReportType}" });
 
        // Générer le fichier selon le format
        byte[] bytes;
        string contentType;
        string fileName;
 
        switch (request.Format?.ToUpper())
        {
            case "PDF":
                bytes       = ExportHelper.ToPdf(data, request.ReportType, request.Filter, request.ChartImage);
                contentType = "application/pdf";
                fileName    = $"{request.ReportType}_{DateTime.Now:yyyyMMdd}.pdf";
                break;
 
            case "XLS":
                bytes       = ExportHelper.ToXls(data);
                contentType = "application/vnd.ms-excel";
                fileName    = $"{request.ReportType}_{DateTime.Now:yyyyMMdd}.xls";
                break;
 
            case "CSV":
                bytes       = ExportHelper.ToCsv(data);
                contentType = "text/csv; charset=utf-8";
                fileName    = $"{request.ReportType}_{DateTime.Now:yyyyMMdd}.csv";
                break;
 
            case "RTF":
                bytes       = ExportHelper.ToRtf(data, request.ReportType, request.Filter);
                contentType = "application/rtf";
                fileName    = $"{request.ReportType}_{DateTime.Now:yyyyMMdd}.rtf";
                break;
 
            default:
                return BadRequest(new { message = $"Format non supporté : {request.Format}" });
        }
 
        // Forcer le téléchargement côté navigateur
        Response.Headers.Append(
            "Content-Disposition",
            $"attachment; filename=\"{fileName}\""
        );
 
        return File(bytes, contentType, fileName);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Export error: {ex}");
        return StatusCode(500, new { message = ex.Message, inner = ex.InnerException?.Message });
    }
}
    }
}