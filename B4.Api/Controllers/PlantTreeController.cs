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
public class PlantTreeController : ApiControllerBase
{
    private readonly IPlantTreeService _plantTreeService;
    private readonly IMapper _mapper;
    private readonly ILogger<PlantTreeController> _logger;

    public PlantTreeController(IPlantTreeService plantTreeService, IMapper mapper, ILogger<PlantTreeController> logger)
    {
        _plantTreeService = plantTreeService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/plantTree
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll plantTree requested");
        var records = await _plantTreeService.GetAllAsync();
        var dtos = _mapper.Map<List<PlantTreeGetDto>>(records);
        _logger.LogInformation("GetAll plantTree returned {Count} records", dtos.Count);
        return OkResponse(dtos);
    }

    // GET api/v1/plantTree/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById plantTree requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _plantTreeService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : _mapper.Map<PlantTreeGetDto>(t.Result)),
            $"No existe plantTree para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById plantTree not found. Id: {Id}", id);
        return result;
    }

    // POST api/v1/plantTree
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlantTreePostDto dto)
    {
        _logger.LogInformation("Create plantTree requested");

        if (dto is null)
        {
            _logger.LogWarning("Create plantTree rejected: empty body");
            return BadRequestResponse<PlantTreeGetDto>("El cuerpo de la petición no puede estar vacío");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create plantTree rejected: invalid model state");
            return BadRequestResponse<PlantTreeGetDto>("Los datos enviados no son válidos");
        }

        var entity = _mapper.Map<LkPlantTree>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.IsActive = 1;
        await _plantTreeService.AddAsync(entity);

        _logger.LogInformation("Create plantTree succeeded. IdTree: {IdTree}", entity.IdTree);
        var resultDto = _mapper.Map<PlantTreeGetDto>(entity);
        return CreatedResponse(nameof(GetById), new { id = entity.IdTree }, resultDto);
    }
    // DELETE api/v1/plantTree/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete plantTree requested. Id: {Id}", id);

        var existing = await _plantTreeService.GetByIdAsync(id);
        if (existing is null)
        {
            _logger.LogWarning("Delete plantTree not found. Id: {Id}", id);
            return NotFoundResponse<PlantTreeGetDto>($"No existe plantTree con id={id}");
        }

        await _plantTreeService.DeleteAsync(id);
        _logger.LogInformation("Delete plantTree succeeded. Id: {Id}", id);
        return OkResponse(new { id, deleted = true });
    }
}
