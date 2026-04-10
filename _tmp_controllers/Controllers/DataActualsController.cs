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

        public DataActualsController(IDataActualsService actualsService, IMapper mapper)
        {
            _actualsService = actualsService;
            _mapper = mapper;
        }

        [HttpGet("{planta:int}/{ejercicio:int}/{epigrafe:int}")]
        public async Task<IActionResult> GetActualsByPlantaEjercicioEpigrafe(int planta, int ejercicio, int epigrafe)
        {
            var record = await _actualsService.GetByPlantaEjercicioEpigrafeAsync(planta, ejercicio, epigrafe);

            if (record is null)
                return NotFound($"No existe actuals para planta={planta}, ejercicio={ejercicio}, epigrafe={epigrafe}");

            // ✅ Entity -> DTO con AutoMapper (pasando parámetros de ruta para completar "Head")
            var dto = _mapper.Map<DataActualsGetDto>(record, opt =>
            {
                opt.Items["planta"] = planta;
                opt.Items["ejercicio"] = ejercicio;
            });

            return Ok(dto);
        }

        [HttpPost("{planta:int}/{ejercicio:int}/{mes:int}/{tipo}")]
        public async Task<IActionResult> PostActualsByPlantaEjercicio(
            int planta, int ejercicio, int mes, string tipo,
            [FromBody] DataActualsPostDto[] nuevosActuals)
        {
            if (nuevosActuals == null || nuevosActuals.Length == 0)
                return BadRequest("El cuerpo de la petición está vacío o no tiene elementos.");

            // ✅ DTO -> Entity con AutoMapper (lista)
            var entities = _mapper.Map<List<DataActuals>>(nuevosActuals);

            int inserted = await _actualsService.AddRangeAsync(planta, ejercicio, mes, tipo, entities);
            return Ok(new { inserted });
        }
    }
}
