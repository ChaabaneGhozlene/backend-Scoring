using Microsoft.EntityFrameworkCore;
using scoring_Backend.DTO;
using scoring_Backend.Models.Scoring;
using scoring_Backend.Repositories.Interfaces.Evaluation;

namespace scoring_Backend.Repositories.Implementations.Evaluation
{
    public class ViewConfigRepository : IViewConfigRepository
    {
        private readonly SqrScoringContext _db;

        public ViewConfigRepository(SqrScoringContext db) => _db = db;

        public async Task<List<ViewConfigDto>> GetUserConfigsAsync(string userLogin, int groupe)
{
    return await _db.Configs
        .Where(c => c.UserLogin == userLogin && c.Groupe == groupe)  // ← dynamique
        .OrderBy(c => c.NomConfig)
        .Select(c => new ViewConfigDto
        {
            Id         = c.Id,
            Name       = c.NomConfig ?? string.Empty,
            LayoutJson = c.Layout    ?? string.Empty,
            Groupe     = c.Groupe,   // ← retourner le groupe dans le DTO
        })
        .ToListAsync();
}

public async Task<ViewConfigDto> CreateConfigAsync(string userLogin, CreateViewConfigDto dto)
{
    bool exists = await _db.Configs
        .AnyAsync(c => c.UserLogin == userLogin
                    && c.NomConfig  == dto.Name
                    && c.Groupe     == dto.Groupe);  // ← dynamique

    if (exists)
        throw new InvalidOperationException($"Une configuration nommée '{dto.Name}' existe déjà.");

    var config = new Config
    {
        UserLogin = userLogin,
        NomConfig = dto.Name,
        Layout    = dto.LayoutJson,
        Groupe    = dto.Groupe,   // ← depuis le DTO
    };

    _db.Configs.Add(config);
    await _db.SaveChangesAsync();

    return new ViewConfigDto
    {
        Id         = config.Id,
        Name       = config.NomConfig ?? string.Empty,
        LayoutJson = config.Layout    ?? string.Empty,
        Groupe     = config.Groupe,
    };
}
        public async Task<ViewConfigDto> UpdateConfigAsync(int configId, string userLogin, string layoutJson)
        {
            var config = await _db.Configs
                .FirstOrDefaultAsync(c => c.Id == configId && c.UserLogin == userLogin)
                ?? throw new KeyNotFoundException($"Configuration {configId} introuvable.");

            config.Layout = layoutJson;
            await _db.SaveChangesAsync();

            return new ViewConfigDto
            {
                Id         = config.Id,
                Name       = config.NomConfig ?? string.Empty,
                LayoutJson = config.Layout    ?? string.Empty
            };
        }

        public async Task DeleteConfigAsync(int configId, string userLogin)
        {
            var config = await _db.Configs
                .FirstOrDefaultAsync(c => c.Id == configId && c.UserLogin == userLogin)
                ?? throw new KeyNotFoundException($"Configuration {configId} introuvable.");

            _db.Configs.Remove(config);
            await _db.SaveChangesAsync();
        }
    }
}