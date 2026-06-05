using System.Text.Json.Serialization;

namespace scoring_Backend.DTO
{
    public record AiEvaluateRequestDto(int RecordId);
public class AiEvalPayload
{
    [JsonPropertyName("record_id")]  public int    RecordId { get; set; }
    [JsonPropertyName("survey_id")]  public int    SurveyId { get; set; }
    [JsonPropertyName("audio_url")]  public string? AudioUrl { get; set; }  // ✅
    [JsonPropertyName("token")]      public string? Token    { get; set; }  // ✅
    [JsonPropertyName("criteria")]   public List<AiCriterion> Criteria { get; set; } = new();
}

public class AiCriterion
{
    [JsonPropertyName("survey_item_id")] public int    SurveyItemId { get; set; }
    [JsonPropertyName("item_id")]        public int    ItemId       { get; set; }
    [JsonPropertyName("question")]    public string Question     { get; set; } = "";
     [JsonPropertyName("description")]      public string Description     { get; set; } = "";
    [JsonPropertyName("section_name")]   public string SectionName  { get; set; } = "";
    [JsonPropertyName("min_value")]      public float  MinValue     { get; set; }
    [JsonPropertyName("max_value")]      public float  MaxValue     { get; set; }
    [JsonPropertyName("allow_na")]       public bool   AllowNA      { get; set; }
}


public class AiPythonResponse
{
    [JsonPropertyName("scores")]
    public List<AiScore> Scores { get; set; } = new();

    [JsonPropertyName("transcript")]
    public string? Transcript { get; set; }
}

public class AiScore
{
    [JsonPropertyName("survey_item_id")]
    public int SurveyItemId { get; set; }

    [JsonPropertyName("value")]
    public float Value { get; set; }

    [JsonPropertyName("justification")]
    public string? Justification { get; set; }

    [JsonPropertyName("method_used")]
    public string? MethodUsed { get; set; }
}
}