// Repositories/Interfaces/Statistique/IUserDashboardRepository.cs
using scoring_Backend.DTO.Statistique;

namespace scoring_Backend.Repositories.Interfaces.Statistique;

public interface IUserDashboardRepository
{
    Task<UserDashboardConfigDto> GetByUserIdAsync(int userId);
    Task UpsertAsync(UserDashboardConfigDto dto);
}