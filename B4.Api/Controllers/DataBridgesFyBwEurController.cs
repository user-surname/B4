using AutoMapper;
using B4.Api.Middleware;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[CustomAuthorize(Policy = "AdminOnly")]
public class DataBridgesFyBwEurController : ControllerBase
{
    private readonly IDataBridgesFyBwEurService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<DataBridgesFyBwEurController> _logger;

    public DataBridgesFyBwEurController(IDataBridgesFyBwEurService service, IMapper mapper, ILogger<DataBridgesFyBwEurController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById DataBridgesFyBwEur requested. Id: {Id}", id);
        var record = await _service.GetByIdAsync(id);
        if (record is null)
        {
            _logger.LogWarning("GetById DataBridgesFyBwEur not found. Id: {Id}", id);
            return NotFound($"No existe DataBridgesFyBwEur con id={id}");
        }
        _logger.LogInformation("GetById DataBridgesFyBwEur succeeded. Id: {Id}", id);
        return Ok(record);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll DataBridgesFyBwEur requested");
        var records = await _service.GetAllAsync();
        _logger.LogInformation("GetAll DataBridgesFyBwEur returned records");
        return Ok(records);
    }
}
