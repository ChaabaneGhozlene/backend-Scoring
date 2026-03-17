using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO;
using scoring_Backend.Repositories.Interfaces.Evaluation;

namespace scoring_Backend.Controllers.Evaluation
{
    [ApiController]
    [Route("api/filters")]
    [Authorize]
    public class FiltersController : ControllerBase
    {
        private readonly IFilterRepository _repo;

        public FiltersController(IFilterRepository repo) => _repo = repo;

        private string CurrentUserLogin =>
            User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repo.GetUserFiltersAsync(CurrentUserLogin));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFilterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var created = await _repo.CreateFilterAsync(CurrentUserLogin, dto);
                return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repo.DeleteFilterAsync(id, CurrentUserLogin);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}