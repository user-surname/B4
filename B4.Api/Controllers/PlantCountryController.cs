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
        private readonly ILogger<PlantCountryController> _logger;

        public PlantCountryController(IPlantCountryService service, IMapper mapper, ILogger<PlantCountryController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GetById PlantCountryController requested. Id: {Id}", id);
            var record = await _service.GetByIdAsync(id);
            if (record == null)
            {
                _logger.LogWarning("GetById PlantCountryController not found. Id: {Id}", id);
                return NotFound($"No existe registro para id={id}");
            }
            var dto = _mapper.Map<PlantCountryGetDto>(record);
            _logger.LogInformation("GetById PlantCountryController succeeded. Id: {Id}", id);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll PlantCountryController requested");
            var records = await _service.GetAllAsync();
            var dtos = _mapper.Map<List<PlantCountryGetDto>>(records);
            _logger.LogInformation("GetAll PlantCountryController returned {Count} records", dtos.Count);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantCountryPostDto dto)
        {
            _logger.LogInformation("Create PlantCountryController requested");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create PlantCountryController rejected: invalid model state");
                return BadRequest(ModelState);
            }
            var entity = _mapper.Map<LkPlantCountry>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;
            await _service.AddAsync(entity);
            var createdDto = _mapper.Map<PlantCountryGetDto>(entity);
            _logger.LogInformation("Create PlantCountryController succeeded. Id: {Id}", entity.IdCountry);
            return CreatedAtAction(nameof(GetById), new { id = entity.IdCountry }, createdDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete PlantCountryController requested. Id: {Id}", id);
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Delete PlantCountryController succeeded. Id: {Id}", id);
                return Ok(new { message = "Pais eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Delete PlantCountryController failed. Id: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
