using AutoMapper;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[AllowAnonymous]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ControlController : ApiControllerBase
{
    private readonly IControlService _controlService;
    private readonly IMapper _mapper;
    private readonly ILogger<ControlController> _logger;

    public ControlController(IControlService controlService, IMapper mapper, ILogger<ControlController> logger)
    {
        _controlService = controlService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/control
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll controls requested");

        var records = await _controlService.GetAllAsync();
        var dtos = _mapper.Map<List<ControlGetDto>>(records);

        _logger.LogInformation("GetAll controls returned {Count} records", dtos.Count);

        return OkResponse(dtos);
    }

    // GET api/v1/control/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById control requested. Id: {Id}", id);

        var result = await ExecuteAsync(
            () => _controlService.GetByIdAsync(id)
                .ContinueWith(t => t.Result is null ? null : _mapper.Map<ControlGetDto>(t.Result)),
            $"No existe control para id={id}"
        );

        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById control not found. Id: {Id}", id);

        return result;
    }

    // POST api/v1/control
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ControlPostDto dto)
    {
        _logger.LogInformation("Create control requested");

        if (dto is null)
        {
            _logger.LogWarning("Create control rejected: empty body");
            return BadRequestResponse<ControlGetDto>("El cuerpo de la petición no puede estar vacío");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create control rejected: invalid model state");
            return BadRequestResponse<ControlGetDto>("Los datos enviados no son válidos");
        }

        var control = _mapper.Map<Control>(dto);
        await _controlService.AddAsync(control);

        _logger.LogInformation("Create control succeeded. IdControl: {IdControl}", control.IdControl);

        var resultDto = _mapper.Map<ControlGetDto>(control);

        return CreatedResponse(nameof(GetById), new { id = control.IdControl }, resultDto);
    }

    // DELETE api/v1/control/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete control requested. Id: {Id}", id);

        var existing = await _controlService.GetByIdAsync(id);

        if (existing is null)
        {
            _logger.LogWarning("Delete control not found. Id: {Id}", id);
            return NotFoundResponse<ControlGetDto>($"No existe control con id={id}");
        }

        await _controlService.DeleteAsync(id);

        _logger.LogInformation("Delete control succeeded. Id: {Id}", id);

        return OkResponse(new { id, deleted = true });
    }
}