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
    public class PlantDivisionCompanyController : ControllerBase
    {
        private readonly IPlantDivisionCompanyRepository _repo;

        public PlantDivisionCompanyController(IPlantDivisionCompanyRepository repo)
        {
            _repo = repo;
        }

        // GET /api/v1/PlantDivisionCompany/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var record = await _repo.GetByIdAsync(id);
                if (record is null)
                    return NotFound($"No existe division company para id={id}");

                var dto = new PlantDivisionCompanyGetDto
                {
                    IdDivisionCompany = record.IdDivisionCompany,
                    DivisionCompany = record.DivisionCompany,
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

        // GET /api/v1/PlantDivisionCompany
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var records = await _repo.GetAllAsync();
                var dtos = records.Select(r => new PlantDivisionCompanyGetDto
                {
                    IdDivisionCompany = r.IdDivisionCompany,
                    DivisionCompany = r.DivisionCompany,
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

        // POST /api/v1/PlantDivisionCompany
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantDivisionCompanyPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = new LkPlantDivisionCompany
            {
                DivisionCompany = dto.DivisionCompany,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            await _repo.AddAsync(entity);

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.IdDivisionCompany },
                entity
            );
        }

        // DELETE /api/v1/PlantDivisionCompany/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No existe division company con id={id}" });

            await _repo.DeleteAsync(id);

            return Ok(new { message = "Division Company eliminado correctamente" });
        }
    }
}
