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
    public class PlantControllersController : ControllerBase
    {
        private readonly IPlantControllersService _service;

        public PlantControllersController(IPlantControllersService service)
        {
            _service = service;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _service.GetByIdAsync(id);
            if (record == null)
                return NotFound($"No existe controlador para id={id}");

            var dto = new PlantControllersGetDto
            {
                IdCompanyController = record.IdCompanyController,
                IdCompany = record.IdCompany,
                Controller = record.Controller,
                Email = record.Email,
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

            var dtos = records.Select(r => new PlantControllersGetDto
            {
                IdCompanyController = r.IdCompanyController,
                IdCompany = r.IdCompany,
                Controller = r.Controller,
                Email = r.Email,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                IsActive = r.IsActive
            }).ToList();

            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantControllersPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = new LkPlantControllers
            {
                IdCompany = dto.IdCompany,
                Controller = dto.Controller,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            await _service.AddAsync(entity);

            return CreatedAtAction(nameof(GetById), new { id = entity.IdCompanyController }, entity);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new { message = "PlantController eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
