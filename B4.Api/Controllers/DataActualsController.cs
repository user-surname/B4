using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Api.Middleware;
using B4.Models.Entities.DataEntities;
using B4.Models.Interfaces.DataInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [CustomAuthorize(Policy = "AdminOnly")]
    public class DataActualsController : ControllerBase
    {
        private readonly IDataActualsRepository _actualsRepo;

        public DataActualsController(IDataActualsRepository actualsRepo)
        {
            _actualsRepo = actualsRepo;
        }

        // GET /api/v1/DataActuals/{planta}/{ejercicio}/{epigrafe}
        [HttpGet("{planta:int}/{ejercicio:int}/{epigrafe:int}")]
        public async Task<IActionResult> GetActualsByPlantaEjercicioEpigrafe(int planta, int ejercicio, int epigrafe)
        {
            try
            {
                var record = await _actualsRepo.GetByPlantaEjercicioEpigrafeAsync(planta, ejercicio, epigrafe);

                if (record is null)
                    return NotFound($"No existe actuals para planta={planta}, ejercicio={ejercicio}, epigrafe={epigrafe}");

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
            catch (Exception ex)
            {
                // Si tienes el middleware wrapper, esto igualmente quedará encapsulado
                return BadRequest(ex.Message);
            }
        }

        // POST /api/v1/DataActuals/{planta}/{ejercicio}/{mes}/{tipo}
        [HttpPost("{planta:int}/{ejercicio:int}/{mes:int}/{tipo}")]
        public async Task<IActionResult> PostActualsByPlantaEjercicio(
            int planta, int ejercicio, int mes, string tipo,
            [FromBody] DataActualsPostDto[] nuevosActuals)
        {
            if (nuevosActuals == null || nuevosActuals.Length == 0)
                return BadRequest("El cuerpo de la petición está vacío o no tiene elementos.");

            try
            {
                int idCiclo = 0;
                int idFase = 0;
                int idCurrency = DeterminarMoneda(planta, tipo);

                Guid guidCarga = Guid.NewGuid();
                DateTime fechaActual = DateTime.UtcNow;

                int filasInsertadas = 0;

                foreach (var item in nuevosActuals)
                {
                    var entity = new DataActuals
                    {
                        IdCompany = planta,
                        Ejercicio = ejercicio,
                        IdCiclo = idCiclo,
                        IdFase = idFase,
                        IdCurrency = idCurrency,

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
                        Mes13 = item.Mes13,

                        IdAPICarga = 1,
                        GuidCarga = guidCarga,
                        FechaUltModif = fechaActual
                    };

                    await _actualsRepo.AddAsync(entity);
                    filasInsertadas++;
                }

                return Ok(new { inserted = filasInsertadas });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private string GetCurrencyCode(int idCurrency)
            => idCurrency == 0 ? "PLN" : "EUR";

        private int DeterminarMoneda(int planta, string tipo)
            => tipo.Equals("EUR", StringComparison.OrdinalIgnoreCase) ? 2 : 1;
    }
}
