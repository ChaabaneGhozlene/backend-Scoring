using Microsoft.EntityFrameworkCore;
using scoring_Backend.Repositories.Interfaces;
using scoring_Backend.Models.Admin;

namespace scoring_Backend.Repositories.Implementations;

public class AuthRepository : IAuthRepository
{
    private readonly SqrAdminContext _context;

    public AuthRepository(SqrAdminContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByLogin(string login)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Login == login);
    }

    // Synchronisation selon le rôle (équivalent SP_SYNC_Update_*)
    public async Task SyncUserByRole(User user)
    {
        if (user.Role == 1 || user.Role == 2)
            await _context.Database.ExecuteSqlRawAsync("EXEC SP_SYNC_Update_ADMIN {0}", user.Oid);
        else if (user.Role == 3 || user.Role == 4)
            await _context.Database.ExecuteSqlRawAsync("EXEC SP_SYNC_Update_AGENT {0}", user.Oid);
        else
            await _context.Database.ExecuteSqlRawAsync("EXEC SP_SYNC_Update_SALESMAN {0}", user.Oid);
    }

    // Ajout utilisateur si inexistant (équivalent SP_SYNC_ADD_*)
    public async Task SyncAddUser(string login)
    {
        await _context.Database.ExecuteSqlRawAsync("EXEC SP_SYNC_ADD_ADMIN {0}", login);
        await _context.Database.ExecuteSqlRawAsync("EXEC SP_SYNC_ADD_AGENT {0}", login);
        await _context.Database.ExecuteSqlRawAsync("EXEC SP_SYNC_ADD_SALESMAN {0}", login);
    }

    // Vérification autorisation application (équivalent autorisation())
    public async Task<bool> UserHasAccessToApp(int userId, string codeApp)
    {
        var application = await _context.Applications
            .FirstOrDefaultAsync(a => a.Code == codeApp || a.Name == codeApp);

        if (application == null)
            return false;

        var access = await _context.UsersApplications
            .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.ApplicationId == application.Id);

        return access != null;
    }
    public async Task UpdateUserPassword(User user)
{
    try
    {
        var existingUser = await _context.Users.FindAsync(user.Id);

        if (existingUser == null)
        {
            Console.WriteLine($"[UpdateUserPassword]  User introuvable: {user.Id}");
            return;
        }

        // Met à jour uniquement le mot de passe
        existingUser.Password = user.Password;

        await _context.SaveChangesAsync();
        Console.WriteLine($"[UpdateUserPassword]  Mot de passe mis à jour pour: {user.Login}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[UpdateUserPassword]  Erreur: {ex.Message}");
        throw;
    }
}
 
}