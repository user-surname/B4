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
public class PlantDivisionCompanyController : ApiControllerBase
{
    private readonly IPlantDivisionCompanyService _plantDivisionCompanyService;
    private readonly IMapper _mapper;
    private readonly ILogger<PlantDivisionCompanyController> _logger;

    public PlantDivisionCompanyController(IPlantDivisionCompanyService plantDivisionCompanyService, IMapper mapper, ILogger<PlantDivisionCompanyController> logger)
    {
        _plantDivisionCompanyService = plantDivisionCompanyService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/plantDivisionCompany
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll plantDivisionCompany requested");
        var records = await _plantDivisionCompanyService.GetAllAsync();
        var dtos = _mapper.Map<List<PlantDivisionCompanyGetDto>>(records);
        _logger.LogInformation("GetAll plantDivisionCompany returned {Count} records", dtos.Count);
        return OkResponse(dtos);
    }

    // GET api/v1/plantDivisionCompany/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById plantDivisionCompany requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _plantDivisionCompanyService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : _mapper.Map<PlantDivisionCompanyGetDto>(t.Result)),
            $"No existe plantDivisionCompany para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById plantDivisionCompany not found. Id: {Id}", id);
        return result;
    }

    // POST api/v1/plantDivisionCompany
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlantDivisionCompanyPostDto dto)
    {
        _logger.LogInformation("Create plantDivisionCompany requested");

        if (dto is null)
        {
            _logger.LogWarning("Create plantDivisionCompany rejected: empty body");
            return BadRequestResponse<PlantDivisionCompanyGetDto>("El cuerpo de la petición no puede estar vacío");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create plantDivisionCompany rejected: invalid model state");
            return BadRequestResponse<PlantDivisionCompanyGetDto>("Los datos enviados no son válidos");
        }

        var entity = _mapper.Map<LkPlantDivisionCompany>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.IsActive = 1;
        await _plantDivisionCompanyService.AddAsync(entity);

        _logger.LogInformation("Create plantDivisionCompany succeeded. IdDivisionCompany: {IdDivisionCompany}", entity.IdDivisionCompany);
        var resultDto = _mapper.Map<PlantDivisionCompanyGetDto>(entity);
        return CreatedResponse(nameof(GetById), new { id = entity.IdDivisionCompany }, resultDto);
    }
    // DELETE api/v1/plantDivisionCompany/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete plantDivisionCompany requested. Id: {Id}", id);

        var existing = await _plantDivisionCompanyService.GetByIdAsync(id);
        if (existing is null)
        {
            _logger.LogWarning("Delete plantDivisionCompany not found. Id: {Id}", id);
            return NotFoundResponse<PlantDivisionCompanyGetDto>($"No existe plantDivisionCompany con id={id}");
        }

        await _plantDivisionCompanyService.DeleteAsync(id);
        _logger.LogInformation("Delete plantDivisionCompany succeeded. Id: {Id}", id);
        return OkResponse(new { id, deleted = true });
    }
}
