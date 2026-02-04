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
    public class PlantDivisionController : ControllerBase
    {
        private readonly IPlantDivisionRepository _divisionRepo;

        public PlantDivisionController(IPlantDivisionRepository divisionRepo)
        {
            _divisionRepo = divisionRepo;
        }

        // GET /api/v1/PlantDivision/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var record = await _divisionRepo.GetByIdAsync(id);
                if (record is null)
                    return NotFound($"No existe división con id={id}");

                var dto = new PlantDivisionGetDto
                {
                    IdDivision = record.IdDivision,
                    Division = record.Division,
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

        // GET /api/v1/PlantDivision
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var records = await _divisionRepo.GetAllAsync();
                var dtos = records.Select(record => new PlantDivisionGetDto
                {
                    IdDivision = record.IdDivision,
                    Division = record.Division,
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

        // POST /api/v1/PlantDivision
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantDivisionPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var division = new LkPlantDivision
            {
                Division = dto.Division,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            await _divisionRepo.AddAsync(division);

            return CreatedAtAction(
                nameof(GetById),
                new { id = division.IdDivision },
                division
            );
        }

        // DELETE /api/v1/PlantDivision/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _divisionRepo.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No existe división con id={id}" });

            await _divisionRepo.DeleteAsync(id);

            return Ok(new { message = "División eliminada correctamente" });
        }
    }
}
