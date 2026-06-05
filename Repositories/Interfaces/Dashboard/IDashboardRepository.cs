using scoring_Backend.DTO.Dashboard;

namespace scoring_Backend.Repositories.Interfaces.Dashboard
{
    public interface IDashboardRepository
    {
        Task<DashboardFullDto> GetDashboardAsync(
            DashboardFilterDto filter, int userId, string userRole, int siteId);
    }
}