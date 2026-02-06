using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities.LkEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PlantillasBotonesPasosTiposController : ControllerBase
    {
        private readonly IPlantillasBotonesPasosTiposService _service;

        public PlantillasBotonesPasosTiposController(IPlantillasBotonesPasosTiposService service)
        {
            _service = service;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _service.GetByIdAsync(id);
            if (record is null)
                return NotFound($"No existe paso tipo con id={id}");

            var dto = new PlantillasBotonesPasosTiposGetDto
            {
                IdPasoTipo = record.IdPasoTipo,
                Pasotipo = record.Pasotipo,
                Descripcion = record.Descripcion,
                CreatedAt = record.CreatedAt,
                UpdatedAt = record.UpdatedAt,
                IsActive = record.IsActive
            };

            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _service.GetAllAsync();
            var dtos = records.Select(r => new PlantillasBotonesPasosTiposGetDto
            {
                IdPasoTipo = r.IdPasoTipo,
                Pasotipo = r.Pasotipo,
                Descripcion = r.Descripcion,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                IsActive = r.IsActive
            }).ToList();

            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantillasBotonesPasosTiposPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = new LkPlantillasBotonesPasosTipos
            {
                IdPasoTipo = dto.IdPasoTipo,
                Pasotipo = dto.Pasotipo,
                Descripcion = dto.Descripcion,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            await _service.AddAsync(entity);

            return CreatedAtAction(nameof(GetById), new { id = entity.IdPasoTipo }, entity);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new { message = "Paso tipo eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
