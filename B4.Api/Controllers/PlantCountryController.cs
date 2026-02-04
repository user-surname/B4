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
    public class PlantCountryController : ControllerBase
    {
        private readonly IPlantCountryRepository _repo;

        public PlantCountryController(IPlantCountryRepository repo)
        {
            _repo = repo;
        }

        // GET /api/v1/PlantCountry/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var record = await _repo.GetByIdAsync(id);
                if (record is null)
                    return NotFound($"No existe país con id={id}");

                var dto = new PlantCountryGetDto
                {
                    IdCountry = record.IdCountry,
                    Country = record.Country,
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

        // GET /api/v1/PlantCountry
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var records = await _repo.GetAllAsync();
                var dtos = records.Select(r => new PlantCountryGetDto
                {
                    IdCountry = r.IdCountry,
                    Country = r.Country,
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

        // POST /api/v1/PlantCountry
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantCountryPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = new LkPlantCountry
            {
                Country = dto.Country,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            await _repo.AddAsync(entity);

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.IdCountry },
                entity
            );
        }

        // DELETE /api/v1/PlantCountry/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No existe país con id={id}" });

            await _repo.DeleteAsync(id);

            return Ok(new { message = "País eliminado correctamente" });
        }
    }
}
