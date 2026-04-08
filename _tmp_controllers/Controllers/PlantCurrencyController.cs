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
    public class PlantCurrencyController : ControllerBase
    {
        private readonly IPlantCurrencyService _service;
        private readonly IMapper _mapper;

        public PlantCurrencyController(IPlantCurrencyService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET /api/v1/PlantCurrency/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _service.GetByIdAsync(id);
            if (record == null)
                return NotFound($"No existe currency con id={id}");

            // Mapear entidad -> DTO
            var dto = _mapper.Map<PlantCurrencyGetDto>(record);

            return Ok(dto);
        }

        // GET /api/v1/PlantCurrency
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _service.GetAllAsync();

            // Mapear lista de entidades -> lista de DTOs
            var dtos = _mapper.Map<List<PlantCurrencyGetDto>>(records);

            return Ok(dtos);
        }

        // POST /api/v1/PlantCurrency
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantCurrencyPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapear DTO -> entidad
            var entity = _mapper.Map<LkPlantCurrency>(dto);

            // Inicializar campos que no vienen del DTO
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;

            await _service.AddAsync(entity);

            // Mapear entidad -> DTO para la respuesta
            var createdDto = _mapper.Map<PlantCurrencyGetDto>(entity);

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.IdCurrency },
                createdDto
            );
        }

        // DELETE /api/v1/PlantCurrency/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new { message = "Currency eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
