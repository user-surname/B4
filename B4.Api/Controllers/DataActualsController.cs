using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Api.Middleware;
using B4.Models.Entities.DataEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [CustomAuthorize(Policy = "AdminOnly")]
    public class DataActualsController : ControllerBase
    {
        private readonly IDataActualsService _actualsService;

        public DataActualsController(IDataActualsService actualsService)
        {
            _actualsService = actualsService;
        }

        [HttpGet("{planta:int}/{ejercicio:int}/{epigrafe:int}")]
        public async Task<IActionResult> GetActualsByPlantaEjercicioEpigrafe(int planta, int ejercicio, int epigrafe)
        {
            var record = await _actualsService.GetByPlantaEjercicioEpigrafeAsync(planta, ejercicio, epigrafe);

            if (record is null)
                return NotFound($"No existe actuals para planta={planta}, ejercicio={ejercicio}, epigrafe={epigrafe}");

            // Mapping entity -> DTO (API hace de “traductor”)
            var dto = new DataActualsGetDto
            {
                Head = new DataActualsGetDto.HeadDto
                {
                    Timestamp = DateTime.UtcNow,
                    Planta = planta.ToString(),
                    Ejercicio = ejercicio,
                    IdCiclo = record.IdCiclo,
                    IdFase = record.IdFase.ToString(),
                    Moneda = GetCurrencyCode(record.IdCurrency)
                },
                Detail = new List<DataActualsGetDto.DetailDto>
                {
                    new()
                    {
                        IdEpigrafe = record.IdEpigrafe,
                        Valores = new decimal[]
                        {
                            record.Mes00, record.Mes01, record.Mes02, record.Mes03,
                            record.Mes04, record.Mes05, record.Mes06, record.Mes07,
                            record.Mes08, record.Mes09, record.Mes10, record.Mes11,
                            record.Mes12, record.Mes13
                        }
                    }
                }
            };

            return Ok(dto);
        }

        [HttpPost("{planta:int}/{ejercicio:int}/{mes:int}/{tipo}")]
        public async Task<IActionResult> PostActualsByPlantaEjercicio(
            int planta, int ejercicio, int mes, string tipo,
            [FromBody] DataActualsPostDto[] nuevosActuals)
        {
            if (nuevosActuals == null || nuevosActuals.Length == 0)
                return BadRequest("El cuerpo de la petición está vacío o no tiene elementos.");

            // Mapping DTO -> entity (API traduce)
            var entities = nuevosActuals.Select(item => new DataActuals
            {
                IdEpigrafe = item.IdEpigrafe,
                Mes00 = 0,
                Mes01 = item.Mes01,
                Mes02 = item.Mes02,
                Mes03 = item.Mes03,
                Mes04 = item.Mes04,
                Mes05 = item.Mes05,
                Mes06 = item.Mes06,
                Mes07 = item.Mes07,
                Mes08 = item.Mes08,
                Mes09 = item.Mes09,
                Mes10 = item.Mes10,
                Mes11 = item.Mes11,
                Mes12 = item.Mes12,
                Mes13 = item.Mes13
            }).ToList();

            int inserted = await _actualsService.AddRangeAsync(planta, ejercicio, mes, tipo, entities);
            return Ok(new { inserted });
        }

        private static string GetCurrencyCode(int idCurrency)
            => idCurrency == 0 ? "PLN" : "EUR";
    }
}
