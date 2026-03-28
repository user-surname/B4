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
        private readonly ILogger<PlantSubdivisionController> _logger;

        public PlantSubdivisionController(IPlantSubdivisionService service, IMapper mapper, ILogger<PlantSubdivisionController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GetById PlantSubdivisionController requested. Id: {Id}", id);
            var record = await _service.GetByIdAsync(id);
            if (record == null)
            {
                _logger.LogWarning("GetById PlantSubdivisionController not found. Id: {Id}", id);
                return NotFound($"No existe registro para id={id}");
            }
            var dto = _mapper.Map<PlantSubdivisionGetDto>(record);
            _logger.LogInformation("GetById PlantSubdivisionController succeeded. Id: {Id}", id);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll PlantSubdivisionController requested");
            var records = await _service.GetAllAsync();
            var dtos = _mapper.Map<List<PlantSubdivisionGetDto>>(records);
            _logger.LogInformation("GetAll PlantSubdivisionController returned {Count} records", dtos.Count);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantSubdivisionPostDto dto)
        {
            _logger.LogInformation("Create PlantSubdivisionController requested");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create PlantSubdivisionController rejected: invalid model state");
                return BadRequest(ModelState);
            }
            var entity = _mapper.Map<LkPlantSubdivision>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;
            await _service.AddAsync(entity);
            var createdDto = _mapper.Map<PlantSubdivisionGetDto>(entity);
            _logger.LogInformation("Create PlantSubdivisionController succeeded. Id: {Id}", entity.IdSubdivision);
            return CreatedAtAction(nameof(GetById), new { id = entity.IdSubdivision }, createdDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete PlantSubdivisionController requested. Id: {Id}", id);
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Delete PlantSubdivisionController succeeded. Id: {Id}", id);
                return Ok(new { message = "Subdivision eliminada correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Delete PlantSubdivisionController failed. Id: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
