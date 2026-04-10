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
public class PlantControllersController : ApiControllerBase
{
    private readonly IPlantControllersService _plantControllersService;
    private readonly IMapper _mapper;
    private readonly ILogger<PlantControllersController> _logger;

    public PlantControllersController(IPlantControllersService plantControllersService, IMapper mapper, ILogger<PlantControllersController> logger)
    {
        _plantControllersService = plantControllersService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/plantControllers
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll plantControllers requested");
        var records = await _plantControllersService.GetAllAsync();
        var dtos = _mapper.Map<List<PlantControllersGetDto>>(records);
        _logger.LogInformation("GetAll plantControllers returned {Count} records", dtos.Count);
        return OkResponse(dtos);
    }

    // GET api/v1/plantControllers/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById plantControllers requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _plantControllersService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : _mapper.Map<PlantControllersGetDto>(t.Result)),
            $"No existe plantControllers para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById plantControllers not found. Id: {Id}", id);
        return result;
    }

    // POST api/v1/plantControllers
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlantControllersPostDto dto)
    {
        _logger.LogInformation("Create plantControllers requested");

        if (dto is null)
        {
            _logger.LogWarning("Create plantControllers rejected: empty body");
            return BadRequestResponse<PlantControllersGetDto>("El cuerpo de la petición no puede estar vacío");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create plantControllers rejected: invalid model state");
            return BadRequestResponse<PlantControllersGetDto>("Los datos enviados no son válidos");
        }

        var entity = _mapper.Map<LkPlantControllers>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.IsActive = 1;
        await _plantControllersService.AddAsync(entity);

        _logger.LogInformation("Create plantControllers succeeded. IdCompanyController: {IdCompanyController}", entity.IdCompanyController);
        var resultDto = _mapper.Map<PlantControllersGetDto>(entity);
        return CreatedResponse(nameof(GetById), new { id = entity.IdCompanyController }, resultDto);
    }
    // DELETE api/v1/plantControllers/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete plantControllers requested. Id: {Id}", id);

        var existing = await _plantControllersService.GetByIdAsync(id);
        if (existing is null)
        {
            _logger.LogWarning("Delete plantControllers not found. Id: {Id}", id);
            return NotFoundResponse<PlantControllersGetDto>($"No existe plantControllers con id={id}");
        }

        await _plantControllersService.DeleteAsync(id);
        _logger.LogInformation("Delete plantControllers succeeded. Id: {Id}", id);
        return OkResponse(new { id, deleted = true });
    }
}
