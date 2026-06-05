// =============================================================
//  Controllers/TranscriptionController.cs
//
//  CORRECTION : StartTranscriptionRequest reçoit un RecordId
//  Le controller résout le filePath via la même requête SQL
//  que StreamAudio → plus de problème de token / HTTP 404
// =============================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using scoring_Backend.DTO.Transcription;
using scoring_Backend.Models.Scoring;
using scoring_Backend.Repositories.Interfaces;

namespace scoring_Backend.Controllers;

[ApiController]
[Route("api/transcription")]
[Authorize]
public class TranscriptionController : ControllerBase
{
    private readonly ITranscriptionRepository         _repo;
    private readonly SqrScoringContext              _context;
    private readonly ILogger<TranscriptionController> _logger;

    public TranscriptionController(
        ITranscriptionRepository         repo,
        SqrScoringContext              context,
        ILogger<TranscriptionController> logger)
    {
        _repo    = repo;
        _context = context;
        _logger  = logger;
    }

    // ── POST /api/transcription/start ────────────────────────────
    /// <summary>
    /// Lance la transcription pour un enregistrement.
    /// Body : { "recordId": 10818711 }
    /// Le controller résout le filePath via SQL (comme StreamAudio).
    /// Retourne : { "jobId": "..." }
    /// </summary>
    [HttpPost("start")]
    public async Task<IActionResult> Start(
        [FromBody] StartTranscriptionRequest request,
        CancellationToken ct)
    {
        if (request.RecordId <= 0)
            return BadRequest(new { error = "recordId est requis." });

        try
        {
            // ✅ Même requête SQL que StreamAudio
            var record = await _context.RecordData
    .Where(r => r.Id == request.RecordId)
    .Select(r => new { FilePath = r.RecFilename  })
    .FirstOrDefaultAsync(ct);

            if (record == null || string.IsNullOrWhiteSpace(record.FilePath))
            {
                _logger.LogWarning("❌ Aucun FilePath trouvé pour recordId={RecordId}", request.RecordId);
                return NotFound(new { error = "Chemin audio introuvable pour cet enregistrement." });
            }

            if (!System.IO.File.Exists(record.FilePath))
            {
                _logger.LogWarning("❌ Fichier absent du disque : {FilePath}", record.FilePath);
                return NotFound(new { error = $"Fichier audio absent du disque : {record.FilePath}" });
            }

            _logger.LogInformation("✅ FilePath résolu : {FilePath}", record.FilePath);

            var jobId = await _repo.StartTranscriptionAsync(record.FilePath, ct);
            return Ok(new { jobId });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Erreur au démarrage de la transcription");
            return StatusCode(503, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur inattendue au démarrage de la transcription");
            return StatusCode(500, new { error = "Erreur interne. Vérifiez que le service Python est démarré." });
        }
    }

    // ── GET /api/transcription/status/{jobId} ────────────────────
    [HttpGet("status/{jobId}")]
    public async Task<IActionResult> Status(string jobId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(jobId))
            return BadRequest(new { error = "jobId est requis." });

        var result = await _repo.GetJobStatusAsync(jobId, ct);
        return Ok(result);
    }

    // ── GET /api/transcription/download/{jobId} ──────────────────
    [HttpGet("download/{jobId}")]
    public async Task<IActionResult> Download(string jobId, CancellationToken ct)
    {
        var result = await _repo.GetJobStatusAsync(jobId, ct);

        if (result.Status != "done" || string.IsNullOrWhiteSpace(result.Transcript))
            return NotFound(new { error = "Transcript non disponible pour ce job." });

        var bytes    = System.Text.Encoding.UTF8.GetBytes(result.Transcript);
        var fileName = $"transcript_{jobId}.txt";
        return File(bytes, "text/plain; charset=utf-8", fileName);
    }

    // ── GET /api/transcription/jobs ──────────────────────────────
    [HttpGet("jobs")]
    public async Task<IActionResult> ListJobs(CancellationToken ct)
    {
        var jobs = await _repo.GetAllJobsAsync(ct);
        return Ok(jobs);
    }
    // ── POST /api/transcription/save-to-record ───────────────────
// ── POST /api/transcription/save-to-record ───────────────────
[HttpPost("save-to-record")]
public async Task<IActionResult> SaveToRecord(
    [FromBody] SaveTranscriptToRecordRequest request,
    CancellationToken ct)
{
    if (request.RecordId <= 0)
        return BadRequest(new { error = "recordId est requis." });

    if (string.IsNullOrWhiteSpace(request.Transcript))
        return BadRequest(new { error = "transcript est vide." });

    try
    {
       

        var record = await _context.RecordData
    .FirstOrDefaultAsync(r => r.Id == request.RecordId, ct);

if (record == null)
    return NotFound(new { error = "Enregistrement introuvable." });

// SQL direct — contourne tout problème de tracking EF
var conn    = _context.Database.GetDbConnection();
var wasOpen = conn.State == System.Data.ConnectionState.Open;
try
{
    if (!wasOpen) await conn.OpenAsync(ct);
    using var cmd        = conn.CreateCommand();
    cmd.CommandText      = "UPDATE RecordData SET Transcript = @transcript, DetectedLang = @lang WHERE ID = @id";
    
    var pTranscript      = cmd.CreateParameter();
    pTranscript.ParameterName = "@transcript";
    pTranscript.Value         = request.Transcript;
    cmd.Parameters.Add(pTranscript);

    var pLang            = cmd.CreateParameter();
    pLang.ParameterName  = "@lang";
    pLang.Value          = request.DetectedLang ?? (object)DBNull.Value;
    cmd.Parameters.Add(pLang);

    var pId              = cmd.CreateParameter();
    pId.ParameterName    = "@id";
    pId.Value            = request.RecordId;
    cmd.Parameters.Add(pId);

    var rows = await cmd.ExecuteNonQueryAsync(ct);

    _logger.LogInformation("✅ Transcript sauvegardé recordId={RecordId} ({Chars} chars) — {Rows} row(s) affectée(s)",
        request.RecordId, request.Transcript.Length, rows);

    if (rows == 0)
        return NotFound(new { error = "UPDATE n'a affecté aucune ligne — recordId introuvable." });
}
finally
{
    if (!wasOpen) await conn.CloseAsync();
}

return Ok(new { message = "Transcript sauvegardé." });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erreur lors de la sauvegarde du transcript pour recordId={RecordId}", request.RecordId);
        return StatusCode(500, new { error = "Erreur interne lors de la sauvegarde du transcript." });
    }       
}
[HttpGet("record/{recordId:int}")]
public async Task<IActionResult> GetSavedTranscript(int recordId, CancellationToken ct)
{
    if (recordId <= 0)
        return BadRequest(new { error = "recordId invalide." });

    var record = await _context.RecordData
        .Where(r => r.Id == recordId)
        .Select(r => new { r.Transcript })
        .FirstOrDefaultAsync(ct);

    if (record == null)
        return NotFound(new { error = "Enregistrement introuvable." });

    if (string.IsNullOrWhiteSpace(record.Transcript))
        return NotFound(new { error = "Aucun transcript sauvegardé pour cet enregistrement." });

    return Ok(new { transcript = record.Transcript });
}
    
}