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
    public class PlantTreeController : ControllerBase
    {
        private readonly IPlantTreeService _service;
        private readonly IMapper _mapper;

        public PlantTreeController(IPlantTreeService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET /api/v1/PlantTree/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _service.GetByIdAsync(id);
            if (record is null)
                return NotFound($"No existe árbol para id={id}");

            var dto = _mapper.Map<PlantTreeGetDto>(record);
            return Ok(dto);
        }

        // GET /api/v1/PlantTree
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _service.GetAllAsync();
            var dtos = _mapper.Map<List<PlantTreeGetDto>>(records);
            return Ok(dtos);
        }

        // POST /api/v1/PlantTree
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantTreePostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _mapper.Map<LkPlantTree>(dto);

            // Inicializar campos adicionales
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;

            await _service.AddAsync(entity);

            var createdDto = _mapper.Map<PlantTreeGetDto>(entity);

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.IdTree },
                createdDto
            );
        }

        // DELETE /api/v1/PlantTree/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new { message = "Árbol eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
