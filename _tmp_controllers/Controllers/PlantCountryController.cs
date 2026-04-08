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
    public class PlantCountryController : ControllerBase
    {
        private readonly IPlantCountryService _service;
        private readonly IMapper _mapper;

        public PlantCountryController(IPlantCountryService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET /api/v1/PlantCountry/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _service.GetByIdAsync(id);
            if (record == null)
                return NotFound($"No existe país con id={id}");

            // Mapear entidad -> DTO
            var dto = _mapper.Map<PlantCountryGetDto>(record);

            return Ok(dto);
        }

        // GET /api/v1/PlantCountry
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _service.GetAllAsync();

            // Mapear lista de entidades -> lista de DTOs
            var dtos = _mapper.Map<List<PlantCountryGetDto>>(records);

            return Ok(dtos);
        }

        // POST /api/v1/PlantCountry
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantCountryPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapear DTO -> entidad
            var entity = _mapper.Map<LkPlantCountry>(dto);

            // Inicializar campos que no vienen del DTO
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;

            await _service.AddAsync(entity);

            // Mapear entidad -> DTO para la respuesta
            var createdDto = _mapper.Map<PlantCountryGetDto>(entity);

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.IdCountry },
                createdDto
            );
        }

        // DELETE /api/v1/PlantCountry/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new { message = "País eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
