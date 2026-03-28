using AutoMapper;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Api.Middleware;
using B4.Models.Entities.DataEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [CustomAuthorize(Policy = "AdminOnly")]
    public class DataActualsController : ControllerBase
    {
        private readonly IDataActualsService _actualsService;
        private readonly IMapper _mapper;
        private readonly ILogger<DataActualsController> _logger;

        public DataActualsController(IDataActualsService actualsService, IMapper mapper, ILogger<DataActualsController> logger)
        {
            _actualsService = actualsService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{planta:int}/{ejercicio:int}/{epigrafe:int}")]
        public async Task<IActionResult> GetActualsByPlantaEjercicioEpigrafe(int planta, int ejercicio, int epigrafe)
        {
            _logger.LogInformation("GetActuals requested. Planta: {Planta}, Ejercicio: {Ejercicio}, Epigrafe: {Epigrafe}", planta, ejercicio, epigrafe);
            var record = await _actualsService.GetByPlantaEjercicioEpigrafeAsync(planta, ejercicio, epigrafe);

            if (record is null)
            {
                _logger.LogWarning("GetActuals not found. Planta: {Planta}, Ejercicio: {Ejercicio}, Epigrafe: {Epigrafe}", planta, ejercicio, epigrafe);
                return NotFound($"No existe actuals para planta={planta}, ejercicio={ejercicio}, epigrafe={epigrafe}");
            }

            var dto = _mapper.Map<DataActualsGetDto>(record, opt =>
            {
                opt.Items["planta"] = planta;
                opt.Items["ejercicio"] = ejercicio;
            });

            _logger.LogInformation("GetActuals succeeded. Planta: {Planta}, Ejercicio: {Ejercicio}, Epigrafe: {Epigrafe}", planta, ejercicio, epigrafe);
            return Ok(dto);
        }

        [HttpPost("{planta:int}/{ejercicio:int}/{mes:int}/{tipo}")]
        public async Task<IActionResult> PostActualsByPlantaEjercicio(
            int planta, int ejercicio, int mes, string tipo,
            [FromBody] DataActualsPostDto[] nuevosActuals)
        {
            _logger.LogInformation("PostActuals requested. Planta: {Planta}, Ejercicio: {Ejercicio}, Mes: {Mes}, Tipo: {Tipo}", planta, ejercicio, mes, tipo);

            if (nuevosActuals == null || nuevosActuals.Length == 0)
            {
                _logger.LogWarning("PostActuals rejected: empty body. Planta: {Planta}, Ejercicio: {Ejercicio}", planta, ejercicio);
                return BadRequest("El cuerpo de la peticion esta vacio o no tiene elementos.");
            }

            var entities = _mapper.Map<List<DataActuals>>(nuevosActuals);
            int inserted = await _actualsService.AddRangeAsync(planta, ejercicio, mes, tipo, entities);

            _logger.LogInformation("PostActuals succeeded. Inserted: {Inserted}. Planta: {Planta}, Ejercicio: {Ejercicio}", inserted, planta, ejercicio);
            return Ok(new { inserted });
        }
    }
}
