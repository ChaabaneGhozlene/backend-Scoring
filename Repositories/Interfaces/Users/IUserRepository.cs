using scoring_Backend.DTO;

namespace scoring_Backend.Repositories.Interfaces.Evaluation
{
    public interface IUserRepository
    {
        Task<PaginatedUsersDto> GetUsersAsync(int page, int pageSize);
        Task<UserDto>           GetByIdAsync(int id);
        Task<UserDto>           CreateAsync(CreateUserDto dto);
        Task<UserDto>           UpdateAsync(int id, UpdateUserDto dto);
        Task                    DeleteAsync(IEnumerable<int> ids);
        Task<bool>              LoginExistsAsync(string login, int? excludeId = null);
        Task<List<SiteDto>>     GetSitesAsync();
    }
}