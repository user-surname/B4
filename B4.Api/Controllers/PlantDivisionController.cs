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
public class PlantDivisionController : ApiControllerBase
{
    private readonly IPlantDivisionService _plantDivisionService;
    private readonly IMapper _mapper;
    private readonly ILogger<PlantDivisionController> _logger;

    public PlantDivisionController(IPlantDivisionService plantDivisionService, IMapper mapper, ILogger<PlantDivisionController> logger)
    {
        _plantDivisionService = plantDivisionService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/plantDivision
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll plantDivision requested");
        var records = await _plantDivisionService.GetAllAsync();
        var dtos = _mapper.Map<List<PlantDivisionGetDto>>(records);
        _logger.LogInformation("GetAll plantDivision returned {Count} records", dtos.Count);
        return OkResponse(dtos);
    }

    // GET api/v1/plantDivision/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById plantDivision requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _plantDivisionService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : _mapper.Map<PlantDivisionGetDto>(t.Result)),
            $"No existe plantDivision para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById plantDivision not found. Id: {Id}", id);
        return result;
    }

    // POST api/v1/plantDivision
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlantDivisionPostDto dto)
    {
        _logger.LogInformation("Create plantDivision requested");

        if (dto is null)
        {
            _logger.LogWarning("Create plantDivision rejected: empty body");
            return BadRequestResponse<PlantDivisionGetDto>("El cuerpo de la petición no puede estar vacío");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create plantDivision rejected: invalid model state");
            return BadRequestResponse<PlantDivisionGetDto>("Los datos enviados no son válidos");
        }

        var entity = _mapper.Map<LkPlantDivision>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.IsActive = 1;
        await _plantDivisionService.AddAsync(entity);

        _logger.LogInformation("Create plantDivision succeeded. IdDivision: {IdDivision}", entity.IdDivision);
        var resultDto = _mapper.Map<PlantDivisionGetDto>(entity);
        return CreatedResponse(nameof(GetById), new { id = entity.IdDivision }, resultDto);
    }
    // DELETE api/v1/plantDivision/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete plantDivision requested. Id: {Id}", id);

        var existing = await _plantDivisionService.GetByIdAsync(id);
        if (existing is null)
        {
            _logger.LogWarning("Delete plantDivision not found. Id: {Id}", id);
            return NotFoundResponse<PlantDivisionGetDto>($"No existe plantDivision con id={id}");
        }

        await _plantDivisionService.DeleteAsync(id);
        _logger.LogInformation("Delete plantDivision succeeded. Id: {Id}", id);
        return OkResponse(new { id, deleted = true });
    }
}
