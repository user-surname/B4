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
public class PlantCountryController : ApiControllerBase
{
    private readonly IPlantCountryService _plantCountryService;
    private readonly IMapper _mapper;
    private readonly ILogger<PlantCountryController> _logger;

    public PlantCountryController(IPlantCountryService plantCountryService, IMapper mapper, ILogger<PlantCountryController> logger)
    {
        _plantCountryService = plantCountryService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/plantCountry
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll plantCountry requested");
        var records = await _plantCountryService.GetAllAsync();
        var dtos = _mapper.Map<List<PlantCountryGetDto>>(records);
        _logger.LogInformation("GetAll plantCountry returned {Count} records", dtos.Count);
        return OkResponse(dtos);
    }

    // GET api/v1/plantCountry/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById plantCountry requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _plantCountryService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : _mapper.Map<PlantCountryGetDto>(t.Result)),
            $"No existe plantCountry para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById plantCountry not found. Id: {Id}", id);
        return result;
    }

    // POST api/v1/plantCountry
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlantCountryPostDto dto)
    {
        _logger.LogInformation("Create plantCountry requested");

        if (dto is null)
        {
            _logger.LogWarning("Create plantCountry rejected: empty body");
            return BadRequestResponse<PlantCountryGetDto>("El cuerpo de la petición no puede estar vacío");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create plantCountry rejected: invalid model state");
            return BadRequestResponse<PlantCountryGetDto>("Los datos enviados no son válidos");
        }

        var entity = _mapper.Map<LkPlantCountry>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.IsActive = 1;
        await _plantCountryService.AddAsync(entity);

        _logger.LogInformation("Create plantCountry succeeded. IdCountry: {IdCountry}", entity.IdCountry);
        var resultDto = _mapper.Map<PlantCountryGetDto>(entity);
        return CreatedResponse(nameof(GetById), new { id = entity.IdCountry }, resultDto);
    }
    // DELETE api/v1/plantCountry/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete plantCountry requested. Id: {Id}", id);

        var existing = await _plantCountryService.GetByIdAsync(id);
        if (existing is null)
        {
            _logger.LogWarning("Delete plantCountry not found. Id: {Id}", id);
            return NotFoundResponse<PlantCountryGetDto>($"No existe plantCountry con id={id}");
        }

        await _plantCountryService.DeleteAsync(id);
        _logger.LogInformation("Delete plantCountry succeeded. Id: {Id}", id);
        return OkResponse(new { id, deleted = true });
    }
}
