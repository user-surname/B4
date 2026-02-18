using AutoMapper;
using B4.Api.Middleware;
using B4.Models.Entities.DataEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[CustomAuthorize(Policy = "AdminOnly")]
public class DataBudgetController : ControllerBase
{
    private readonly IDataBudgetService _service;
    private readonly IMapper _mapper;

    public DataBudgetController(IDataBudgetService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var record = await _service.GetByIdAsync(id);
        return record is null
            ? NotFound($"No existe DataBudget con id={id}")
            : Ok(record); // luego: Ok(_mapper.Map<...Dto>(record))
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await _service.GetAllAsync();
        return Ok(records); // luego: Ok(_mapper.Map<List<...Dto>>(records))
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DataBudget entity)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _service.AddAsync(entity);
        return Ok(entity); // luego: Ok(_mapper.Map<...Dto>(entity))
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DataBudget entity)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (entity.Id != 0 && entity.Id != id)
            return BadRequest("El id de la ruta no coincide con el id del body.");

        entity.Id = id;
        await _service.UpdateAsync(entity);
        return Ok(entity); // luego: Ok(_mapper.Map<...Dto>(entity))
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
