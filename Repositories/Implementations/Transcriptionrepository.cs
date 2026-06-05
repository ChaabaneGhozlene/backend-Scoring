// =============================================================
//  Repositories/Implementations/TranscriptionRepository.cs
//
//  Inchangé dans la logique — reçoit déjà un filePath local
//  depuis le controller. Token supprimé (inutile pour fichier local).
// =============================================================

using System.Collections.Concurrent;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using scoring_Backend.DTO.Transcription;
using scoring_Backend.Models.Scoring;
using scoring_Backend.Repositories.Interfaces;

namespace scoring_Backend.Repositories.Implementations;

public class TranscriptionRepository : ITranscriptionRepository
{
    private readonly HttpClient                       _http;
    private readonly ILogger<TranscriptionRepository> _logger;

    private static readonly ConcurrentDictionary<string, TranscriptionJobResponse> _store = new();

    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy        = null,
        DefaultIgnoreCondition      = JsonIgnoreCondition.WhenWritingNull,
    };

    public TranscriptionRepository(
        HttpClient                       http,
        ILogger<TranscriptionRepository> logger)
    {
        _http   = http;
        _logger = logger;
    }

    // ── 1. Lancer la transcription ────────────────────────────────
    public async Task<string> StartTranscriptionAsync(
        string audioUrl,
        CancellationToken ct = default)
    {
        _logger.LogInformation("▶ Lancement transcription : {Url}", audioUrl);

        HttpResponseMessage response;
        try
        {
            // ✅ audioUrl est un filePath local — Token = null
            //    Python lit le fichier via os.path.exists() sans HTTP
            var payload = new TranscribeRequest
            {
                AudioUrl = audioUrl,
                Token    = null,
            };
            response = await _http.PostAsJsonAsync("/transcribe", payload, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Impossible de joindre le service Python");
            throw new InvalidOperationException($"Service Python injoignable : {ex.Message}", ex);
        }

        var rawBody = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("❌ Python HTTP {Code} : {Body}", response.StatusCode, rawBody);
            throw new InvalidOperationException($"Python a répondu {(int)response.StatusCode} : {rawBody}");
        }

        _logger.LogDebug("✅ Python /transcribe body : {Body}", rawBody);

        TranscribeStartResponse? result;
        try
        {
            result = JsonSerializer.Deserialize<TranscribeStartResponse>(rawBody, _jsonOpts);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "❌ JSON invalide de Python (start). Body reçu : {Body}", rawBody);
            throw new InvalidOperationException(
                $"Réponse JSON invalide du service Python (start). Body : {rawBody}", ex);
        }

        if (result is null || string.IsNullOrWhiteSpace(result.JobId))
        {
            _logger.LogError("❌ job_id manquant. Body reçu : {Body}", rawBody);
            throw new InvalidOperationException($"Python n'a pas retourné de job_id. Body : {rawBody}");
        }

        _store[result.JobId] = new TranscriptionJobResponse
        {
            JobId   = result.JobId,
            Status  = "pending",
            Message = "Job en attente de traitement.",
        };

        _logger.LogInformation("✅ Job créé : {JobId}", result.JobId);
        return result.JobId;
    }

    // ── 2. Interroger le statut ───────────────────────────────────
    public async Task<TranscriptionJobResponse> GetJobStatusAsync(
        string jobId,
        CancellationToken ct = default)
    {
        if (_store.TryGetValue(jobId, out var cached) && cached.Status == "done")
        {
            _logger.LogInformation("📦 Job {JobId} retourné depuis le cache.", jobId);
            return cached;
        }

        HttpResponseMessage response;
        try
        {
            response = await _http.GetAsync($"/transcribe/{jobId}", ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Impossible de joindre Python pour le job {JobId}", jobId);
            return new TranscriptionJobResponse
            {
                JobId  = jobId,
                Status = "error",
                Error  = $"Service Python injoignable : {ex.Message}",
            };
        }

        var rawBody = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("❌ Python HTTP {Code} pour {JobId} : {Body}",
                response.StatusCode, jobId, rawBody);
            return new TranscriptionJobResponse
            {
                JobId  = jobId,
                Status = "error",
                Error  = $"Python HTTP {(int)response.StatusCode} : {rawBody}",
            };
        }

        _logger.LogDebug("Python /transcribe/{JobId} body : {Body}", jobId, rawBody);

        TranscribeJobResult? result;
        try
        {
            result = JsonSerializer.Deserialize<TranscribeJobResult>(rawBody, _jsonOpts);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex,
                "❌ JSON invalide de Python (status) pour {JobId}. Body reçu : {Body}",
                jobId, rawBody);
            return new TranscriptionJobResponse
            {
                JobId  = jobId,
                Status = "error",
                Error  = $"Réponse JSON invalide du service Python. Body reçu : {rawBody[..Math.Min(200, rawBody.Length)]}",
            };
        }

        if (result is null)
        {
            return new TranscriptionJobResponse
            {
                JobId  = jobId,
                Status = "error",
                Error  = "Réponse vide du service Python.",
            };
        }

        var jobResponse = new TranscriptionJobResponse
        {
            JobId           = result.JobId,
            Status          = result.Status,
            PipelineUsed    = result.PipelineUsed,
            DetectedLang    = result.DetectedLang,
            Transcript      = result.Transcript,
            Mapping         = result.Mapping,
            Segments        = result.Segments,
            DurationSeconds = result.DurationSeconds,
            Error           = result.Error,
        };

        _store[jobId] = jobResponse;

        if (result.IsSuccess)
            _logger.LogInformation("✅ Job {JobId} terminé en {Dur:F1}s.",
                jobId, result.DurationSeconds);
        else if (result.IsFailed)
            _logger.LogError("❌ Job {JobId} échoué : {Error}", jobId, result.Error);

        return jobResponse;
    }

    // ── 3. Liste tous les jobs ────────────────────────────────────
    public Task<List<TranscriptionJobResponse>> GetAllJobsAsync(
        CancellationToken ct = default)
        => Task.FromResult(_store.Values.ToList());

    // Modifier le constructeur pour injecter le context
private readonly SqrScoringContext _context;



}