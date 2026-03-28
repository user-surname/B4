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
        private readonly ILogger<PlantTreeController> _logger;

        public PlantTreeController(IPlantTreeService service, IMapper mapper, ILogger<PlantTreeController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GetById PlantTreeController requested. Id: {Id}", id);
            var record = await _service.GetByIdAsync(id);
            if (record == null)
            {
                _logger.LogWarning("GetById PlantTreeController not found. Id: {Id}", id);
                return NotFound($"No existe registro para id={id}");
            }
            var dto = _mapper.Map<PlantTreeGetDto>(record);
            _logger.LogInformation("GetById PlantTreeController succeeded. Id: {Id}", id);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll PlantTreeController requested");
            var records = await _service.GetAllAsync();
            var dtos = _mapper.Map<List<PlantTreeGetDto>>(records);
            _logger.LogInformation("GetAll PlantTreeController returned {Count} records", dtos.Count);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantTreePostDto dto)
        {
            _logger.LogInformation("Create PlantTreeController requested");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create PlantTreeController rejected: invalid model state");
                return BadRequest(ModelState);
            }
            var entity = _mapper.Map<LkPlantTree>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;
            await _service.AddAsync(entity);
            var createdDto = _mapper.Map<PlantTreeGetDto>(entity);
            _logger.LogInformation("Create PlantTreeController succeeded. Id: {Id}", entity.IdTree);
            return CreatedAtAction(nameof(GetById), new { id = entity.IdTree }, createdDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete PlantTreeController requested. Id: {Id}", id);
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Delete PlantTreeController succeeded. Id: {Id}", id);
                return Ok(new { message = "Arbol eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Delete PlantTreeController failed. Id: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
