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
        private readonly ILogger<PlantDivisionController> _logger;

        public PlantDivisionController(IPlantDivisionService service, IMapper mapper, ILogger<PlantDivisionController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GetById PlantDivisionController requested. Id: {Id}", id);
            var record = await _service.GetByIdAsync(id);
            if (record == null)
            {
                _logger.LogWarning("GetById PlantDivisionController not found. Id: {Id}", id);
                return NotFound($"No existe registro para id={id}");
            }
            var dto = _mapper.Map<PlantDivisionGetDto>(record);
            _logger.LogInformation("GetById PlantDivisionController succeeded. Id: {Id}", id);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll PlantDivisionController requested");
            var records = await _service.GetAllAsync();
            var dtos = _mapper.Map<List<PlantDivisionGetDto>>(records);
            _logger.LogInformation("GetAll PlantDivisionController returned {Count} records", dtos.Count);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantDivisionPostDto dto)
        {
            _logger.LogInformation("Create PlantDivisionController requested");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create PlantDivisionController rejected: invalid model state");
                return BadRequest(ModelState);
            }
            var entity = _mapper.Map<LkPlantDivision>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;
            await _service.AddAsync(entity);
            var createdDto = _mapper.Map<PlantDivisionGetDto>(entity);
            _logger.LogInformation("Create PlantDivisionController succeeded. Id: {Id}", entity.IdDivision);
            return CreatedAtAction(nameof(GetById), new { id = entity.IdDivision }, createdDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete PlantDivisionController requested. Id: {Id}", id);
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Delete PlantDivisionController succeeded. Id: {Id}", id);
                return Ok(new { message = "Division eliminada correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Delete PlantDivisionController failed. Id: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
