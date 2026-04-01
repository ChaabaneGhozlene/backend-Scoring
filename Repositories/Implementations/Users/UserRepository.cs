using Microsoft.EntityFrameworkCore;
using scoring_Backend.DTO;
using scoring_Backend.Models.Admin;
using scoring_Backend.Models.Scoring;
using scoring_Backend.Repositories.Interfaces.Evaluation;

namespace scoring_Backend.Repositories.Implementations.Evaluation
{
    public class UserRepository : IUserRepository
    {
        // ✔️ Deux DbContext (Admin + Scoring)
        private readonly SqrAdminContext _adminDb;
        private readonly SqrScoringContext _scoringDb;

        private const int USER_ROLE = 5;
        private const int USER_TYPE = 1;

        public UserRepository(SqrAdminContext adminDb, SqrScoringContext scoringDb)
        {
            _adminDb = adminDb;
            _scoringDb = scoringDb;
        }

        // ── Liste paginée ─────────────────────────────────────────────────────
        public async Task<PaginatedUsersDto> GetUsersAsync(int page, int pageSize)
        {
            var query = _adminDb.Users
                .Where(u => u.Role == USER_ROLE && u.Type == USER_TYPE)
                .OrderBy(u => u.Id);

            var total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserDto
                {
                    Id        = u.Id,
                    Login     = u.Login,
                    FirstName = u.FirstName ?? string.Empty,
                    LastName  = u.LastName ?? string.Empty,
                    IsActive  = u.IsActive == 1,
                    SiteName  = u.SiteName ?? string.Empty,
                    SiteId    = u.SiteId ?? 0,
                })
                .ToListAsync();

            return new PaginatedUsersDto
            {
                TotalCount = total,
                Page = page,
                PageSize = pageSize,
                Items = items
            };
        }

        // ── Détail ────────────────────────────────────────────────────────────
        public async Task<UserDto> GetByIdAsync(int id)
        {
            var u = await _adminDb.Users.FindAsync(id)
                ?? throw new KeyNotFoundException($"User {id} not found");

            return MapToDto(u);
        }

        // ── Création ──────────────────────────────────────────────────────────
        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            if (await LoginExistsAsync(dto.Login))
                throw new InvalidOperationException("Login already exists");

            var user = new User
            {
                Login = dto.Login,
                Password = dto.Password,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                IsActive = dto.IsActive ? 1 : 0,
                Role = USER_ROLE,
                Type = USER_TYPE,
                SiteId = dto.SiteId,
                SiteName = dto.SiteName,
                CreateDate = DateTime.Now
            };

            _adminDb.Users.Add(user);
            await _adminDb.SaveChangesAsync();

            // UsersApplication
            var userApp = new UsersApplication
            {
                UserId = user.Id,
                ApplicationId = 1,
                ProfilId = USER_ROLE
            };
            _adminDb.UsersApplications.Add(userApp);

            // UsersSite
            var userSite = new UsersSite
            {
                UserId = user.Id,
                SiteId = dto.SiteId
            };
            _adminDb.UsersSites.Add(userSite);

            await _adminDb.SaveChangesAsync();

            return MapToDto(user);
        }

        // ── Modification ──────────────────────────────────────────────────────
        public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _adminDb.Users.FindAsync(id)
                ?? throw new KeyNotFoundException($"User {id} not found");

            if (await LoginExistsAsync(dto.Login, id))
                throw new InvalidOperationException("Login already exists");

            user.Login = dto.Login;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.IsActive = dto.IsActive ? 1 : 0;
            user.SiteId = dto.SiteId;
            user.SiteName = dto.SiteName;
            user.UpdateDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.Password = dto.Password;

            var userSite = await _adminDb.UsersSites
                .FirstOrDefaultAsync(s => s.UserId == id);

            if (userSite != null)
                userSite.SiteId = dto.SiteId;

            await _adminDb.SaveChangesAsync();
            return MapToDto(user);
        }

        // ── Suppression ───────────────────────────────────────────────────────
        public async Task DeleteAsync(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                var user = await _adminDb.Users.FindAsync(id)
                    ?? throw new KeyNotFoundException($"User {id} not found");

                // ✔️ Utiliser ScoringDb pour Ls et ApActions
                bool hasFiches = await _scoringDb.Ls.AnyAsync(l => l.CreateBy == id);
                bool hasActions = await _scoringDb.ApActions.AnyAsync(a => a.UserId == id);

                if (hasFiches || hasActions)
                    throw new InvalidOperationException(
                        $"Cannot delete user {user.Login} — linked to existing records");

                var userApp = await _adminDb.UsersApplications
                    .FirstOrDefaultAsync(a => a.UserId == id && a.ApplicationId == 1);

                if (userApp != null)
                    _adminDb.UsersApplications.Remove(userApp);

                var userSite = await _adminDb.UsersSites
                    .FirstOrDefaultAsync(s => s.UserId == id);

                if (userSite != null)
                    _adminDb.UsersSites.Remove(userSite);

                _adminDb.Users.Remove(user);
            }

            await _adminDb.SaveChangesAsync();
        }

        // ── Login unique ──────────────────────────────────────────────────────
        public async Task<bool> LoginExistsAsync(string login, int? excludeId = null)
        {
            var query = _adminDb.Users.Where(u => u.Login == login);

            if (excludeId.HasValue)
                query = query.Where(u => u.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        // ── Sites ─────────────────────────────────────────────────────────────
        public async Task<List<SiteDto>> GetSitesAsync()
        {
            return await _adminDb.ListCustomers
                .Where(c => c.CustomerId != 0 && c.Type == 1)
                .Select(c => new SiteDto
                {
                    Id = c.CustomerId,
                    Name = c.Description ?? string.Empty
                })
                .ToListAsync();
        }

        // ── Helper ────────────────────────────────────────────────────────────
        private static UserDto MapToDto(User u) => new()
        {
            Id = u.Id,
            Login = u.Login,
            FirstName = u.FirstName ?? string.Empty,
            LastName = u.LastName ?? string.Empty,
            IsActive = u.IsActive == 1,
            SiteName = u.SiteName ?? string.Empty,
            SiteId = u.SiteId ?? 0
        };
    }
}