using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scoring_Backend.DTO;
using scoring_Backend.Repositories.Interfaces.Configuration;

namespace scoring_Backend.Controllers.Configuration
{   [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationCampagnesRepository _repo;
        private readonly ILogger<ConfigurationController> _logger;

        public ConfigurationController(
            IConfigurationCampagnesRepository repo,
            ILogger<ConfigurationController> logger)
        {
            _repo   = repo;
            _logger = logger;
        }

        // ── Lookups ──────────────────────────────────────────────────────────

        [HttpGet("periodes")]
        public async Task<IActionResult> GetPeriodes()
            => Ok(await _repo.GetPeriodesAsync());

        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers()
            => Ok(await _repo.GetCustomersAsync());

        [HttpGet("available-campaigns")]
        public async Task<IActionResult> GetAvailableCampaigns([FromQuery] int? excludeTemplateId)
            => Ok(await _repo.GetAvailableCampaignsAsync(excludeTemplateId));

        // ── Templates ────────────────────────────────────────────────────────

        [HttpGet("templates")]
        public async Task<IActionResult> GetTemplates()
            => Ok(await _repo.GetAllTemplatesAsync());

        [HttpGet("templates/{id:int}")]
        public async Task<IActionResult> GetTemplate(int id)
        {
            var data = await _repo.GetTemplateByIdAsync(id);
            return data is null ? NotFound() : Ok(data);
        }

       [HttpPost("templates")]
public async Task<IActionResult> CreateTemplate([FromBody] CreateLsTemplateDto dto)
{
    try
    {
        // 🔍 LOG pour voir ce qui arrive
        Console.WriteLine($"=== CreateTemplate ===");
        Console.WriteLine($"Description: {dto.Description}");
        Console.WriteLine($"StartDate: {dto.StartDate}");
        Console.WriteLine($"EndDate: {dto.EndDate}");
        Console.WriteLine($"PeriodeId: {dto.LsTemplatePeriodeId}");

        if (await _repo.TemplateExistsByDescriptionAsync(dto.Description))
            return Conflict(new { message = "Un modèle avec ce nom existe déjà." });

        var id = await _repo.CreateTemplateAsync(dto);
        return CreatedAtAction(nameof(GetTemplate), new { id }, new { id });
    }
    catch (Exception ex)
    {
        // 🔍 LOG l'erreur complète
        Console.WriteLine($"ERREUR: {ex.Message}");
        Console.WriteLine($"STACK: {ex.StackTrace}");
        _logger.LogError(ex, "Erreur CreateTemplate");
        return StatusCode(500, new { message = ex.Message, detail = ex.InnerException?.Message });
    }
}

        [HttpPut("templates/{id:int}")]
        public async Task<IActionResult> UpdateTemplate(int id, [FromBody] UpdateLsTemplateDto dto)
        {
            if (await _repo.GetTemplateByIdAsync(id) is null) return NotFound();
            await _repo.UpdateTemplateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("templates/{id:int}")]
        public async Task<IActionResult> DeleteTemplate(int id)
        {
            if (await _repo.GetTemplateByIdAsync(id) is null) return NotFound();
            if (!await _repo.CanDeleteTemplateAsync(id))
                return Conflict(new { message = "Ce modèle ne peut pas être supprimé car des évaluations y sont associées." });
            await _repo.DeleteTemplateAsync(id);
            return NoContent();
        }

        [HttpPost("templates/{id:int}/change-model")]
        public async Task<IActionResult> ChangeModel(int id, [FromBody] ChangeModelDto dto)
        {
            if (await _repo.GetTemplateByIdAsync(id) is null) return NotFound();
            if (await _repo.HasPendingVersionAsync(id))
                return Conflict(new { message = "Ce modèle a déjà été versionné." });
            if (await _repo.TemplateExistsByDescriptionAsync(dto.Description))
                return Conflict(new { message = "Un modèle avec ce nom existe déjà." });

            var newId = await _repo.ChangeModelAsync(id, dto);
            return CreatedAtAction(nameof(GetTemplate), new { id = newId }, new { id = newId });
        }

        // ── Item Groups ──────────────────────────────────────────────────────

        [HttpGet("templates/{templateId:int}/groups")]
        public async Task<IActionResult> GetItemGroups(int templateId)
            => Ok(await _repo.GetItemGroupsByTemplateAsync(templateId));

        [HttpPost("templates/{templateId:int}/groups")]
        public async Task<IActionResult> CreateItemGroup(int templateId, [FromBody] CreateItemGroupDto dto)
        {
            var id = await _repo.CreateItemGroupAsync(templateId, dto);
            return Ok(new { id });
        }

        [HttpPut("groups/{id:int}")]
        public async Task<IActionResult> UpdateItemGroup(int id, [FromBody] UpdateItemGroupDto dto)
        {
            await _repo.UpdateItemGroupAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("groups/{id:int}")]
        public async Task<IActionResult> DeleteItemGroup(int id)
        {
            await _repo.DeleteItemGroupAsync(id);
            return NoContent();
        }

        // ── Template Items ───────────────────────────────────────────────────

        [HttpGet("groups/{groupId:int}/items")]
        public async Task<IActionResult> GetItems(int groupId)
            => Ok(await _repo.GetItemsByGroupAsync(groupId));

        [HttpPost("items")]
        public async Task<IActionResult> CreateTemplateItem([FromBody] CreateTemplateItemDto dto)
        {
            var id = await _repo.CreateTemplateItemAsync(dto);
            return Ok(new { id });
        }

        [HttpPut("items/{id:int}")]
        public async Task<IActionResult> UpdateTemplateItem(int id, [FromBody] UpdateTemplateItemDto dto)
        {
            await _repo.UpdateTemplateItemAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("items/{id:int}")]
        public async Task<IActionResult> DeleteTemplateItem(int id)
        {
            await _repo.DeleteTemplateItemAsync(id);
            return NoContent();
        }

        // ── Called Campaigns ─────────────────────────────────────────────────

        [HttpGet("templates/{templateId:int}/campaigns")]
        public async Task<IActionResult> GetCalledCampaigns(int templateId)
            => Ok(await _repo.GetCalledCampaignsByTemplateAsync(templateId));

        [HttpPost("campaigns")]
        public async Task<IActionResult> CreateCalledCampaign([FromBody] CreateCalledCampaignDto dto)
        {
            var id = await _repo.CreateCalledCampaignAsync(dto);
            return Ok(new { id });
        }

        [HttpPut("campaigns/{id:int}")]
        public async Task<IActionResult> UpdateCalledCampaign(int id, [FromBody] UpdateCalledCampaignDto dto)
        {
            await _repo.UpdateCalledCampaignAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("campaigns/{id:int}")]
        public async Task<IActionResult> DeleteCalledCampaign(int id)
        {
            if (!await _repo.CanDeleteCalledCampaignAsync(id))
                return Conflict(new { message = "Cette campagne ne peut pas être supprimée car des évaluations y sont associées." });
            await _repo.DeleteCalledCampaignAsync(id);
            return NoContent();
        }
[HttpGet("available-campaigns/by-site/{customerId:int}")]
public async Task<IActionResult> GetAvailableCampaignsBySite(
    int customerId,
    [FromQuery] int? templateId)  // ✅ cette ligne doit exister
    => Ok(await _repo.GetAvailableCampaignsBySiteAsync(customerId, templateId));
    }
}