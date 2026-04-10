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
public class FasesController : ApiControllerBase
{
    private readonly IFasesService _fasesService;
    private readonly IMapper _mapper;
    private readonly ILogger<FasesController> _logger;

    public FasesController(IFasesService fasesService, IMapper mapper, ILogger<FasesController> logger)
    {
        _fasesService = fasesService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/fases
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll fases requested");
        var records = await _fasesService.GetAllAsync();
        var dtos = _mapper.Map<List<FasesGetDto>>(records);
        _logger.LogInformation("GetAll fases returned {Count} records", dtos.Count);
        return OkResponse(dtos);
    }

    // GET api/v1/fases/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById fases requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _fasesService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : _mapper.Map<FasesGetDto>(t.Result)),
            $"No existe fases para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById fases not found. Id: {Id}", id);
        return result;
    }

    // POST api/v1/fases
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FasesPostDto dto)
    {
        _logger.LogInformation("Create fases requested");

        if (dto is null)
        {
            _logger.LogWarning("Create fases rejected: empty body");
            return BadRequestResponse<FasesGetDto>("El cuerpo de la petición no puede estar vacío");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create fases rejected: invalid model state");
            return BadRequestResponse<FasesGetDto>("Los datos enviados no son válidos");
        }

        var entity = _mapper.Map<LkFases>(dto);
        await _fasesService.AddAsync(entity);

        _logger.LogInformation("Create fases succeeded. IdFase: {IdFase}", entity.IdFase);
        var resultDto = _mapper.Map<FasesGetDto>(entity);
        return CreatedResponse(nameof(GetById), new { id = entity.IdFase }, resultDto);
    }
    // DELETE api/v1/fases/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete fases requested. Id: {Id}", id);

        var existing = await _fasesService.GetByIdAsync(id);
        if (existing is null)
        {
            _logger.LogWarning("Delete fases not found. Id: {Id}", id);
            return NotFoundResponse<FasesGetDto>($"No existe fases con id={id}");
        }

        await _fasesService.DeleteAsync(id);
        _logger.LogInformation("Delete fases succeeded. Id: {Id}", id);
        return OkResponse(new { id, deleted = true });
    }
}
