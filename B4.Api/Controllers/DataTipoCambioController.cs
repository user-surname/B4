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
    private readonly ILogger<DataTipoCambioController> _logger;

    public DataTipoCambioController(IDataTipoCambioService service, IMapper mapper, ILogger<DataTipoCambioController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    // -------------------------
    // GET básicos
    // -------------------------
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById DataTipoCambio requested. Id: {Id}", id);
        var record = await _service.GetByIdAsync(id);
        if (record is null)
        {
            _logger.LogWarning("GetById DataTipoCambio not found. Id: {Id}", id);
            return NotFound($"No existe DataTipoCambio con id={id}");
        }
        _logger.LogInformation("GetById DataTipoCambio succeeded. Id: {Id}", id);
        return Ok(record);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll DataTipoCambio requested");
        var records = await _service.GetAllAsync();
        _logger.LogInformation("GetAll DataTipoCambio returned records");
        return Ok(records);
    }

    // -------------------------
    // GET funcionales
    // -------------------------
    [HttpGet("ejercicio/{ejercicio:int}")]
    public async Task<IActionResult> GetByEjercicio(int ejercicio)
    {
        _logger.LogInformation("GetByEjercicio DataTipoCambio requested. Ejercicio: {Ejercicio}", ejercicio);
        var records = await _service.GetByEjercicioAsync(ejercicio);
        _logger.LogInformation("GetByEjercicio DataTipoCambio returned records. Ejercicio: {Ejercicio}", ejercicio);
        return Ok(records);
    }

    [HttpGet("ejercicio/{ejercicio:int}/currency/{idCurrency:int}")]
    public async Task<IActionResult> GetByEjercicioCurrency(int ejercicio, int idCurrency)
    {
        _logger.LogInformation("GetByEjercicioCurrency DataTipoCambio requested. Ejercicio: {Ejercicio}, Currency: {Currency}", ejercicio, idCurrency);
        var records = await _service.GetByEjercicioCurrencyAsync(ejercicio, idCurrency);
        _logger.LogInformation("GetByEjercicioCurrency DataTipoCambio returned records");
        return Ok(records);
    }

    // -------------------------
    // POST / PUT / DELETE
    // -------------------------
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DataTipoCambio entity)
    {
        _logger.LogInformation("Create DataTipoCambio requested");
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create DataTipoCambio rejected: invalid model state");
            return BadRequest(ModelState);
        }
        await _service.AddAsync(entity);
        _logger.LogInformation("Create DataTipoCambio succeeded. Id: {Id}", entity.Id);
        return Ok(entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DataTipoCambio entity)
    {
        _logger.LogInformation("Update DataTipoCambio requested. Id: {Id}", id);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Update DataTipoCambio rejected: invalid model state. Id: {Id}", id);
            return BadRequest(ModelState);
        }
        if (entity.Id != 0 && entity.Id != id)
        {
            _logger.LogWarning("Update DataTipoCambio rejected: route id {RouteId} does not match body id {BodyId}", id, entity.Id);
            return BadRequest("El id de la ruta no coincide con el id del body.");
        }
        entity.Id = id;
        await _service.UpdateAsync(entity);
        _logger.LogInformation("Update DataTipoCambio succeeded. Id: {Id}", id);
        return Ok(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete DataTipoCambio requested. Id: {Id}", id);
        try
        {
            await _service.DeleteAsync(id);
            _logger.LogInformation("Delete DataTipoCambio succeeded. Id: {Id}", id);
            return Ok(new { message = "Eliminado correctamente" });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Delete DataTipoCambio failed. Id: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
    }
}
