using AutoMapper;
using B4.Api.Middleware;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[CustomAuthorize(Policy = "AdminOnly")]
public class DataBridgesFyBwController : ControllerBase
{
    private readonly IDataBridgesFyBwService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<DataBridgesFyBwController> _logger;

    public DataBridgesFyBwController(IDataBridgesFyBwService service, IMapper mapper, ILogger<DataBridgesFyBwController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById DataBridgesFyBw requested. Id: {Id}", id);
        var record = await _service.GetByIdAsync(id);
        if (record is null)
        {
            _logger.LogWarning("GetById DataBridgesFyBw not found. Id: {Id}", id);
            return NotFound($"No existe DataBridgesFyBw con id={id}");
        }
        _logger.LogInformation("GetById DataBridgesFyBw succeeded. Id: {Id}", id);
        return Ok(record);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll DataBridgesFyBw requested");
        var records = await _service.GetAllAsync();
        _logger.LogInformation("GetAll DataBridgesFyBw returned records");
        return Ok(records);
    }
}
