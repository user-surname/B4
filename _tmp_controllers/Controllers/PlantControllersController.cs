using AutoMapper;
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
        private readonly IMapper _mapper;

        public PlantControllersController(IPlantControllersService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET /api/v1/PlantControllers/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _service.GetByIdAsync(id);
            if (record == null)
                return NotFound($"No existe controlador para id={id}");

            // Mapear entidad -> DTO
            var dto = _mapper.Map<PlantControllersGetDto>(record);

            return Ok(dto);
        }

        // GET /api/v1/PlantControllers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _service.GetAllAsync();

            // Mapear lista de entidades -> lista de DTOs
            var dtos = _mapper.Map<List<PlantControllersGetDto>>(records);

            return Ok(dtos);
        }

        // POST /api/v1/PlantControllers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantControllersPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapear DTO -> entidad
            var entity = _mapper.Map<LkPlantControllers>(dto);

            // Inicializar campos que no vienen del DTO
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;

            await _service.AddAsync(entity);

            // Mapear entidad -> DTO para la respuesta
            var createdDto = _mapper.Map<PlantControllersGetDto>(entity);

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.IdCompanyController },
                createdDto
            );
        }

        // DELETE /api/v1/PlantControllers/{id}
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
