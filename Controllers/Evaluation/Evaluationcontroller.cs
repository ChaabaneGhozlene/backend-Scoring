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
    {private readonly IEvaluationRepository            _repo;
        private readonly ILogger<EvaluationController>    _logger;
        private readonly IConfiguration                   _configuration;

        // ── Constructeur corrigé avec toutes les dépendances ──
        public EvaluationController(
            IEvaluationRepository         repo,
            ILogger<EvaluationController> logger,
            IConfiguration                configuration)
        {
            _repo          = repo;
            _logger        = logger;
            _configuration = configuration;
        }
        // ── Claims ──────────────────────────────────────────────
        private int    CurrentUserId    => int.Parse(User.FindFirstValue("userId")   ?? "0");
        private string CurrentUserLogin => User.FindFirstValue(ClaimTypes.Name)      ?? string.Empty;
private string CurrentUserRole => User.FindFirstValue("userRole") ?? "";
        private int    CurrentUserSite  => int.Parse(User.FindFirstValue("userSite") ?? "0");

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
            CurrentUserId,   // int
            CurrentUserRole, // string ("SuperAdmin", "Admin", ...)
            CurrentUserSite  // int
        );
        return Ok(list);
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = ex.Message });
    }
}

[HttpGet("records/{recordId:int}/stream")]
public async Task<IActionResult> StreamAudio(int recordId)
{
    Console.WriteLine($"🎵 StreamAudio appelé pour recordId={recordId}");

    var record = await _repo.GetRecordFilePathAsync(recordId);

    Console.WriteLine($"🎵 FilePath={record?.FilePath ?? "NULL"}");

    if (record == null || string.IsNullOrEmpty(record.FilePath))
        return NotFound(new { message = "Enregistrement introuvable." });

    Console.WriteLine($"🎵 File.Exists={System.IO.File.Exists(record.FilePath)}");

    if (!System.IO.File.Exists(record.FilePath))
        return NotFound(new { message = "Fichier audio introuvable sur le serveur." });

    var stream   = System.IO.File.OpenRead(record.FilePath);
    var mimeType = record.FilePath.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase)
                   ? "audio/mpeg"
                   : "audio/wav";

    Console.WriteLine($"🎵 Streaming fichier mimeType={mimeType}");

    return File(stream, mimeType, enableRangeProcessing: true);
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
[HttpGet("records/{recordId:int}/stream-screen")]
public async Task<IActionResult> StreamScreen(int recordId)
{
    var record = await _repo.GetRecordScreenPathAsync(recordId);

    if (record == null || string.IsNullOrEmpty(record.ScreenSource))
        return NotFound(new { message = "ScreenSource introuvable." });

    if (!System.IO.File.Exists(record.ScreenSource))
        return NotFound(new { message = $"Fichier introuvable: {record.ScreenSource}" });

    var ext = Path.GetExtension(record.ScreenSource).ToLowerInvariant();

    // MP4/WEBM → stream direct
    if (ext == ".mp4" || ext == ".webm")
    {
        var mime = ext == ".mp4" ? "video/mp4" : "video/webm";
        return File(System.IO.File.OpenRead(record.ScreenSource), mime, enableRangeProcessing: true);
    }

    // FLV → conversion ffmpeg → MP4 streamé directement
    var ffmpegPath = _configuration["FfmpegPath"] ?? "ffmpeg";

    var psi = new System.Diagnostics.ProcessStartInfo
    {
        FileName  = ffmpegPath,
        Arguments = $"-i \"{record.ScreenSource}\" " +
            $"-c:v libx264 -preset ultrafast -crf 28 " +
            $"-c:a aac -b:a 128k -ar 44100 -ac 2 " +  // ✅ forcer stéréo 44100Hz
            $"-movflags frag_keyframe+empty_moov+faststart " +
            $"-f mp4 pipe:1",
        RedirectStandardOutput = true,
        RedirectStandardError  = true,
        UseShellExecute        = false,
        CreateNoWindow         = true,
    };

    var process = System.Diagnostics.Process.Start(psi);
    if (process == null)
        return StatusCode(500, new { message = "Impossible de démarrer ffmpeg." });

    // ✅ Lire stderr en arrière-plan pour éviter le deadlock
    _ = process.StandardError.ReadToEndAsync();

    Response.OnCompleted(() =>
    {
        try { if (!process.HasExited) process.Kill(); } catch { }
        process.Dispose();
        return Task.CompletedTask;
    });

    // ✅ Stream direct sans attendre la fin de la conversion
    return File(process.StandardOutput.BaseStream, "video/mp4");
}

    }
}