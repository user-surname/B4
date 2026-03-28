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
public class DataForecastController : ControllerBase
{
    private readonly IDataForecastService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<DataForecastController> _logger;

    public DataForecastController(IDataForecastService service, IMapper mapper, ILogger<DataForecastController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById DataForecast requested. Id: {Id}", id);
        var record = await _service.GetByIdAsync(id);
        if (record is null)
        {
            _logger.LogWarning("GetById DataForecast not found. Id: {Id}", id);
            return NotFound($"No existe DataForecast con id={id}");
        }
        _logger.LogInformation("GetById DataForecast succeeded. Id: {Id}", id);
        return Ok(record);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll DataForecast requested");
        var records = await _service.GetAllAsync();
        _logger.LogInformation("GetAll DataForecast returned records");
        return Ok(records);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DataForecast entity)
    {
        _logger.LogInformation("Create DataForecast requested");
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create DataForecast rejected: invalid model state");
            return BadRequest(ModelState);
        }
        await _service.AddAsync(entity);
        _logger.LogInformation("Create DataForecast succeeded. Id: {Id}", entity.Id);
        return Ok(entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DataForecast entity)
    {
        _logger.LogInformation("Update DataForecast requested. Id: {Id}", id);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Update DataForecast rejected: invalid model state. Id: {Id}", id);
            return BadRequest(ModelState);
        }
        if (entity.Id != 0 && entity.Id != id)
        {
            _logger.LogWarning("Update DataForecast rejected: route id {RouteId} does not match body id {BodyId}", id, entity.Id);
            return BadRequest("El id de la ruta no coincide con el id del body.");
        }
        entity.Id = id;
        await _service.UpdateAsync(entity);
        _logger.LogInformation("Update DataForecast succeeded. Id: {Id}", id);
        return Ok(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete DataForecast requested. Id: {Id}", id);
        try
        {
            await _service.DeleteAsync(id);
            _logger.LogInformation("Delete DataForecast succeeded. Id: {Id}", id);
            return Ok(new { message = "Eliminado correctamente" });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Delete DataForecast failed. Id: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
    }
}
