using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PlantillasBotonesPasosTiposController : ControllerBase
    {
        private readonly IPlantillasBotonesPasosTiposRepository _repo;

        public PlantillasBotonesPasosTiposController(IPlantillasBotonesPasosTiposRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var record = await _repo.GetByIdAsync(id);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var records = await _repo.GetAllAsync();
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

            await _repo.AddAsync(entity);

            return CreatedAtAction(nameof(GetById), new { id = entity.IdPasoTipo }, entity);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No existe paso tipo con id={id}" });

            await _repo.DeleteAsync(id);

            return Ok(new { message = "Paso tipo eliminado correctamente" });
        }
    }
}
