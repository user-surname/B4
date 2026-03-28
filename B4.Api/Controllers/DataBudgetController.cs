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
public class DataBudgetController : ControllerBase
{
    private readonly IDataBudgetService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<DataBudgetController> _logger;

    public DataBudgetController(IDataBudgetService service, IMapper mapper, ILogger<DataBudgetController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById DataBudget requested. Id: {Id}", id);
        var record = await _service.GetByIdAsync(id);
        if (record is null)
        {
            _logger.LogWarning("GetById DataBudget not found. Id: {Id}", id);
            return NotFound($"No existe DataBudget con id={id}");
        }
        _logger.LogInformation("GetById DataBudget succeeded. Id: {Id}", id);
        return Ok(record);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll DataBudget requested");
        var records = await _service.GetAllAsync();
        _logger.LogInformation("GetAll DataBudget returned records");
        return Ok(records);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DataBudget entity)
    {
        _logger.LogInformation("Create DataBudget requested");
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create DataBudget rejected: invalid model state");
            return BadRequest(ModelState);
        }
        await _service.AddAsync(entity);
        _logger.LogInformation("Create DataBudget succeeded. Id: {Id}", entity.Id);
        return Ok(entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DataBudget entity)
    {
        _logger.LogInformation("Update DataBudget requested. Id: {Id}", id);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Update DataBudget rejected: invalid model state. Id: {Id}", id);
            return BadRequest(ModelState);
        }
        if (entity.Id != 0 && entity.Id != id)
        {
            _logger.LogWarning("Update DataBudget rejected: route id {RouteId} does not match body id {BodyId}", id, entity.Id);
            return BadRequest("El id de la ruta no coincide con el id del body.");
        }
        entity.Id = id;
        await _service.UpdateAsync(entity);
        _logger.LogInformation("Update DataBudget succeeded. Id: {Id}", id);
        return Ok(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete DataBudget requested. Id: {Id}", id);
        try
        {
            await _service.DeleteAsync(id);
            _logger.LogInformation("Delete DataBudget succeeded. Id: {Id}", id);
            return Ok(new { message = "Eliminado correctamente" });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Delete DataBudget failed. Id: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
    }
}
