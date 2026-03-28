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
        private readonly ILogger<PlantCurrencyController> _logger;

        public PlantCurrencyController(IPlantCurrencyService service, IMapper mapper, ILogger<PlantCurrencyController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GetById PlantCurrencyController requested. Id: {Id}", id);
            var record = await _service.GetByIdAsync(id);
            if (record == null)
            {
                _logger.LogWarning("GetById PlantCurrencyController not found. Id: {Id}", id);
                return NotFound($"No existe registro para id={id}");
            }
            var dto = _mapper.Map<PlantCurrencyGetDto>(record);
            _logger.LogInformation("GetById PlantCurrencyController succeeded. Id: {Id}", id);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll PlantCurrencyController requested");
            var records = await _service.GetAllAsync();
            var dtos = _mapper.Map<List<PlantCurrencyGetDto>>(records);
            _logger.LogInformation("GetAll PlantCurrencyController returned {Count} records", dtos.Count);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantCurrencyPostDto dto)
        {
            _logger.LogInformation("Create PlantCurrencyController requested");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create PlantCurrencyController rejected: invalid model state");
                return BadRequest(ModelState);
            }
            var entity = _mapper.Map<LkPlantCurrency>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;
            await _service.AddAsync(entity);
            var createdDto = _mapper.Map<PlantCurrencyGetDto>(entity);
            _logger.LogInformation("Create PlantCurrencyController succeeded. Id: {Id}", entity.IdCurrency);
            return CreatedAtAction(nameof(GetById), new { id = entity.IdCurrency }, createdDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete PlantCurrencyController requested. Id: {Id}", id);
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Delete PlantCurrencyController succeeded. Id: {Id}", id);
                return Ok(new { message = "Currency eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Delete PlantCurrencyController failed. Id: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
