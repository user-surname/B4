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
public class DataBudgetBwController : ApiControllerBase
{
    private readonly IDataBudgetBwService _dataBudgetBwService;
    private readonly IMapper _mapper;
    private readonly ILogger<DataBudgetBwController> _logger;

    public DataBudgetBwController(IDataBudgetBwService dataBudgetBwService, IMapper mapper, ILogger<DataBudgetBwController> logger)
    {
        _dataBudgetBwService = dataBudgetBwService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/dataBudgetBw
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll dataBudgetBw requested");
        var records = await _dataBudgetBwService.GetAllAsync();
        _logger.LogInformation("GetAll dataBudgetBw returned {Count} records", records?.Count() ?? 0);
        return OkResponse(records);
    }

    // GET api/v1/dataBudgetBw/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById dataBudgetBw requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _dataBudgetBwService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : (object)t.Result),
            $"No existe dataBudgetBw para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById dataBudgetBw not found. Id: {Id}", id);
        return result;
    }

}
