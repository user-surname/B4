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
public class DataTipoCambioController : ControllerBase
{
    private readonly IDataTipoCambioService _service;
    private readonly IMapper _mapper;

    public DataTipoCambioController(IDataTipoCambioService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    // -------------------------
    // GET básicos
    // -------------------------
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var record = await _service.GetByIdAsync(id);
        return record is null
            ? NotFound($"No existe DataTipoCambio con id={id}")
            : Ok(record); // luego: Ok(_mapper.Map<...Dto>(record))
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await _service.GetAllAsync();
        return Ok(records); // luego: Ok(_mapper.Map<List<...Dto>>(records))
    }

    // -------------------------
    // GET funcionales
    // -------------------------
    [HttpGet("ejercicio/{ejercicio:int}")]
    public async Task<IActionResult> GetByEjercicio(int ejercicio)
    {
        var records = await _service.GetByEjercicioAsync(ejercicio);
        return Ok(records);
    }

    [HttpGet("ejercicio/{ejercicio:int}/currency/{idCurrency:int}")]
    public async Task<IActionResult> GetByEjercicioCurrency(int ejercicio, int idCurrency)
    {
        var records = await _service.GetByEjercicioCurrencyAsync(ejercicio, idCurrency);
        return Ok(records);
    }

    // -------------------------
    // POST / PUT / DELETE
    // -------------------------
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DataTipoCambio entity)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.AddAsync(entity);
        return Ok(entity); // luego: Ok(_mapper.Map<...Dto>(entity))
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DataTipoCambio entity)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

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
