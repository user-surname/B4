using B4.Api.Middleware;
using B4.Models.Entities.DataEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[CustomAuthorize(Policy = "AdminOnly")]
public class DataForecastController : ControllerBase
{
    private readonly IDataForecastService _service;

    public DataForecastController(IDataForecastService service)
    {
        _service = service;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var record = await _service.GetByIdAsync(id);
        return record is null
            ? NotFound($"No existe DataForecast con id={id}")
            : Ok(record);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await _service.GetAllAsync();
        return Ok(records);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DataForecast entity)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.AddAsync(entity);
        return Ok(entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DataForecast entity)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (entity.Id != 0 && entity.Id != id)
            return BadRequest("El id de la ruta no coincide con el id del body.");

        entity.Id = id;
        await _service.UpdateAsync(entity);
        return Ok(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return Ok(new { message = "Eliminado correctamente" });
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
