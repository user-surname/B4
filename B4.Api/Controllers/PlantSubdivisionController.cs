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
public class PlantSubdivisionController : ApiControllerBase
{
    private readonly IPlantSubdivisionService _plantSubdivisionService;
    private readonly IMapper _mapper;
    private readonly ILogger<PlantSubdivisionController> _logger;

    public PlantSubdivisionController(IPlantSubdivisionService plantSubdivisionService, IMapper mapper, ILogger<PlantSubdivisionController> logger)
    {
        _plantSubdivisionService = plantSubdivisionService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/plantSubdivision
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll plantSubdivision requested");
        var records = await _plantSubdivisionService.GetAllAsync();
        var dtos = _mapper.Map<List<PlantSubdivisionGetDto>>(records);
        _logger.LogInformation("GetAll plantSubdivision returned {Count} records", dtos.Count);
        return OkResponse(dtos);
    }

    // GET api/v1/plantSubdivision/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById plantSubdivision requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _plantSubdivisionService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : _mapper.Map<PlantSubdivisionGetDto>(t.Result)),
            $"No existe plantSubdivision para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById plantSubdivision not found. Id: {Id}", id);
        return result;
    }

    // POST api/v1/plantSubdivision
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlantSubdivisionPostDto dto)
    {
        _logger.LogInformation("Create plantSubdivision requested");

        if (dto is null)
        {
            _logger.LogWarning("Create plantSubdivision rejected: empty body");
            return BadRequestResponse<PlantSubdivisionGetDto>("El cuerpo de la petición no puede estar vacío");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create plantSubdivision rejected: invalid model state");
            return BadRequestResponse<PlantSubdivisionGetDto>("Los datos enviados no son válidos");
        }

        var entity = _mapper.Map<LkPlantSubdivision>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.IsActive = 1;
        await _plantSubdivisionService.AddAsync(entity);

        _logger.LogInformation("Create plantSubdivision succeeded. IdSubdivision: {IdSubdivision}", entity.IdSubdivision);
        var resultDto = _mapper.Map<PlantSubdivisionGetDto>(entity);
        return CreatedResponse(nameof(GetById), new { id = entity.IdSubdivision }, resultDto);
    }
    // DELETE api/v1/plantSubdivision/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete plantSubdivision requested. Id: {Id}", id);

        var existing = await _plantSubdivisionService.GetByIdAsync(id);
        if (existing is null)
        {
            _logger.LogWarning("Delete plantSubdivision not found. Id: {Id}", id);
            return NotFoundResponse<PlantSubdivisionGetDto>($"No existe plantSubdivision con id={id}");
        }

        await _plantSubdivisionService.DeleteAsync(id);
        _logger.LogInformation("Delete plantSubdivision succeeded. Id: {Id}", id);
        return OkResponse(new { id, deleted = true });
    }
}
