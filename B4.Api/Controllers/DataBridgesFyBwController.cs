using B4.Api.Middleware;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[CustomAuthorize(Policy = "AdminOnly")]
public class DataBridgesFyBwController : ControllerBase
{
    private readonly IDataBridgesFyBwService _service;

    public DataBridgesFyBwController(IDataBridgesFyBwService service)
    {
        _service = service;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var record = await _service.GetByIdAsync(id);
        return record is null
            ? NotFound($"No existe DataBridgesFyBw con id={id}")
            : Ok(record);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await _service.GetAllAsync();
        return Ok(records);
    }
}
