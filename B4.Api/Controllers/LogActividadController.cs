using B4.Api.Middleware;
using B4.Models.Entities.DataEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[CustomAuthorize(Policy = "AdminOnly")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LogActividadController : ApiControllerBase
{
    private readonly ILogActividadService _logActividadService;
    private readonly ILogger<LogActividadController> _logger;

    public LogActividadController(ILogActividadService logActividadService, ILogger<LogActividadController> logger)
    {
        _logActividadService = logActividadService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll log_actividad requested");
        var records = await _logActividadService.GetAllAsync();
        _logger.LogInformation("GetAll log_actividad returned {Count} records", records?.Count() ?? 0);
        return OkResponse(records);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById log_actividad requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _logActividadService.GetByIdAsync(id).ContinueWith(t => t.Result is null ? null : (object)t.Result),
            $"No existe log_actividad para id={id}");

        if (result is NotFoundObjectResult)
        {
            _logger.LogWarning("GetById log_actividad not found. Id: {Id}", id);
        }

        return result;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LogActividad entity)
    {
        _logger.LogInformation("Create log_actividad requested");
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create log_actividad rejected: invalid model state");
            return BadRequestResponse<object>("Los datos enviados no son válidos");
        }

        await _logActividadService.AddAsync(entity);
        _logger.LogInformation("Create log_actividad succeeded. Id: {Id}", entity.Id);
        return OkResponse(entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] LogActividad entity)
    {
        _logger.LogInformation("Update log_actividad requested. Id: {Id}", id);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Update log_actividad rejected: invalid model state. Id: {Id}", id);
            return BadRequestResponse<object>("Los datos enviados no son válidos");
        }

        if (entity.Id != 0 && entity.Id != id)
        {
            _logger.LogWarning("Update log_actividad rejected: route id {RouteId} does not match body id {BodyId}", id, entity.Id);
            return BadRequestResponse<object>("El id de la ruta no coincide con el id del body.");
        }

        entity.Id = id;
        await _logActividadService.UpdateAsync(entity);
        _logger.LogInformation("Update log_actividad succeeded. Id: {Id}", id);
        return OkResponse(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete log_actividad requested. Id: {Id}", id);
        try
        {
            await _logActividadService.DeleteAsync(id);
            _logger.LogInformation("Delete log_actividad succeeded. Id: {Id}", id);
            return OkResponse(new { deleted = true });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Delete log_actividad failed. Id: {Id}", id);
            return NotFoundResponse<object>(ex.Message);
        }
    }
}
