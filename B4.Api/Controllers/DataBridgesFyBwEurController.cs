using B4.Api.Middleware;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[CustomAuthorize(Policy = "AdminOnly")]
public class DataBridgesFyBwEurController : ControllerBase
{
    private readonly IDataBridgesFyBwEurService _service;

    public DataBridgesFyBwEurController(IDataBridgesFyBwEurService service)
    {
        _service = service;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var record = await _service.GetByIdAsync(id);
        return record is null
            ? NotFound($"No existe DataBridgesFyBwEur con id={id}")
            : Ok(record);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await _service.GetAllAsync();
        return Ok(records);
    }
}
