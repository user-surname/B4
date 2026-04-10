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
public class EpigrafeController : ApiControllerBase
{
    private readonly IEpigrafeService _epigrafeService;
    private readonly IMapper _mapper;
    private readonly ILogger<EpigrafeController> _logger;

    public EpigrafeController(IEpigrafeService epigrafeService, IMapper mapper, ILogger<EpigrafeController> logger)
    {
        _epigrafeService = epigrafeService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/epigrafe
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll epigrafe requested");
        var records = await _epigrafeService.GetAllAsync();
        var dtos = _mapper.Map<List<EpigrafeGetDto>>(records);
        _logger.LogInformation("GetAll epigrafe returned {Count} records", dtos.Count);
        return OkResponse(dtos);
    }

    // GET api/v1/epigrafe/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById epigrafe requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _epigrafeService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : _mapper.Map<EpigrafeGetDto>(t.Result)),
            $"No existe epigrafe para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById epigrafe not found. Id: {Id}", id);
        return result;
    }

    // POST api/v1/epigrafe
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EpigrafePostDto dto)
    {
        _logger.LogInformation("Create epigrafe requested");

        if (dto is null)
        {
            _logger.LogWarning("Create epigrafe rejected: empty body");
            return BadRequestResponse<EpigrafeGetDto>("El cuerpo de la petición no puede estar vacío");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create epigrafe rejected: invalid model state");
            return BadRequestResponse<EpigrafeGetDto>("Los datos enviados no son válidos");
        }

        var entity = _mapper.Map<LkEpigrafe>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.IsActive = 1;
        await _epigrafeService.AddAsync(entity);

        _logger.LogInformation("Create epigrafe succeeded. IdEpigrafe: {IdEpigrafe}", entity.IdEpigrafe);
        var resultDto = _mapper.Map<EpigrafeGetDto>(entity);
        return CreatedResponse(nameof(GetById), new { id = entity.IdEpigrafe }, resultDto);
    }
    // DELETE api/v1/epigrafe/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete epigrafe requested. Id: {Id}", id);

        var existing = await _epigrafeService.GetByIdAsync(id);
        if (existing is null)
        {
            _logger.LogWarning("Delete epigrafe not found. Id: {Id}", id);
            return NotFoundResponse<EpigrafeGetDto>($"No existe epigrafe con id={id}");
        }

        await _epigrafeService.DeleteAsync(id);
        _logger.LogInformation("Delete epigrafe succeeded. Id: {Id}", id);
        return OkResponse(new { id, deleted = true });
    }
}
