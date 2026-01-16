using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities.DataEntities;
using B4.Models.Interfaces.DataInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
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
                // ✅ método que añadiste en la interfaz
                var record = await _actualsRepo.GetByPlantaEjercicioEpigrafeAsync(planta, ejercicio, epigrafe);

                if (record is null)
                    return NotFound($"No existe actuals para planta={planta}, ejercicio={ejercicio}, epigrafe={epigrafe}");

                var head = new DataActualsGetDto.HeadDto
                {
                    ts = DateTime.UtcNow,                 // tu DTO lo tiene como DateTime
                    p = planta.ToString(),
                    e = ejercicio,
                    c = record.IdCiclo,
                    f = record.IdFase.ToString(),
                    m = GetCurrencyCode(record.IdCurrency)
                };

                var detailList = new List<DataActualsGetDto.DetailDto>
                {
                    new DataActualsGetDto.DetailDto
                    {
                        e = record.IdEpigrafe,
                        v = new decimal[]
                        {
                            record.Mes00, record.Mes01, record.Mes02, record.Mes03,
                            record.Mes04, record.Mes05, record.Mes06, record.Mes07,
                            record.Mes08, record.Mes09, record.Mes10, record.Mes11,
                            record.Mes12, record.Mes13
                        }
                    }
                };

                var dataDto = new DataActualsGetDto { head = head, detail = detailList };
                return Ok(dataDto);
            }
            catch (Exception ex)
            {
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
                        IdEpigrafe = item.e,

                        Mes00 = 0,
                        Mes01 = item.m1, Mes02 = item.m2, Mes03 = item.m3, Mes04 = item.m4,
                        Mes05 = item.m5, Mes06 = item.m6, Mes07 = item.m7, Mes08 = item.m8,
                        Mes09 = item.m9, Mes10 = item.m10, Mes11 = item.m11, Mes12 = item.m12,
                        Mes13 = item.m13,

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
