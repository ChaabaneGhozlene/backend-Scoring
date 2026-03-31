using System.Threading.Tasks;
using scoring_Backend.DTO;

namespace scoring_Backend.Repositories.Interfaces.Evaluation
{
    public interface IRecordRepository
    {
        // ── Existants (déjà implémentés et fonctionnels) ───────────────────
        Task<PagedRecordsDto> GetRecordsByFilterAsync(
            RecordFilterDto filter, int userId, string userRole);

        Task<RecordDataDto?> GetByIdAsync(int recordId);


        Task<byte[]> ExportToCsvAsync(
            RecordFilterDto filter, int userId, string userRole);

        // ── Nouveaux (à implémenter dans RecordRepository) ─────────────────
        Task<string?> GetRecordSourceAsync(int recordId);
        Task SaveTraceAsync(TraceActionDto dto, int userId);
        Task<List<ListenHistoryDto>> GetListenHistoryAsync(int recordId);
Task<List<ListenHistoryDto>> GetScreenHistoryAsync(int recordId);
Task DeleteRecordAsync(int recordId);

    }

}