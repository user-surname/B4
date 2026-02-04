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
    public class PlantSubdivisionController : ControllerBase
    {
        private readonly IPlantSubdivisionRepository _repository;

        public PlantSubdivisionController(IPlantSubdivisionRepository repository)
        {
            _repository = repository;
        }

        // GET: /api/v1/PlantSubdivision/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var record = await _repository.GetByIdAsync(id);
                if (record is null)
                    return NotFound($"No existe subdivisión con id={id}");

                var dto = new PlantSubdivisionGetDto
                {
                    IdSubdivision = record.IdSubdivision,
                    Subdivision = record.Subdivision,
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

        // GET: /api/v1/PlantSubdivision
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var records = await _repository.GetAllAsync();
                var dtos = records.Select(r => new PlantSubdivisionGetDto
                {
                    IdSubdivision = r.IdSubdivision,
                    Subdivision = r.Subdivision,
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

        // POST: /api/v1/PlantSubdivision
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantSubdivisionPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = new LkPlantSubdivision
            {
                IdSubdivision = dto.IdSubdivision,
                Subdivision = dto.Subdivision,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            await _repository.AddAsync(entity);

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.IdSubdivision },
                entity
            );
        }

        // DELETE: /api/v1/PlantSubdivision/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No existe subdivisión con id={id}" });

            await _repository.DeleteAsync(id);

            return Ok(new { message = "Subdivisión eliminada correctamente" });
        }
    }
}
