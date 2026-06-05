using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO.Dashboard;
using scoring_Backend.Repositories.Interfaces.Dashboard;

namespace scoring_Backend.Controllers.Dashboard
{
    [Authorize]
    [ApiController]
    [Route("api/dashboard-v2")]
    public class DashboardV2Controller : ControllerBase
    {
        private readonly IDashboardRepository _repo;

        public DashboardV2Controller(IDashboardRepository repo) => _repo = repo;

        private int    UserId   => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        private string UserRole => User.FindFirstValue(ClaimTypes.Role) ?? "Agent";
        private int    SiteId   => int.Parse(User.FindFirstValue("siteId") ?? "0");

        [HttpPost]
        public async Task<IActionResult> GetDashboard([FromBody] DashboardFilterDto filter)
        {
            try
            {
                var data = await _repo.GetDashboardAsync(filter, UserId, UserRole, SiteId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}