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
public class DataBridgesFyController : ApiControllerBase
{
    private readonly IDataBridgesFyService _dataBridgesFyService;
    private readonly IMapper _mapper;
    private readonly ILogger<DataBridgesFyController> _logger;

    public DataBridgesFyController(IDataBridgesFyService dataBridgesFyService, IMapper mapper, ILogger<DataBridgesFyController> logger)
    {
        _dataBridgesFyService = dataBridgesFyService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/dataBridgesFy
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll dataBridgesFy requested");
        var records = await _dataBridgesFyService.GetAllAsync();
        _logger.LogInformation("GetAll dataBridgesFy returned {Count} records", records?.Count() ?? 0);
        return OkResponse(records);
    }

    // GET api/v1/dataBridgesFy/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById dataBridgesFy requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _dataBridgesFyService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : (object)t.Result),
            $"No existe dataBridgesFy para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById dataBridgesFy not found. Id: {Id}", id);
        return result;
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DataBridgesFy entity)
    {
        _logger.LogInformation("Create DataBridgesFy requested");
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create DataBridgesFy rejected: invalid model state");
            return BadRequestResponse<object>("Los datos enviados no son válidos");
        }
        await _dataBridgesFyService.AddAsync(entity);
        _logger.LogInformation("Create DataBridgesFy succeeded. Id: {Id}", entity.Id);
        return OkResponse(entity);
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DataBridgesFy entity)
    {
        _logger.LogInformation("Update DataBridgesFy requested. Id: {Id}", id);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Update DataBridgesFy rejected: invalid model state. Id: {Id}", id);
            return BadRequestResponse<object>("Los datos enviados no son válidos");
        }
        if (entity.Id != 0 && entity.Id != id)
        {
            _logger.LogWarning("Update DataBridgesFy rejected: route id {RouteId} does not match body id {BodyId}", id, entity.Id);
            return BadRequestResponse<object>("El id de la ruta no coincide con el id del body.");
        }
        entity.Id = id;
        await _dataBridgesFyService.UpdateAsync(entity);
        _logger.LogInformation("Update DataBridgesFy succeeded. Id: {Id}", id);
        return OkResponse(entity);
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete DataBridgesFy requested. Id: {Id}", id);
        try
        {
            await _dataBridgesFyService.DeleteAsync(id);
            _logger.LogInformation("Delete DataBridgesFy succeeded. Id: {Id}", id);
            return OkResponse(new { deleted = true });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Delete DataBridgesFy failed. Id: {Id}", id);
            return NotFoundResponse<object>(ex.Message);
        }
    }
}
