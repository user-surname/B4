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
public class DataForecastController : ApiControllerBase
{
    private readonly IDataForecastService _dataForecastService;
    private readonly IMapper _mapper;
    private readonly ILogger<DataForecastController> _logger;

    public DataForecastController(IDataForecastService dataForecastService, IMapper mapper, ILogger<DataForecastController> logger)
    {
        _dataForecastService = dataForecastService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/dataForecast
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll dataForecast requested");
        var records = await _dataForecastService.GetAllAsync();
        _logger.LogInformation("GetAll dataForecast returned {Count} records", records?.Count() ?? 0);
        return OkResponse(records);
    }

    // GET api/v1/dataForecast/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById dataForecast requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _dataForecastService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : (object)t.Result),
            $"No existe dataForecast para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById dataForecast not found. Id: {Id}", id);
        return result;
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DataForecast entity)
    {
        _logger.LogInformation("Create DataForecast requested");
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create DataForecast rejected: invalid model state");
            return BadRequestResponse<object>("Los datos enviados no son válidos");
        }
        await _dataForecastService.AddAsync(entity);
        _logger.LogInformation("Create DataForecast succeeded. Id: {Id}", entity.Id);
        return OkResponse(entity);
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DataForecast entity)
    {
        _logger.LogInformation("Update DataForecast requested. Id: {Id}", id);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Update DataForecast rejected: invalid model state. Id: {Id}", id);
            return BadRequestResponse<object>("Los datos enviados no son válidos");
        }
        if (entity.Id != 0 && entity.Id != id)
        {
            _logger.LogWarning("Update DataForecast rejected: route id {RouteId} does not match body id {BodyId}", id, entity.Id);
            return BadRequestResponse<object>("El id de la ruta no coincide con el id del body.");
        }
        entity.Id = id;
        await _dataForecastService.UpdateAsync(entity);
        _logger.LogInformation("Update DataForecast succeeded. Id: {Id}", id);
        return OkResponse(entity);
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete DataForecast requested. Id: {Id}", id);
        try
        {
            await _dataForecastService.DeleteAsync(id);
            _logger.LogInformation("Delete DataForecast succeeded. Id: {Id}", id);
            return OkResponse(new { deleted = true });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Delete DataForecast failed. Id: {Id}", id);
            return NotFoundResponse<object>(ex.Message);
        }
    }
}
