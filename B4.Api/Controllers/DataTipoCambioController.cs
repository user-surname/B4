using AutoMapper;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities.DataEntities;
using B4.Models.ServiceInterfaces;
using B4.Api.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[CustomAuthorize(Policy = "AdminOnly")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DataTipoCambioController : ApiControllerBase
{
    private readonly IDataTipoCambioService _dataTipoCambioService;
    private readonly IMapper _mapper;
    private readonly ILogger<DataTipoCambioController> _logger;

    public DataTipoCambioController(IDataTipoCambioService dataTipoCambioService, IMapper mapper, ILogger<DataTipoCambioController> logger)
    {
        _dataTipoCambioService = dataTipoCambioService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/dataTipoCambio
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll dataTipoCambio requested");
        var records = await _dataTipoCambioService.GetAllAsync();
        _logger.LogInformation("GetAll dataTipoCambio returned {Count} records", records?.Count() ?? 0);
        return OkResponse(records);
    }

    // GET api/v1/dataTipoCambio/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById dataTipoCambio requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _dataTipoCambioService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : (object)t.Result),
            $"No existe dataTipoCambio para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById dataTipoCambio not found. Id: {Id}", id);
        return result;
    }


    [HttpGet("ejercicio/{ejercicio:int}")]
    public async Task<IActionResult> GetByEjercicio(int ejercicio)
    {
        _logger.LogInformation("GetByEjercicio DataTipoCambio requested. Ejercicio: {Ejercicio}", ejercicio);
        var records = await _dataTipoCambioService.GetByEjercicioAsync(ejercicio);
        _logger.LogInformation("GetByEjercicio DataTipoCambio returned records. Ejercicio: {Ejercicio}", ejercicio);
        return OkResponse(records);
    }


    [HttpGet("ejercicio/{ejercicio:int}/currency/{idCurrency:int}")]
    public async Task<IActionResult> GetByEjercicioCurrency(int ejercicio, int idCurrency)
    {
        _logger.LogInformation("GetByEjercicioCurrency DataTipoCambio requested. Ejercicio: {Ejercicio}, Currency: {Currency}", ejercicio, idCurrency);
        var records = await _dataTipoCambioService.GetByEjercicioCurrencyAsync(ejercicio, idCurrency);
        _logger.LogInformation("GetByEjercicioCurrency DataTipoCambio returned records");
        return OkResponse(records);
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
            return BadRequestResponse<object>("Los datos enviados no son válidos");
        }
        await _dataTipoCambioService.AddAsync(entity);
        _logger.LogInformation("Create DataTipoCambio succeeded. Id: {Id}", entity.Id);
        return OkResponse(entity);
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DataTipoCambio entity)
    {
        _logger.LogInformation("Update DataTipoCambio requested. Id: {Id}", id);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Update DataTipoCambio rejected: invalid model state. Id: {Id}", id);
            return BadRequestResponse<object>("Los datos enviados no son válidos");
        }
        if (entity.Id != 0 && entity.Id != id)
        {
            _logger.LogWarning("Update DataTipoCambio rejected: route id {RouteId} does not match body id {BodyId}", id, entity.Id);
            return BadRequestResponse<object>("El id de la ruta no coincide con el id del body.");
        }
        entity.Id = id;
        await _dataTipoCambioService.UpdateAsync(entity);
        _logger.LogInformation("Update DataTipoCambio succeeded. Id: {Id}", id);
        return OkResponse(entity);
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete DataTipoCambio requested. Id: {Id}", id);
        try
        {
            await _dataTipoCambioService.DeleteAsync(id);
            _logger.LogInformation("Delete DataTipoCambio succeeded. Id: {Id}", id);
            return OkResponse(new { deleted = true });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Delete DataTipoCambio failed. Id: {Id}", id);
            return NotFoundResponse<object>(ex.Message);
        }
    }
}
