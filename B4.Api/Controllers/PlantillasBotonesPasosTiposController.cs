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
public class PlantillasBotonesPasosTiposController : ApiControllerBase
{
    private readonly IPlantillasBotonesPasosTiposService _plantillasBotonesPasosTiposService;
    private readonly IMapper _mapper;
    private readonly ILogger<PlantillasBotonesPasosTiposController> _logger;

    public PlantillasBotonesPasosTiposController(IPlantillasBotonesPasosTiposService plantillasBotonesPasosTiposService, IMapper mapper, ILogger<PlantillasBotonesPasosTiposController> logger)
    {
        _plantillasBotonesPasosTiposService = plantillasBotonesPasosTiposService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/plantillasBotonesPasosTipos
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll plantillasBotonesPasosTipos requested");
        var records = await _plantillasBotonesPasosTiposService.GetAllAsync();
        var dtos = _mapper.Map<List<PlantillasBotonesPasosTiposGetDto>>(records);
        _logger.LogInformation("GetAll plantillasBotonesPasosTipos returned {Count} records", dtos.Count);
        return OkResponse(dtos);
    }

    // GET api/v1/plantillasBotonesPasosTipos/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById plantillasBotonesPasosTipos requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _plantillasBotonesPasosTiposService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : _mapper.Map<PlantillasBotonesPasosTiposGetDto>(t.Result)),
            $"No existe plantillasBotonesPasosTipos para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById plantillasBotonesPasosTipos not found. Id: {Id}", id);
        return result;
    }

    // POST api/v1/plantillasBotonesPasosTipos
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlantillasBotonesPasosTiposPostDto dto)
    {
        _logger.LogInformation("Create plantillasBotonesPasosTipos requested");

        if (dto is null)
        {
            _logger.LogWarning("Create plantillasBotonesPasosTipos rejected: empty body");
            return BadRequestResponse<PlantillasBotonesPasosTiposGetDto>("El cuerpo de la petición no puede estar vacío");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create plantillasBotonesPasosTipos rejected: invalid model state");
            return BadRequestResponse<PlantillasBotonesPasosTiposGetDto>("Los datos enviados no son válidos");
        }

        var entity = _mapper.Map<LkPlantillasBotonesPasosTipos>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.IsActive = 1;
        await _plantillasBotonesPasosTiposService.AddAsync(entity);

        _logger.LogInformation("Create plantillasBotonesPasosTipos succeeded. IdPasoTipo: {IdPasoTipo}", entity.IdPasoTipo);
        var resultDto = _mapper.Map<PlantillasBotonesPasosTiposGetDto>(entity);
        return CreatedResponse(nameof(GetById), new { id = entity.IdPasoTipo }, resultDto);
    }
    // DELETE api/v1/plantillasBotonesPasosTipos/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete plantillasBotonesPasosTipos requested. Id: {Id}", id);

        var existing = await _plantillasBotonesPasosTiposService.GetByIdAsync(id);
        if (existing is null)
        {
            _logger.LogWarning("Delete plantillasBotonesPasosTipos not found. Id: {Id}", id);
            return NotFoundResponse<PlantillasBotonesPasosTiposGetDto>($"No existe plantillasBotonesPasosTipos con id={id}");
        }

        await _plantillasBotonesPasosTiposService.DeleteAsync(id);
        _logger.LogInformation("Delete plantillasBotonesPasosTipos succeeded. Id: {Id}", id);
        return OkResponse(new { id, deleted = true });
    }
}
