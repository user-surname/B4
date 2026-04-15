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
public class PlantCurrencyController : ApiControllerBase
{
    private readonly IPlantCurrencyService _plantCurrencyService;
    private readonly IMapper _mapper;
    private readonly ILogger<PlantCurrencyController> _logger;

    public PlantCurrencyController(IPlantCurrencyService plantCurrencyService, IMapper mapper, ILogger<PlantCurrencyController> logger)
    {
        _plantCurrencyService = plantCurrencyService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/plantCurrency
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll plantCurrency requested");
        var records = await _plantCurrencyService.GetAllAsync();
        var dtos = _mapper.Map<List<PlantCurrencyGetDto>>(records);
        _logger.LogInformation("GetAll plantCurrency returned {Count} records", dtos.Count);
        return OkResponse(dtos);
    }

    // GET api/v1/plantCurrency/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById plantCurrency requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _plantCurrencyService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : _mapper.Map<PlantCurrencyGetDto>(t.Result)),
            $"No existe plantCurrency para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById plantCurrency not found. Id: {Id}", id);
        return result;
    }

    // POST api/v1/plantCurrency
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlantCurrencyPostDto dto)
    {
        _logger.LogInformation("Create plantCurrency requested");

        if (dto is null)
        {
            _logger.LogWarning("Create plantCurrency rejected: empty body");
            return BadRequestResponse<PlantCurrencyGetDto>("El cuerpo de la petición no puede estar vacío");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create plantCurrency rejected: invalid model state");
            return BadRequestResponse<PlantCurrencyGetDto>("Los datos enviados no son válidos");
        }

        var entity = _mapper.Map<LkPlantCurrency>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.IsActive = 1;
        await _plantCurrencyService.AddAsync(entity);

        _logger.LogInformation("Create plantCurrency succeeded. IdCurrency: {IdCurrency}", entity.IdCurrency);
        var resultDto = _mapper.Map<PlantCurrencyGetDto>(entity);
        return CreatedResponse(nameof(GetById), new { id = entity.IdCurrency }, resultDto);
    }
    // DELETE api/v1/plantCurrency/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete plantCurrency requested. Id: {Id}", id);

        var existing = await _plantCurrencyService.GetByIdAsync(id);
        if (existing is null)
        {
            _logger.LogWarning("Delete plantCurrency not found. Id: {Id}", id);
            return NotFoundResponse<PlantCurrencyGetDto>($"No existe plantCurrency con id={id}");
        }

        await _plantCurrencyService.DeleteAsync(id);
        _logger.LogInformation("Delete plantCurrency succeeded. Id: {Id}", id);
        return OkResponse(new { id, deleted = true });
    }
}
