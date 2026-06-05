// =============================================================
//  DTO/Transcription/TranscriptionModels.cs
//
//  CORRECTION : StartTranscriptionRequest utilise RecordId
//  au lieu de AudioUrl — le controller résout lui-même le
//  filePath via SQL (même logique que StreamAudio).
// =============================================================

using System.Text.Json.Serialization;

namespace scoring_Backend.DTO.Transcription;

// ── Requête envoyée à Python POST /transcribe ─────────────────
public class TranscribeRequest
{
    [JsonPropertyName("audio_url")]
    public string AudioUrl { get; set; } = "";

    [JsonPropertyName("token")]
    public string? Token { get; set; }   // null pour chemin local
}

// ── Réponse de Python POST /transcribe (202) ──────────────────
public class TranscribeStartResponse
{
    [JsonPropertyName("job_id")]
    public string JobId { get; set; } = "";

    [JsonPropertyName("status")]
    public string Status { get; set; } = "";

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}

// ── Réponse de Python GET /transcribe/{job_id} ────────────────
public class TranscribeJobResult
{
    [JsonPropertyName("job_id")]
    public string JobId { get; set; } = "";
        public int     RecordId        { get; set; }      // ← ajouter


    [JsonPropertyName("status")]
    public string Status { get; set; } = "";

    [JsonPropertyName("audio_url")]
    public string? AudioUrl { get; set; }

    [JsonPropertyName("pipeline_used")]
    public string? PipelineUsed { get; set; }

    [JsonPropertyName("detected_lang")]
    public string? DetectedLang { get; set; }

    [JsonPropertyName("mapping")]
    public Dictionary<string, string>? Mapping { get; set; }

    [JsonPropertyName("segments")]
    public List<TranscriptSegment>? Segments { get; set; }

    [JsonPropertyName("transcript")]
    public string? Transcript { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("duration_seconds")]
    public double? DurationSeconds { get; set; }

    [JsonPropertyName("created_at")]
    public double? CreatedAt { get; set; }

    [JsonPropertyName("finished_at")]
    public double? FinishedAt { get; set; }

    [JsonIgnore]
    public bool IsSuccess => Status == "done";

    [JsonIgnore]
    public bool IsFailed => Status == "error";
}

// ── Segment individuel ────────────────────────────────────────
public class TranscriptSegment
{
    [JsonPropertyName("start")]
    public double Start { get; set; }

    [JsonPropertyName("end")]
    public double End { get; set; }

    [JsonPropertyName("speaker")]
    public string Speaker { get; set; } = "";

    [JsonPropertyName("text")]
    public string Text { get; set; } = "";
}

// ── Objet stocké en mémoire .NET + retourné au frontend ───────
public class TranscriptionJobResponse
{
    public string  JobId           { get; set; } = "";
    public string  Status          { get; set; } = "pending";
    public string? PipelineUsed    { get; set; }
    public string? DetectedLang    { get; set; }
    public string? Transcript      { get; set; }
    public string? Error           { get; set; }
    public double? DurationSeconds { get; set; }
    public Dictionary<string, string>?  Mapping  { get; set; }
    public List<TranscriptSegment>?     Segments { get; set; }

    [JsonIgnore]
    public bool IsSuccess => Status == "done";

    [JsonIgnore]
    public bool IsFailed  => Status == "error";

    [JsonIgnore]
    public string? Message { get; set; }
}

// ── Body reçu par POST /api/transcription/start ───────────────
// ✅ CORRECTION : RecordId au lieu de AudioUrl
//    Le controller résout lui-même le filePath via SQL
public class StartTranscriptionRequest
{
    public int RecordId { get; set; }
}
public class SaveTranscriptToRecordRequest
{
    public int    RecordId   { get; set; }
    public string Transcript { get; set; } = "";
    public string? DetectedLang { get; set; } 
}
public class RecordTranscriptDto
{
    public int     Id         { get; set; }
    public string? FilePath   { get; set; }
    public string? Transcript { get; set; }
    public string? DetectedLang { get; set; }
}