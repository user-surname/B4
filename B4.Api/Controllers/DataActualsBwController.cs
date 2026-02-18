using AutoMapper;
using B4.Api.Middleware;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[CustomAuthorize(Policy = "AdminOnly")]
public class DataActualsBwController : ControllerBase
{
    private readonly IDataActualsBwService _service;
    private readonly IMapper _mapper;

    public DataActualsBwController(IDataActualsBwService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var record = await _service.GetByIdAsync(id);

        return record is null
            ? NotFound($"No existe DataActualsBw con id={id}")
            : Ok(record); // por ahora devolvemos entidad; cuando tengamos DTO, mapeamos aquí
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await _service.GetAllAsync();
        return Ok(records); // idem
    }
}
