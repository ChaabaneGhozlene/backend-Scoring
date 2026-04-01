using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO;
using scoring_Backend.Repositories.Interfaces.Evaluation;

namespace scoring_Backend.Controllers.Evaluation
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class EvalUsersController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public EvalUsersController(IUserRepository repo) => _repo = repo;

        // ────────────────────────────────────────────────────
        // GET /api/eval-users?page=1&pageSize=15
        // ────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page     = 1,
            [FromQuery] int pageSize = 15)
        {
            try
            {
                var result = await _repo.GetUsersAsync(page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ────────────────────────────────────────────────────
        // GET /api/eval-users/{id}
        // ────────────────────────────────────────────────────
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _repo.GetByIdAsync(id);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ────────────────────────────────────────────────────
        // POST /api/eval-users
        // ────────────────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var created = await _repo.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ────────────────────────────────────────────────────
        // PUT /api/eval-users/{id}
        // ────────────────────────────────────────────────────
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updated = await _repo.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ────────────────────────────────────────────────────
        // DELETE /api/eval-users
        // Body: [1, 2, 3]  (liste d'IDs)
        // ────────────────────────────────────────────────────
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return BadRequest(new { message = "No IDs provided" });

            try
            {
                await _repo.DeleteAsync(ids);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Utilisateur lié à des fiches/actions → ne peut pas être supprimé
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ────────────────────────────────────────────────────
        // GET /api/eval-users/check-login?login=xxx&excludeId=1
        // ────────────────────────────────────────────────────
        [HttpGet("check-login")]
        public async Task<IActionResult> CheckLogin(
            [FromQuery] string login,
            [FromQuery] int?   excludeId = null)
        {
            var exists = await _repo.LoginExistsAsync(login, excludeId);
            return Ok(new { exists });
        }

        // ────────────────────────────────────────────────────
        // GET /api/eval-users/sites
        // ────────────────────────────────────────────────────
        [HttpGet("sites")]
        public async Task<IActionResult> GetSites()
        {
            try
            {
                var sites = await _repo.GetSitesAsync();
                return Ok(sites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}