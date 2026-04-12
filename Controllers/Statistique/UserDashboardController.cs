// Controllers/Statistique/UserDashboardController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO.Statistique;
using scoring_Backend.Repositories.Interfaces.Statistique;
using System.Security.Claims;

namespace scoring_Backend.Controllers.Statistique;

[Authorize]
[ApiController]
[Route("api/dashboard")]
public class UserDashboardController : ControllerBase
{
    private readonly IUserDashboardRepository _repo;

    public UserDashboardController(IUserDashboardRepository repo) => _repo = repo;

    private int GetUserId() =>
        int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    [HttpGet("config")]
    public async Task<IActionResult> GetConfig()
    {
        try
        {
            var config = await _repo.GetByUserIdAsync(GetUserId());
            return Ok(config);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("config")]
    public async Task<IActionResult> SaveConfig([FromBody] UserDashboardConfigDto dto)
    {
        try
        {
            // Sécurité : forcer le userId depuis le token, pas depuis le body
            dto.userId = GetUserId();
            await _repo.UpsertAsync(dto);
            return Ok(new { message = "Configuration sauvegardée" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}