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
    public class PlantDivisionCompanyController : ControllerBase
    {
        private readonly IPlantDivisionCompanyService _service;
        private readonly IMapper _mapper;

        public PlantDivisionCompanyController(IPlantDivisionCompanyService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET /api/v1/PlantDivisionCompany/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _service.GetByIdAsync(id);
            if (record is null)
                return NotFound($"No existe division company para id={id}");

            // Mapear entidad -> DTO
            var dto = _mapper.Map<PlantDivisionCompanyGetDto>(record);

            return Ok(dto);
        }

        // GET /api/v1/PlantDivisionCompany
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _service.GetAllAsync();

            // Mapear lista de entidades -> lista de DTOs
            var dtos = _mapper.Map<List<PlantDivisionCompanyGetDto>>(records);

            return Ok(dtos);
        }

        // POST /api/v1/PlantDivisionCompany
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantDivisionCompanyPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapear DTO -> entidad
            var entity = _mapper.Map<LkPlantDivisionCompany>(dto);

            // Inicializar campos que no vienen del DTO
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;

            await _service.AddAsync(entity);

            // Mapear entidad -> DTO para la respuesta
            var createdDto = _mapper.Map<PlantDivisionCompanyGetDto>(entity);

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.IdDivisionCompany },
                createdDto
            );
        }

        // DELETE /api/v1/PlantDivisionCompany/{id}
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
