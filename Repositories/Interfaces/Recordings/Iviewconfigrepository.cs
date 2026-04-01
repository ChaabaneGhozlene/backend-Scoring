
using scoring_Backend.DTO;

namespace scoring_Backend.Repositories.Interfaces.Evaluation
{
    public interface IViewConfigRepository
    {
Task<List<ViewConfigDto>> GetUserConfigsAsync(string userLogin, int groupe);
        Task<ViewConfigDto>       CreateConfigAsync(string userLogin, CreateViewConfigDto dto);
        Task<ViewConfigDto>       UpdateConfigAsync(int configId, string userLogin, string layoutJson);
        Task                      DeleteConfigAsync(int configId, string userLogin);
    }
}