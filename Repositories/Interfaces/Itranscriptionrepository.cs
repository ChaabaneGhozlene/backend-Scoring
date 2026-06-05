// =============================================================
//  Repositories/Interfaces/Recordings/ITranscriptionRepository.cs
// =============================================================

using scoring_Backend.DTO.Transcription;

namespace scoring_Backend.Repositories.Interfaces;

public interface ITranscriptionRepository
{
    Task<string> StartTranscriptionAsync(string audioUrl, CancellationToken ct = default);
    Task<TranscriptionJobResponse> GetJobStatusAsync(string jobId, CancellationToken ct = default);
    Task<List<TranscriptionJobResponse>> GetAllJobsAsync(CancellationToken ct = default);
    
}
