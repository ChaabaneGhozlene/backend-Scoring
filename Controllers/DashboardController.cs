using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO.Dashboard;
using scoring_Backend.Services;

namespace scoring_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpPost]
        public async Task<ActionResult<DashboardResponseDto>> GetDashboard([FromBody] DashboardFilterDto filter)
        {
            var result = await _dashboardService.GetDashboardAsync(filter);
            return Ok(result);
        }
    }
}