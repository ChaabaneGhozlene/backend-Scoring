using scoring_Backend.DTO.Statistique;
namespace scoring_Backend.Repositories.Interfaces.Statistique
{
    public interface ISectionStatRepository
    {
        Task<SectionStatResponseDto> GetSectionStatsAsync(
            SectionStatFilterDto filter,
            int? userId   = null,
            int? userRole = null,
            int? siteId   = null);
    }
}