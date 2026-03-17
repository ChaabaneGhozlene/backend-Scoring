using Microsoft.EntityFrameworkCore;
using scoring_Backend.DTO;
using scoring_Backend.Models.Scoring;
using scoring_Backend.Repositories.Interfaces.Evaluation;

namespace scoring_Backend.Repositories.Implementations.Evaluation
{
    public class FilterRepository : IFilterRepository
    {
        private readonly SqrScoringContext _db;

        public FilterRepository(SqrScoringContext db) => _db = db;

        public async Task<List<UserFilterDto>> GetUserFiltersAsync(string userLogin)
        {
            return await _db.Filtres
                .Where(f => f.UserLogin == userLogin && f.Groupe == 1)
                .OrderBy(f => f.NomFiltre)
                .Select(f => new UserFilterDto
                {
                    Id         = f.Id,
                    Name       = f.NomFiltre  ?? string.Empty,
                    Expression = f.Expression,
                    SqlWhere   = f.SqlWhere,
                    Type       = f.Type,
                    Status     = f.Status
                })
                .ToListAsync();
        }

        public async Task<UserFilterDto> CreateFilterAsync(string userLogin, CreateFilterDto dto)
        {
            bool exists = await _db.Filtres
                .AnyAsync(f => f.UserLogin == userLogin
                            && f.NomFiltre  == dto.Name
                            && f.Groupe     == 1);

            if (exists)
                throw new InvalidOperationException($"Un filtre nommé '{dto.Name}' existe déjà.");

            var filtre = new Filtre
            {
                UserLogin    = userLogin,
                NomFiltre    = dto.Name,
                Expression   = dto.Expression,
                SqlWhere     = dto.SqlWhere,
                Type         = dto.Type,
                Status       = 1,
                Groupe       = 1,
                DateCreation = DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            _db.Filtres.Add(filtre);
            await _db.SaveChangesAsync();

            return new UserFilterDto
            {
                Id         = filtre.Id,
                Name       = filtre.NomFiltre  ?? string.Empty,
                Expression = filtre.Expression,
                SqlWhere   = filtre.SqlWhere,
                Type       = filtre.Type,
                Status     = filtre.Status
            };
        }

        public async Task DeleteFilterAsync(int filterId, string userLogin)
        {
            var filtre = await _db.Filtres
                .FirstOrDefaultAsync(f => f.Id == filterId && f.UserLogin == userLogin)
                ?? throw new KeyNotFoundException($"Filtre {filterId} introuvable ou accès refusé.");

            _db.Filtres.Remove(filtre);
            await _db.SaveChangesAsync();
        }
    }
}