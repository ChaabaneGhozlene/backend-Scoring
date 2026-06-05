// ============================================================
// Controllers/Evaluation/EvaluationController.cs
// ============================================================

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using scoring_Backend.DTO;
using scoring_Backend.Models.Scoring;
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
        private readonly IHttpClientFactory               _httpClientFactory;  // ← ajouter
        private readonly SqrScoringContext                _scoringContext;     // ← ajouter
        // ── Constructeur corrigé avec toutes les dépendances ──
            public EvaluationController(
        IEvaluationRepository repo,
        ILogger<EvaluationController> logger,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        SqrScoringContext scoringContext)
    {
        _repo = repo;
        _logger = logger;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _scoringContext = scoringContext;
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
    // ── 1. Récupérer le chemin ────────────────────────────
    var record = await _repo.GetRecordScreenPathAsync(recordId);

    Console.WriteLine($"🎬 StreamScreen → recordId={recordId}");
    Console.WriteLine($"🎬 ScreenSource={record?.ScreenSource ?? "NULL"}");

    if (record == null || string.IsNullOrEmpty(record.ScreenSource))
        return NotFound(new { message = "ScreenSource introuvable en base." });

    if (!System.IO.File.Exists(record.ScreenSource))
        return NotFound(new { message = $"Fichier physique introuvable : {record.ScreenSource}" });

    var ext = Path.GetExtension(record.ScreenSource).ToLowerInvariant();
    Console.WriteLine($"🎬 Extension={ext}");

    // ── 2. MP4 / WEBM → stream direct ────────────────────
    if (ext == ".mp4" || ext == ".webm")
    {
        var mime = ext == ".mp4" ? "video/mp4" : "video/webm";
        return File(System.IO.File.OpenRead(record.ScreenSource),
                    mime, enableRangeProcessing: true);
    }

    // ── 3. FLV (ou autre) → conversion ffmpeg ─────────────
    var ffmpegPath = _configuration["FfmpegPath"] ?? "ffmpeg";

    // Vérifier que ffmpeg existe
    if (!System.IO.File.Exists(ffmpegPath) && ffmpegPath != "ffmpeg")
    {
        Console.WriteLine($"❌ ffmpeg introuvable à : {ffmpegPath}");
        return StatusCode(500, new { message = $"ffmpeg introuvable : {ffmpegPath}" });
    }

    Console.WriteLine($"🎬 Lancement ffmpeg : {ffmpegPath}");
    Console.WriteLine($"🎬 Fichier source   : {record.ScreenSource}");

    var psi = new System.Diagnostics.ProcessStartInfo
    {
        FileName               = ffmpegPath,
        Arguments              = $"-i \"{record.ScreenSource}\" " +
                                 $"-c:v libx264 -preset ultrafast -crf 28 " +
                                 $"-c:a aac -b:a 128k -ar 44100 -ac 2 " +
                                 $"-movflags frag_keyframe+empty_moov+faststart " +
                                 $"-f mp4 pipe:1",
        RedirectStandardOutput = true,
        RedirectStandardError  = true,
        UseShellExecute        = false,
        CreateNoWindow         = true,
    };

    System.Diagnostics.Process? process = null;

    try
    {
        process = System.Diagnostics.Process.Start(psi);

        if (process == null)
        {
            Console.WriteLine("❌ Process.Start a retourné null");
            return StatusCode(500, new { message = "Impossible de démarrer ffmpeg." });
        }

        // ── Lire stderr en tâche de fond (évite deadlock) ─
        var stderrTask = process.StandardError.ReadToEndAsync();

        // ── Attendre brièvement que ffmpeg démarre ────────
        await Task.Delay(300);

        if (process.HasExited)
        {
            var stderr = await stderrTask;
            Console.WriteLine($"❌ ffmpeg s'est arrêté immédiatement. Stderr:\n{stderr}");
            return StatusCode(500, new
            {
                message = "ffmpeg a échoué au démarrage.",
                detail  = stderr
            });
        }

        Console.WriteLine("✅ ffmpeg démarré, début du streaming...");

        // ── Nettoyage après envoi ─────────────────────────
        Response.OnCompleted(() =>
        {
            try
            {
                if (!process.HasExited) process.Kill();
            }
            catch { }
            process.Dispose();
            return Task.CompletedTask;
        });

        return File(process.StandardOutput.BaseStream, "video/mp4");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Exception StreamScreen : {ex.Message}");
        Console.WriteLine($"❌ StackTrace : {ex.StackTrace}");

        try { if (process != null && !process.HasExited) process.Kill(); } catch { }
        process?.Dispose();

        return StatusCode(500, new
        {
            message    = ex.Message,
            inner      = ex.InnerException?.Message,
            stackTrace = ex.StackTrace
        });
    }
}
// ── DTOs internes au controller ──────────────────────────────────
private record TranscribeStartResp(string? job_id);
private record TranscribeStatusResp(
    string?  status,
    string?  transcript,
    string?  detected_lang, 
    string?  error);

