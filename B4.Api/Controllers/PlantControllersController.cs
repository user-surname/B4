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
    public class PlantControllersController : ControllerBase
    {
        private readonly IPlantControllersRepository _repo;

        public PlantControllersController(IPlantControllersRepository repo)
        {
            _repo = repo;
        }

        // GET /api/v1/PlantControllers/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var record = await _repo.GetByIdAsync(id);

                if (record is null)
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/v1/PlantControllers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var records = await _repo.GetAllAsync();

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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST /api/v1/PlantControllers
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

            await _repo.AddAsync(entity);

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.IdCompanyController },
                entity
            );
        }

        // DELETE /api/v1/PlantControllers/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No existe controlador con id={id}" });

            await _repo.DeleteAsync(id);

            return Ok(new { message = "PlantController eliminado correctamente" });
        }
    }
}
