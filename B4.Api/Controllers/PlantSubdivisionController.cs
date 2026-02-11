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
    public class PlantSubdivisionController : ControllerBase
    {
        private readonly IPlantSubdivisionService _service;
        private readonly IMapper _mapper;

        public PlantSubdivisionController(IPlantSubdivisionService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET /api/v1/PlantSubdivision/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _service.GetByIdAsync(id);
            if (record is null)
                return NotFound($"No existe subdivisión con id={id}");

            var dto = _mapper.Map<PlantSubdivisionGetDto>(record);
            return Ok(dto);
        }

        // GET /api/v1/PlantSubdivision
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _service.GetAllAsync();
            var dtos = _mapper.Map<List<PlantSubdivisionGetDto>>(records);
            return Ok(dtos);
        }

        // POST /api/v1/PlantSubdivision
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantSubdivisionPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _mapper.Map<LkPlantSubdivision>(dto);

            // Inicializar campos adicionales
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;

            await _service.AddAsync(entity);

            var createdDto = _mapper.Map<PlantSubdivisionGetDto>(entity);

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.IdSubdivision },
                createdDto
            );
        }

        // DELETE /api/v1/PlantSubdivision/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new { message = "Subdivisión eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
