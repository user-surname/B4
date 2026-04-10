using AutoMapper;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities.LkEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[AllowAnonymous]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CiclosController : ApiControllerBase
{
    private readonly ICiclosService _ciclosService;
    private readonly IMapper _mapper;
    private readonly ILogger<CiclosController> _logger;

    public CiclosController(ICiclosService ciclosService, IMapper mapper, ILogger<CiclosController> logger)
    {
        _ciclosService = ciclosService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/ciclos
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll ciclos requested");
        var records = await _ciclosService.GetAllAsync();
        var dtos = _mapper.Map<List<CiclosGetDto>>(records);
        _logger.LogInformation("GetAll ciclos returned {Count} records", dtos.Count);
        return OkResponse(dtos);
    }

    // GET api/v1/ciclos/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById ciclo requested. Id: {Id}", id);
        // ExecuteAsync mide el tiempo y gestiona el 404 automáticamente
        var result = await ExecuteAsync(
            () => _ciclosService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : _mapper.Map<CiclosGetDto>(t.Result)),
            $"No existe ciclo para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById ciclo not found. Id: {Id}", id);
        return result;
    }

    // POST api/v1/ciclos
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CiclosPostDto dto)
    {
        _logger.LogInformation("Create ciclo requested");

        if (dto is null)
        {
            _logger.LogWarning("Create ciclo rejected: empty body");
            return BadRequestResponse<CiclosGetDto>("El cuerpo de la petición no puede estar vacío");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create ciclo rejected: invalid model state");
            return BadRequestResponse<CiclosGetDto>("Los datos enviados no son válidos");
        }

        var ciclo = _mapper.Map<LkCiclos>(dto);
        await _ciclosService.AddAsync(ciclo);

        _logger.LogInformation("Create ciclo succeeded. IdCiclo: {IdCiclo}", ciclo.IdCiclo);
        var resultDto = _mapper.Map<CiclosGetDto>(ciclo);
        return CreatedResponse(nameof(GetById), new { id = ciclo.IdCiclo }, resultDto);
    }

    // DELETE api/v1/ciclos/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete ciclo requested. Id: {Id}", id);

        // KeyNotFoundException es capturada por GlobalExceptionHandlerMiddleware,
        // pero si quieres un 404 controlado en lugar de un 500, mantenlo aquí.
        var existing = await _ciclosService.GetByIdAsync(id);
        if (existing is null)
        {
            _logger.LogWarning("Delete ciclo not found. Id: {Id}", id);
            return NotFoundResponse<CiclosGetDto>($"No existe ciclo con id={id}");
        }

        await _ciclosService.DeleteAsync(id);
        _logger.LogInformation("Delete ciclo succeeded. Id: {Id}", id);
        return OkResponse(new { id, deleted = true });
    }
}
