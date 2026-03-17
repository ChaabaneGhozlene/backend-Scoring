using scoring_Backend.Models.Admin;
namespace scoring_Backend.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<User?> GetUserByLogin(string login);
    Task SyncUserByRole(User user);        // SP_SYNC_Update_ADMIN/AGENT/SALESMAN
    Task SyncAddUser(string login);        // SP_SYNC_ADD_ADMIN/AGENT/SALESMAN
    Task<bool> UserHasAccessToApp(int userId, string codeApp);
    Task UpdateUserPassword(User user);
}