using AutoMapper;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.ServiceInterfaces;
using B4.Api.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[CustomAuthorize(Policy = "AdminOnly")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DataForecastBwController : ApiControllerBase
{
    private readonly IDataForecastBwService _dataForecastBwService;
    private readonly IMapper _mapper;
    private readonly ILogger<DataForecastBwController> _logger;

    public DataForecastBwController(IDataForecastBwService dataForecastBwService, IMapper mapper, ILogger<DataForecastBwController> logger)
    {
        _dataForecastBwService = dataForecastBwService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/dataForecastBw
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll dataForecastBw requested");
        var records = await _dataForecastBwService.GetAllAsync();
        _logger.LogInformation("GetAll dataForecastBw returned {Count} records", records?.Count() ?? 0);
        return OkResponse(records);
    }

    // GET api/v1/dataForecastBw/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById dataForecastBw requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _dataForecastBwService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : (object)t.Result),
            $"No existe dataForecastBw para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById dataForecastBw not found. Id: {Id}", id);
        return result;
    }

}
