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
public class DataBridgesMonthBwController : ApiControllerBase
{
    private readonly IDataBridgesMonthBwService _dataBridgesMonthBwService;
    private readonly IMapper _mapper;
    private readonly ILogger<DataBridgesMonthBwController> _logger;

    public DataBridgesMonthBwController(IDataBridgesMonthBwService dataBridgesMonthBwService, IMapper mapper, ILogger<DataBridgesMonthBwController> logger)
    {
        _dataBridgesMonthBwService = dataBridgesMonthBwService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/dataBridgesMonthBw
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll dataBridgesMonthBw requested");
        var records = await _dataBridgesMonthBwService.GetAllAsync();
        _logger.LogInformation("GetAll dataBridgesMonthBw returned {Count} records", records?.Count() ?? 0);
        return OkResponse(records);
    }

    // GET api/v1/dataBridgesMonthBw/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById dataBridgesMonthBw requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _dataBridgesMonthBwService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : (object)t.Result),
            $"No existe dataBridgesMonthBw para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById dataBridgesMonthBw not found. Id: {Id}", id);
        return result;
    }

}
