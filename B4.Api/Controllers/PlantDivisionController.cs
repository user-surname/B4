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
    public class PlantDivisionController : ControllerBase
    {
        private readonly IPlantDivisionService _service;
        private readonly IMapper _mapper;

        public PlantDivisionController(IPlantDivisionService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET /api/v1/PlantDivision/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _service.GetByIdAsync(id);
            if (record is null)
                return NotFound($"No existe división con id={id}");

            // Mapear entidad -> DTO
            var dto = _mapper.Map<PlantDivisionGetDto>(record);

            return Ok(dto);
        }

        // GET /api/v1/PlantDivision
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _service.GetAllAsync();

            // Mapear lista de entidades -> lista de DTOs
            var dtos = _mapper.Map<List<PlantDivisionGetDto>>(records);

            return Ok(dtos);
        }

        // POST /api/v1/PlantDivision
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantDivisionPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapear DTO -> entidad
            var entity = _mapper.Map<LkPlantDivision>(dto);

            // Inicializar campos que no vienen del DTO
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;

            await _service.AddAsync(entity);

            // Mapear entidad -> DTO para la respuesta
            var createdDto = _mapper.Map<PlantDivisionGetDto>(entity);

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.IdDivision },
                createdDto
            );
        }

        // DELETE /api/v1/PlantDivision/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new { message = "División eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
