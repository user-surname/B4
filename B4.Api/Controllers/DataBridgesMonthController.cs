using AutoMapper;
using B4.Api.Middleware;
using B4.Models.Entities.DataEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[CustomAuthorize(Policy = "AdminOnly")]
public class DataBridgesMonthController : ControllerBase
{
    private readonly IDataBridgesMonthService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<DataBridgesMonthController> _logger;

    public DataBridgesMonthController(IDataBridgesMonthService service, IMapper mapper, ILogger<DataBridgesMonthController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById DataBridgesMonth requested. Id: {Id}", id);
        var record = await _service.GetByIdAsync(id);
        if (record is null)
        {
            _logger.LogWarning("GetById DataBridgesMonth not found. Id: {Id}", id);
            return NotFound($"No existe DataBridgesMonth con id={id}");
        }
        _logger.LogInformation("GetById DataBridgesMonth succeeded. Id: {Id}", id);
        return Ok(record);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll DataBridgesMonth requested");
        var records = await _service.GetAllAsync();
        _logger.LogInformation("GetAll DataBridgesMonth returned records");
        return Ok(records);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DataBridgesMonth entity)
    {
        _logger.LogInformation("Create DataBridgesMonth requested");
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create DataBridgesMonth rejected: invalid model state");
            return BadRequest(ModelState);
        }
        await _service.AddAsync(entity);
        _logger.LogInformation("Create DataBridgesMonth succeeded. Id: {Id}", entity.Id);
        return Ok(entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DataBridgesMonth entity)
    {
        _logger.LogInformation("Update DataBridgesMonth requested. Id: {Id}", id);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Update DataBridgesMonth rejected: invalid model state. Id: {Id}", id);
            return BadRequest(ModelState);
        }
        if (entity.Id != 0 && entity.Id != id)
        {
            _logger.LogWarning("Update DataBridgesMonth rejected: route id {RouteId} does not match body id {BodyId}", id, entity.Id);
            return BadRequest("El id de la ruta no coincide con el id del body.");
        }
        entity.Id = id;
        await _service.UpdateAsync(entity);
        _logger.LogInformation("Update DataBridgesMonth succeeded. Id: {Id}", id);
        return Ok(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete DataBridgesMonth requested. Id: {Id}", id);
        try
        {
            await _service.DeleteAsync(id);
            _logger.LogInformation("Delete DataBridgesMonth succeeded. Id: {Id}", id);
            return Ok(new { message = "Eliminado correctamente" });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Delete DataBridgesMonth failed. Id: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
    }
}