// ── Helpers ──────────────────────────────────────────────────────
private async Task SaveTranscriptToDbAsync(int recordId, string transcript, string? detectedLang = null)
{
    var conn    = _scoringContext.Database.GetDbConnection();
    var wasOpen = conn.State == System.Data.ConnectionState.Open;
    try
    {
        if (!wasOpen) await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE RecordData SET Transcript = @t , DetectedLang = @lang WHERE ID = @id";

        var pt = cmd.CreateParameter(); pt.ParameterName = "@t";  pt.Value = transcript;  cmd.Parameters.Add(pt);
        var pl = cmd.CreateParameter(); pl.ParameterName = "@lang"; pl.Value = detectedLang; cmd.Parameters.Add(pl);
        var pi = cmd.CreateParameter(); pi.ParameterName = "@id"; pi.Value = recordId;    cmd.Parameters.Add(pi);

        await cmd.ExecuteNonQueryAsync();
    }
    finally { if (!wasOpen) await conn.CloseAsync(); }
}

// ── Endpoint principal ────────────────────────────────────────────
[HttpPost("ai-evaluate")]
public async Task<IActionResult> AiEvaluate(
    [FromBody] AiEvaluateRequestDto dto)
{
    // ─────────────────────────────────────────────────────────────
    // 0. Validation
    // ─────────────────────────────────────────────────────────────
    if (dto.RecordId <= 0)
        return BadRequest("RecordId invalide.");

    var pythonUrl = _configuration["PythonAiService:BaseUrl"]
                    ?? "http://localhost:8000";

    var http = _httpClientFactory.CreateClient();

    // timeout global
    http.Timeout = TimeSpan.FromMinutes(15);

    // ─────────────────────────────────────────────────────────────
    // 1. Ouvrir la grille d’évaluation
    // ─────────────────────────────────────────────────────────────
    var evalResult = await _repo.OpenEvaluationAsync(
        dto.RecordId,
        CurrentUserId,
        CurrentUserLogin);

    if (evalResult.SurveyId == 0)
    {
        return BadRequest(new
        {
            message = evalResult.ErrorMessage
        });
    }

    // ─────────────────────────────────────────────────────────────
    // 2. Récupérer transcript / audio
    // ─────────────────────────────────────────────────────────────
    var record = await _repo.GetRecordTranscriptAsync(dto.RecordId);

    if (record == null)
    {
        return NotFound(new
        {
            message = "Enregistrement introuvable."
        });
    }

    string? transcript = record.Transcript;

    // langue détectée
    string detectedLang = record.DetectedLang ?? "";

    // juste pour logs/debug
    bool transcriptGenerated = false;

    // ─────────────────────────────────────────────────────────────
    // 3. Si transcript absent → transcription Whisper
    // ─────────────────────────────────────────────────────────────
    if (string.IsNullOrWhiteSpace(transcript))
    {
        _logger.LogInformation(
            "🎙️ Pas de transcript en base → transcription recordId={Id}",
            dto.RecordId);

        // vérifier fichier audio
        if (string.IsNullOrWhiteSpace(record.FilePath)
            || !System.IO.File.Exists(record.FilePath))
        {
            return NotFound(new
            {
                message = $"Fichier audio introuvable : {record.FilePath}"
            });
        }

        // ─────────────────────────────────────────────────────────
        // 3a. Démarrer transcription Python
        // ─────────────────────────────────────────────────────────
        HttpResponseMessage startRes;

        try
        {
            startRes = await http.PostAsJsonAsync(
                $"{pythonUrl}/transcribe",
                new
                {
                    audio_url = record.FilePath,
                    token = (string?)null
                });
        }
        catch (Exception ex)
        {
            return StatusCode(502, new
            {
                message = $"Service Python injoignable : {ex.Message}"
            });
        }

        // erreur HTTP
        if (!startRes.IsSuccessStatusCode)
        {
            var body = await startRes.Content.ReadAsStringAsync();

            _logger.LogError(
                "❌ Erreur démarrage transcription : {Body}",
                body);

            return StatusCode(502, new
            {
                message = "Erreur démarrage transcription",
                detail = body
            });
        }

        // ─────────────────────────────────────────────────────────
        // 3b. Lire job_id
        // ─────────────────────────────────────────────────────────
        var startData =
            await startRes.Content.ReadFromJsonAsync<TranscribeStartResp>(
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        if (string.IsNullOrWhiteSpace(startData?.job_id))
        {
            return StatusCode(500, new
            {
                message = "job_id manquant dans la réponse Python."
            });
        }

        var jobId = startData.job_id;

        _logger.LogInformation(
            "✅ Transcription démarrée jobId={JobId}",
            jobId);

        // ─────────────────────────────────────────────────────────
        // 3c. Polling transcription
        // ─────────────────────────────────────────────────────────
        const int maxAttempts = 72; // 72 × 5s = 6 min
        const int delayMs = 5000;

        TranscribeStatusResp? jobResult = null;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            await Task.Delay(delayMs);

            try
            {
                var statusRes =
                    await http.GetAsync(
                        $"{pythonUrl}/transcribe/{jobId}");

                if (!statusRes.IsSuccessStatusCode)
                    continue;

                jobResult =
                    await statusRes.Content
                        .ReadFromJsonAsync<TranscribeStatusResp>(
                            new System.Text.Json.JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    "⚠️ Polling transcription erreur : {Msg}",
                    ex.Message);

                continue;
            }

            // ─────────────────────────────────────────────────────
            // transcription terminée
            // ─────────────────────────────────────────────────────
            if (jobResult?.status == "done")
            {
                transcript = jobResult.transcript?.Trim();

                if (string.IsNullOrWhiteSpace(transcript))
                {
                    return StatusCode(500, new
                    {
                        message =
                            "Transcript vide retourné par Python."
                    });
                }

                detectedLang = jobResult.detected_lang ?? "";

                // mettre à jour objet local
                record.Transcript = transcript;
                record.DetectedLang = detectedLang;

                // sauvegarder SQL
                await SaveTranscriptToDbAsync(
                    dto.RecordId,
                    transcript,
                    detectedLang);

                transcriptGenerated = true;

                _logger.LogInformation(
                    "✅ Transcript généré et sauvegardé recordId={Id}",
                    dto.RecordId);

                break;
            }

            // ─────────────────────────────────────────────────────
            // erreur transcription
            // ─────────────────────────────────────────────────────
            if (jobResult?.status == "error")
            {
                return StatusCode(500, new
                {
                    message =
                        $"Transcription échouée : {jobResult.error}"
                });
            }
        }

        // timeout
        if (string.IsNullOrWhiteSpace(transcript))
        {
            return StatusCode(504, new
            {
                message =
                    "Timeout : transcription trop longue (> 6 min)."
            });
        }
    }
    else
    {
        _logger.LogInformation(
            "📦 Transcript récupéré depuis la base recordId={Id}",
            dto.RecordId);
    }

    // ─────────────────────────────────────────────────────────────
    // 4. Vérification transcript avant IA
    // ─────────────────────────────────────────────────────────────
    if (string.IsNullOrWhiteSpace(transcript))
    {
        return StatusCode(500, new
        {
            message =
                "Transcript introuvable avant évaluation IA."
        });
    }

    _logger.LogInformation(
        transcriptGenerated
            ? "🎙️ Evaluation après transcription"
            : "📦 Evaluation avec transcript existant");

    _logger.LogInformation(
        "🧠 Transcript length = {Len}",
        transcript.Length);

    // ─────────────────────────────────────────────────────────────
    // 5. Construire payload IA
    // ─────────────────────────────────────────────────────────────
    var audioRecord = await _repo.GetRecordFilePathAsync(dto.RecordId);

var evalPayload = new
{
    record_id     = dto.RecordId,
    transcript    = transcript,
    detected_lang = detectedLang,
    audio_url     = audioRecord?.FilePath,   // ← chemin local direct, pas HTTP
    criteria      = evalResult.GridRows.Select(r => new
    {
        survey_item_id = r.TemplateItemId,
        question       = r.Question   ?? "",
        description    = r.Description ?? "",
        section_name   = r.GroupName  ?? "",
        min_value      = r.ScaleMin,
        max_value      = r.ScaleMax,
    }).ToList(),
};
    // log payload
    var payloadJson =
        System.Text.Json.JsonSerializer.Serialize(evalPayload);

    _logger.LogInformation(
        "📤 Payload IA : {Json}",
        payloadJson[..Math.Min(500, payloadJson.Length)]);

    // ─────────────────────────────────────────────────────────────
    // 6. Appel Python evaluate-with-transcript
    // ─────────────────────────────────────────────────────────────
    HttpResponseMessage aiRes;

    try
    {
        aiRes = await http.PostAsJsonAsync(
            $"{pythonUrl}/evaluate-with-transcript",
            evalPayload);
    }
    catch (TaskCanceledException)
    {
        return StatusCode(504, new
        {
            message =
                "Timeout : évaluation IA trop longue."
        });
    }
    catch (Exception ex)
    {
        return StatusCode(502, new
        {
            message =
                $"Service Python injoignable : {ex.Message}"
        });
    }

    // erreur HTTP
    if (!aiRes.IsSuccessStatusCode)
    {
        var errBody = await aiRes.Content.ReadAsStringAsync();

        _logger.LogError(
            "❌ /evaluate-with-transcript HTTP {Code} : {Body}",
            aiRes.StatusCode,
            errBody);

        return StatusCode(502, new
        {
            message = "Erreur service IA",
            detail = errBody
        });
    }

    // ─────────────────────────────────────────────────────────────
    // 7. Désérialisation JSON
    // ─────────────────────────────────────────────────────────────
    var jsonOpts = new System.Text.Json.JsonSerializerOptions
    {
        PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.SnakeCaseLower,

        PropertyNameCaseInsensitive = true,
    };

    var aiResponse =
        await aiRes.Content.ReadFromJsonAsync<AiPythonResponse>(
            jsonOpts);

    if (aiResponse == null)
    {
        return StatusCode(500, new
        {
            message = "Réponse IA vide."
        });
    }

    // ─────────────────────────────────────────────────────────────
    // 8. Vérification scores
    // ─────────────────────────────────────────────────────────────
    if (aiResponse.Scores == null
        || aiResponse.Scores.Count == 0)
    {
        _logger.LogError(
            "❌ aiResponse.Scores vide");

        return StatusCode(500, new
        {
            message =
                "Réponse IA vide ou mal désérialisée."
        });
    }

    _logger.LogInformation(
        "✅ {Count} scores reçus de Python",
        aiResponse.Scores.Count);

    // ─────────────────────────────────────────────────────────────
    // 9. Construire SaveEvaluationDto
    // ─────────────────────────────────────────────────────────────
    var saveDto = new SaveEvaluationDto
    {
        SurveyId = evalResult.SurveyId,

        IsAiEval = true,

        Items = aiResponse.Scores.Select(s =>
        {
            _logger.LogDebug(
                "📝 Score reçu templateItemId={Id} value={Val}",
                s.SurveyItemId,
                s.Value);

            return new SurveyItemValueDto
            {
                ItemId = s.SurveyItemId,

                Value = s.Value,

                Memo = s.Justification ?? ""
            };
        }).ToList()
    };

    // ─────────────────────────────────────────────────────────────
    // 10. Sauvegarder scores SQL
    // ─────────────────────────────────────────────────────────────
    var saveResult =
        await _repo.SaveEvaluationAsync(
            saveDto,
            CurrentUserId,
            CurrentUserLogin);

    if (!saveResult.Success)
    {
        return BadRequest(new
        {
            message = saveResult.Message
        });
    }

    _logger.LogInformation(
        "✅ Évaluation IA sauvegardée surveyId={Id} score={Score}%",
        evalResult.SurveyId,
        saveResult.Score);

    // ─────────────────────────────────────────────────────────────
    // 11. Retour frontend
    // ─────────────────────────────────────────────────────────────
    return Ok(new
    {
        surveyId = evalResult.SurveyId,

        score = saveResult.Score,

        items = aiResponse.Scores,

        transcript = transcript,

        transcriptGenerated = transcriptGenerated,

        message =
            "Évaluation IA sauvegardée avec succès."
    });
}
    }
}