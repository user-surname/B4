using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Api.Middleware;
using B4.Models.Entities.DataEntities;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.DataInterfaces;
using B4.Models.Interfaces.LkInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CilcosController : ControllerBase
    {
        private readonly ICiclosRepository _ciclosRepo;

        public CilcosController(ICiclosRepository actualsRepo)
        {
            _ciclosRepo = actualsRepo;
        }

        // GET /api/v1/DataActuals/{id}}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var record = await _ciclosRepo.GetByIdAsync(id);

                if (record is null)
                    return NotFound($"No existe ciclo para id={id}");

                var dto = new CiclosGetDto
                {
                    CreatedAt = record.CreatedAt,
                    UpdatedAt = record.UpdatedAt,
                    IsActive = record.IsActive,
                    IdCiclo = record.IdCiclo,
                    Ciclo = record.Ciclo,
                    Descripcion = record.Descripcion
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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CiclosPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ciclo = new LkCiclos
            {
                IdCiclo = dto.IdCiclo,
                Ciclo = dto.Ciclo,
                Descripcion = dto.Descripcion,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _ciclosRepo.AddAsync(ciclo);

            return CreatedAtAction(
                nameof(GetById),
                new { id = ciclo.Id },
                ciclo
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var records = await _ciclosRepo.GetAllAsync();

                var dtos = records.Select(record => new CiclosGetDto
                {
                    IdCiclo = record.IdCiclo,
                    Ciclo = record.Ciclo,
                    Descripcion = record.Descripcion,
                    CreatedAt = record.CreatedAt,
                    UpdatedAt = record.UpdatedAt,
                    IsActive = record.IsActive
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _ciclosRepo.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No existe ciclo con id={id}" });

            await _ciclosRepo.DeleteAsync(id);

            return Ok(new { message = "Ciclo eliminado correctamente" });
        }
    }
}
