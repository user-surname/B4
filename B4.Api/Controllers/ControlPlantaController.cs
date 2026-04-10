using AutoMapper;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.ServiceInterfaces;
using B4.Api.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[CustomAuthorize(Policy = "AdminOnly")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ControlPlantaController : ApiControllerBase
{
    private readonly IControlPlantaService _controlPlantaService;
    private readonly IMapper _mapper;
    private readonly ILogger<ControlPlantaController> _logger;

    public ControlPlantaController(IControlPlantaService controlPlantaService, IMapper mapper, ILogger<ControlPlantaController> logger)
    {
        _controlPlantaService = controlPlantaService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/controlPlanta
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll controlPlanta requested");
        var records = await _controlPlantaService.GetAllAsync();
        _logger.LogInformation("GetAll controlPlanta returned {Count} records", records?.Count() ?? 0);
        return OkResponse(records);
    }

    // GET api/v1/controlPlanta/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById controlPlanta requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _controlPlantaService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : (object)t.Result),
            $"No existe controlPlanta para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById controlPlanta not found. Id: {Id}", id);
        return result;
    }

}
