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
public class DataBridgesMonthController : ApiControllerBase
{
    private readonly IDataBridgesMonthService _dataBridgesMonthService;
    private readonly IMapper _mapper;
    private readonly ILogger<DataBridgesMonthController> _logger;

    public DataBridgesMonthController(IDataBridgesMonthService dataBridgesMonthService, IMapper mapper, ILogger<DataBridgesMonthController> logger)
    {
        _dataBridgesMonthService = dataBridgesMonthService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/dataBridgesMonth
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll dataBridgesMonth requested");
        var records = await _dataBridgesMonthService.GetAllAsync();
        _logger.LogInformation("GetAll dataBridgesMonth returned {Count} records", records?.Count() ?? 0);
        return OkResponse(records);
    }

    // GET api/v1/dataBridgesMonth/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById dataBridgesMonth requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _dataBridgesMonthService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : (object)t.Result),
            $"No existe dataBridgesMonth para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById dataBridgesMonth not found. Id: {Id}", id);
        return result;
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DataBridgesMonth entity)
    {
        _logger.LogInformation("Create DataBridgesMonth requested");
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create DataBridgesMonth rejected: invalid model state");
            return BadRequestResponse<object>("Los datos enviados no son válidos");
        }
        await _dataBridgesMonthService.AddAsync(entity);
        _logger.LogInformation("Create DataBridgesMonth succeeded. Id: {Id}", entity.Id);
        return OkResponse(entity);
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DataBridgesMonth entity)
    {
        _logger.LogInformation("Update DataBridgesMonth requested. Id: {Id}", id);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Update DataBridgesMonth rejected: invalid model state. Id: {Id}", id);
            return BadRequestResponse<object>("Los datos enviados no son válidos");
        }
        if (entity.Id != 0 && entity.Id != id)
        {
            _logger.LogWarning("Update DataBridgesMonth rejected: route id {RouteId} does not match body id {BodyId}", id, entity.Id);
            return BadRequestResponse<object>("El id de la ruta no coincide con el id del body.");
        }
        entity.Id = id;
        await _dataBridgesMonthService.UpdateAsync(entity);
        _logger.LogInformation("Update DataBridgesMonth succeeded. Id: {Id}", id);
        return OkResponse(entity);
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete DataBridgesMonth requested. Id: {Id}", id);
        try
        {
            await _dataBridgesMonthService.DeleteAsync(id);
            _logger.LogInformation("Delete DataBridgesMonth succeeded. Id: {Id}", id);
            return OkResponse(new { deleted = true });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Delete DataBridgesMonth failed. Id: {Id}", id);
            return NotFoundResponse<object>(ex.Message);
        }
    }
}
