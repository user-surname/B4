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
public class PlantCompanyController : ApiControllerBase
{
    private readonly IPlantCompanyService _plantCompanyService;
    private readonly IMapper _mapper;
    private readonly ILogger<PlantCompanyController> _logger;

    public PlantCompanyController(IPlantCompanyService plantCompanyService, IMapper mapper, ILogger<PlantCompanyController> logger)
    {
        _plantCompanyService = plantCompanyService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/plantCompany
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll plantCompany requested");
        var records = await _plantCompanyService.GetAllAsync();
        var dtos = _mapper.Map<List<PlantCompanyGetDto>>(records);
        _logger.LogInformation("GetAll plantCompany returned {Count} records", dtos.Count);
        return OkResponse(dtos);
    }

    // GET api/v1/plantCompany/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById plantCompany requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _plantCompanyService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : _mapper.Map<PlantCompanyGetDto>(t.Result)),
            $"No existe plantCompany para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById plantCompany not found. Id: {Id}", id);
        return result;
    }

    // POST api/v1/plantCompany
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlantCompanyPostDto dto)
    {
        _logger.LogInformation("Create plantCompany requested");

        if (dto is null)
        {
            _logger.LogWarning("Create plantCompany rejected: empty body");
            return BadRequestResponse<PlantCompanyGetDto>("El cuerpo de la petición no puede estar vacío");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create plantCompany rejected: invalid model state");
            return BadRequestResponse<PlantCompanyGetDto>("Los datos enviados no son válidos");
        }

        var entity = _mapper.Map<LkPlantCompany>(dto);
        await _plantCompanyService.AddAsync(entity);

        _logger.LogInformation("Create plantCompany succeeded. IdCompany: {IdCompany}", entity.IdCompany);
        var resultDto = _mapper.Map<PlantCompanyGetDto>(entity);
        return CreatedResponse(nameof(GetById), new { id = entity.IdCompany }, resultDto);
    }
    // DELETE api/v1/plantCompany/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete plantCompany requested. Id: {Id}", id);

        var existing = await _plantCompanyService.GetByIdAsync(id);
        if (existing is null)
        {
            _logger.LogWarning("Delete plantCompany not found. Id: {Id}", id);
            return NotFoundResponse<PlantCompanyGetDto>($"No existe plantCompany con id={id}");
        }

        await _plantCompanyService.DeleteAsync(id);
        _logger.LogInformation("Delete plantCompany succeeded. Id: {Id}", id);
        return OkResponse(new { id, deleted = true });
    }
}
