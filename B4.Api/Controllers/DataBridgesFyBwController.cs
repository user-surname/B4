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
public class DataBridgesFyBwController : ApiControllerBase
{
    private readonly IDataBridgesFyBwService _dataBridgesFyBwService;
    private readonly IMapper _mapper;
    private readonly ILogger<DataBridgesFyBwController> _logger;

    public DataBridgesFyBwController(IDataBridgesFyBwService dataBridgesFyBwService, IMapper mapper, ILogger<DataBridgesFyBwController> logger)
    {
        _dataBridgesFyBwService = dataBridgesFyBwService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/dataBridgesFyBw
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll dataBridgesFyBw requested");
        var records = await _dataBridgesFyBwService.GetAllAsync();
        _logger.LogInformation("GetAll dataBridgesFyBw returned {Count} records", records?.Count() ?? 0);
        return OkResponse(records);
    }

    // GET api/v1/dataBridgesFyBw/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById dataBridgesFyBw requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _dataBridgesFyBwService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : (object)t.Result),
            $"No existe dataBridgesFyBw para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById dataBridgesFyBw not found. Id: {Id}", id);
        return result;
    }

}
