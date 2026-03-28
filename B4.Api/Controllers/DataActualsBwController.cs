using AutoMapper;
using B4.Api.Middleware;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[CustomAuthorize(Policy = "AdminOnly")]
public class DataActualsBwController : ControllerBase
{
    private readonly IDataActualsBwService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<DataActualsBwController> _logger;

    public DataActualsBwController(IDataActualsBwService service, IMapper mapper, ILogger<DataActualsBwController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById DataActualsBw requested. Id: {Id}", id);
        var record = await _service.GetByIdAsync(id);

        if (record is null)
        {
            _logger.LogWarning("GetById DataActualsBw not found. Id: {Id}", id);
            return NotFound($"No existe DataActualsBw con id={id}");
        }

        _logger.LogInformation("GetById DataActualsBw succeeded. Id: {Id}", id);
        return Ok(record);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll DataActualsBw requested");
        var records = await _service.GetAllAsync();
        _logger.LogInformation("GetAll DataActualsBw returned {Count} records", records?.Count() ?? 0);
        return Ok(records);
    }
}
