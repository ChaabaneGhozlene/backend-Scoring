using System.Collections.Generic;
using System.Threading.Tasks;
using scoring_Backend.DTO;

namespace scoring_Backend.Repositories.Interfaces.Evaluation
{
    public interface IFilterRepository
    {
        Task<List<UserFilterDto>> GetUserFiltersAsync(string userLogin);
        Task<UserFilterDto>       CreateFilterAsync(string userLogin, CreateFilterDto dto);
        Task                      DeleteFilterAsync(int filterId, string userLogin);
    }
}