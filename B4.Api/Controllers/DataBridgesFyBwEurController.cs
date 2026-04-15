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
public class DataBridgesFyBwEurController : ApiControllerBase
{
    private readonly IDataBridgesFyBwEurService _dataBridgesFyBwEurService;
    private readonly IMapper _mapper;
    private readonly ILogger<DataBridgesFyBwEurController> _logger;

    public DataBridgesFyBwEurController(IDataBridgesFyBwEurService dataBridgesFyBwEurService, IMapper mapper, ILogger<DataBridgesFyBwEurController> logger)
    {
        _dataBridgesFyBwEurService = dataBridgesFyBwEurService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/dataBridgesFyBwEur
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll dataBridgesFyBwEur requested");
        var records = await _dataBridgesFyBwEurService.GetAllAsync();
        _logger.LogInformation("GetAll dataBridgesFyBwEur returned {Count} records", records?.Count() ?? 0);
        return OkResponse(records);
    }

    // GET api/v1/dataBridgesFyBwEur/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById dataBridgesFyBwEur requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _dataBridgesFyBwEurService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : (object)t.Result),
            $"No existe dataBridgesFyBwEur para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById dataBridgesFyBwEur not found. Id: {Id}", id);
        return result;
    }

}
