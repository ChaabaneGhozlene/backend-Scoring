using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO;
using scoring_Backend.Repositories.Interfaces.Evaluation;
using System.IO.Compression;
namespace scoring_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecordsController : ControllerBase
    {
        private readonly IRecordRepository _recordRepo;

        public RecordsController(IRecordRepository recordRepo)
        {
            _recordRepo = recordRepo;
        }

        private int    CurrentUserId   =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        private string CurrentUserRole =>
            User.FindFirstValue(ClaimTypes.Role) ?? "Agent";

        // ══════════════════════════════════════════════
        // POST /api/records/search
        // ══════════════════════════════════════════════
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] RecordFilterDto filter)
        {
            if (filter == null)
                return BadRequest("Filtre invalide.");

            var result = await _recordRepo.GetRecordsByFilterAsync(
                filter, CurrentUserId, CurrentUserRole);

            return Ok(result);
        }

        // ══════════════════════════════════════════════
        // GET /api/records/{id}
        // ══════════════════════════════════════════════
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _recordRepo.GetByIdAsync(id);
            if (record == null)
                return NotFound($"Record {id} introuvable.");
            return Ok(record);
        }

      
        // ══════════════════════════════════════════════
        // POST /api/records/export  →  CSV
        // ══════════════════════════════════════════════
        [HttpPost("export")]
        public async Task<IActionResult> Export([FromBody] RecordFilterDto filter)
        {
            if (filter == null)
                return BadRequest("Filtre invalide.");

            var csvBytes = await _recordRepo.ExportToCsvAsync(
                filter, CurrentUserId, CurrentUserRole);

            return File(csvBytes, "text/csv; charset=utf-8", "records_export.csv");
        }

   

      
        // ══════════════════════════════════════════════
        // POST /api/records/trace
        // Sauvegarde une action d'écoute (Play/Pause/…)
        // Toujours 200 — non bloquant
        // ══════════════════════════════════════════════
        [HttpPost("trace")]
        public async Task<IActionResult> Trace([FromBody] TraceActionDto dto)
        {
            if (dto == null || dto.RecordId <= 0)
                return Ok();

            try
            {
                await _recordRepo.SaveTraceAsync(dto, CurrentUserId);
            }
            catch
            {
                // trace non bloquant : erreur ignorée
            }

            return Ok();
        }
    
    [HttpGet("{id:int}/history")]
public async Task<IActionResult> GetListenHistory(int id)
{
    try
    {
        var history = await _recordRepo.GetListenHistoryAsync(id);
        return Ok(history);
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = ex.Message });
    }
}
[HttpGet("{id:int}/screen-history")]
public async Task<IActionResult> GetScreenHistory(int id)
{
    try
    {
        var history = await _recordRepo.GetScreenHistoryAsync(id);
        return Ok(history);
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = ex.Message });
    }
}
[HttpDelete("{id:int}")]
public async Task<IActionResult> DeleteRecord(int id)
{
    try
    {
        await _recordRepo.DeleteRecordAsync(id);
        return Ok(new { message = "Enregistrement supprimé avec succès." });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = ex.Message });
    }
}

// ══════════════════════════════════════════════
        // GET /api/records/{id}/stream  →  audio
        // ══════════════════════════════════════════════
        [HttpGet("{id:int}/stream")]
        public async Task<IActionResult> StreamRecord(int id)
        {
            try
            {
                var path = await _recordRepo.GetRecordSourceAsync(id);
                if (path == null || !System.IO.File.Exists(path))
                    return NotFound("Fichier audio introuvable.");

                var stream = System.IO.File.OpenRead(path);
                return File(stream, "audio/mpeg", enableRangeProcessing: true);
            }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

       
    }
}