using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Api.Middleware;
using B4.Models.Entities.DataEntities;
using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CiclosController : ControllerBase
    {
        private readonly ICiclosService _ciclosService;

        public CiclosController(ICiclosService ciclosService)
        {
            _ciclosService = ciclosService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _ciclosService.GetByIdAsync(id);

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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CiclosPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ciclo = new LkCiclos
            {
                IdCiclo = dto.IdCiclo,
                Ciclo = dto.Ciclo,
                Descripcion = dto.Descripcion
            };

            await _ciclosService.AddAsync(ciclo);

            return CreatedAtAction(nameof(GetById), new { id = ciclo.Id }, ciclo);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _ciclosService.GetAllAsync();

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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ciclosService.DeleteAsync(id);
                return Ok(new { message = "Ciclo eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }

}
