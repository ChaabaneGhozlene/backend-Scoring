using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO;
using scoring_Backend.Repositories.Interfaces.Evaluation;

namespace scoring_Backend.Controllers.Evaluation
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ViewConfigsController : ControllerBase
    {
        private readonly IViewConfigRepository _repo;

        public ViewConfigsController(IViewConfigRepository repo) => _repo = repo;

        private string CurrentUserLogin =>
            User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

        // GET /api/viewconfigs
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repo.GetUserConfigsAsync(CurrentUserLogin));

        // POST /api/viewconfigs
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateViewConfigDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var created = await _repo.CreateConfigAsync(CurrentUserLogin, dto);
                return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT /api/viewconfigs/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLayoutDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var updated = await _repo.UpdateConfigAsync(id, CurrentUserLogin, dto.LayoutJson);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE /api/viewconfigs/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repo.DeleteConfigAsync(id, CurrentUserLogin);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}