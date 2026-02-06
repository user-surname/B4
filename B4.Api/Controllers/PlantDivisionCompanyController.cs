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
    public class PlantDivisionCompanyController : ControllerBase
    {
        private readonly IPlantDivisionCompanyService _service;

        public PlantDivisionCompanyController(IPlantDivisionCompanyService service)
        {
            _service = service;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _service.GetByIdAsync(id);
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _service.GetAllAsync();
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

            await _service.AddAsync(entity);

            return CreatedAtAction(nameof(GetById), new { id = entity.IdDivisionCompany }, entity);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new { message = "Division Company eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
