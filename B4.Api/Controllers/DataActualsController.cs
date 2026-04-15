using AutoMapper;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Api.Middleware;
using B4.Models.Entities.DataEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[CustomAuthorize(Policy = "AdminOnly")]
public class DataActualsController : ApiControllerBase
{
    private readonly IDataActualsService _actualsService;
    private readonly IMapper _mapper;
    private readonly ILogger<DataActualsController> _logger;

    public DataActualsController(
        IDataActualsService actualsService,
        IMapper mapper,
        ILogger<DataActualsController> logger)
    {
        _actualsService = actualsService;
        _mapper = mapper;
        _logger = logger;
    }

    // -------------------------------------------------
    // GET actuals por planta, ejercicio y epígrafe
    // -------------------------------------------------
    [HttpGet("{planta:int}/{ejercicio:int}/{epigrafe:int}")]
    public async Task<IActionResult> GetActualsByPlantaEjercicioEpigrafe(
        int planta, int ejercicio, int epigrafe)
    {

        var result = await ExecuteAsync(
            () => _actualsService
                .GetByPlantaEjercicioEpigrafeAsync(planta, ejercicio, epigrafe)
                .ContinueWith(t =>
                    t.Result is null
                        ? null
                        : _mapper.Map<DataActualsGetDto>(t.Result, opt =>
                        {
                            opt.Items["planta"] = planta;
                            opt.Items["ejercicio"] = ejercicio;
                        })
                ),
            $"No existen actuals para planta={planta}, ejercicio={ejercicio}, epigrafe={epigrafe}"
        );

        return result;
    }

    // -------------------------------------------------
    // POST actuals por planta y ejercicio
    // -------------------------------------------------
    [HttpPost("{planta:int}/{ejercicio:int}/{mes:int}/{tipo}")]
    public async Task<IActionResult> PostActualsByPlantaEjercicio(
        int planta,
        int ejercicio,
        int mes,
        string tipo,
        [FromBody] DataActualsPostDto[] nuevosActuals)
    {

        if (nuevosActuals is null || nuevosActuals.Length == 0)
        {
            return BadRequestResponse<object>(
                "El cuerpo de la petición está vacío o no contiene elementos");
        }

        if (!ModelState.IsValid)
        {
            return BadRequestResponse<object>(
                "Los datos enviados no son válidos");
        }

        var entities = _mapper.Map<List<DataActuals>>(nuevosActuals);

        int inserted = await _actualsService.AddRangeAsync(
            planta, ejercicio, mes, tipo, entities);

        return OkResponse(new InsertResultDto
        {
            Inserted = inserted
        });
    }
}