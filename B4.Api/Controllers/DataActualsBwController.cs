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
public class DataActualsBwController : ApiControllerBase
{
    private readonly IDataActualsBwService _dataActualsBwService;
    private readonly IMapper _mapper;
    private readonly ILogger<DataActualsBwController> _logger;

    public DataActualsBwController(IDataActualsBwService dataActualsBwService, IMapper mapper, ILogger<DataActualsBwController> logger)
    {
        _dataActualsBwService = dataActualsBwService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/dataActualsBw
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll dataActualsBw requested");
        var records = await _dataActualsBwService.GetAllAsync();
        _logger.LogInformation("GetAll dataActualsBw returned {Count} records", records?.Count() ?? 0);
        return OkResponse(records);
    }

    // GET api/v1/dataActualsBw/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById dataActualsBw requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _dataActualsBwService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : (object)t.Result),
            $"No existe dataActualsBw para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById dataActualsBw not found. Id: {Id}", id);
        return result;
    }

}
