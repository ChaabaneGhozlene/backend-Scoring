// Repositories/Implementations/Statistique/UserDashboardRepository.cs
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using scoring_Backend.Models.Scoring;
using scoring_Backend.DTO.Statistique;
using scoring_Backend.Repositories.Interfaces.Statistique;

namespace scoring_Backend.Repositories.Implementations.Statistique;

public class UserDashboardRepository : IUserDashboardRepository
{
    private readonly SqrScoringContext _db;
    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public UserDashboardRepository(SqrScoringContext db) => _db = db;

    public async Task<UserDashboardConfigDto> GetByUserIdAsync(int userId)
    {
        var entity = await _db.UserDashboardConfigs
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (entity == null)
            return new UserDashboardConfigDto { userId = userId, widgets = new() };

        var widgets = JsonSerializer.Deserialize<List<WidgetInstanceDto>>(
            entity.ConfigJson, _jsonOpts) ?? new();

        return new UserDashboardConfigDto { userId = userId, widgets = widgets };
    }

    public async Task UpsertAsync(UserDashboardConfigDto dto)
    {
        var entity = await _db.UserDashboardConfigs
            .FirstOrDefaultAsync(c => c.UserId == dto.userId);

        var json = JsonSerializer.Serialize(dto.widgets, _jsonOpts);

        if (entity == null)
        {
            _db.UserDashboardConfigs.Add(new UserDashboardConfig
            {
                UserId     = dto.userId,
                ConfigJson = json,
                UpdatedAt  = DateTime.UtcNow
            });
        }
        else
        {
            entity.ConfigJson = json;
            entity.UpdatedAt  = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
    }
}