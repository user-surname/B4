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
    public class PlantillasBotonesPasosTiposController : ControllerBase
    {
        private readonly IPlantillasBotonesPasosTiposService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<PlantillasBotonesPasosTiposController> _logger;

        public PlantillasBotonesPasosTiposController(IPlantillasBotonesPasosTiposService service, IMapper mapper, ILogger<PlantillasBotonesPasosTiposController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GetById PlantillasBotonesPasosTiposController requested. Id: {Id}", id);
            var record = await _service.GetByIdAsync(id);
            if (record == null)
            {
                _logger.LogWarning("GetById PlantillasBotonesPasosTiposController not found. Id: {Id}", id);
                return NotFound($"No existe registro para id={id}");
            }
            var dto = _mapper.Map<PlantillasBotonesPasosTiposGetDto>(record);
            _logger.LogInformation("GetById PlantillasBotonesPasosTiposController succeeded. Id: {Id}", id);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll PlantillasBotonesPasosTiposController requested");
            var records = await _service.GetAllAsync();
            var dtos = _mapper.Map<List<PlantillasBotonesPasosTiposGetDto>>(records);
            _logger.LogInformation("GetAll PlantillasBotonesPasosTiposController returned {Count} records", dtos.Count);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantillasBotonesPasosTiposPostDto dto)
        {
            _logger.LogInformation("Create PlantillasBotonesPasosTiposController requested");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create PlantillasBotonesPasosTiposController rejected: invalid model state");
                return BadRequest(ModelState);
            }
            var entity = _mapper.Map<LkPlantillasBotonesPasosTipos>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;
            await _service.AddAsync(entity);
            var createdDto = _mapper.Map<PlantillasBotonesPasosTiposGetDto>(entity);
            _logger.LogInformation("Create PlantillasBotonesPasosTiposController succeeded. Id: {Id}", entity.IdPasoTipo);
            return CreatedAtAction(nameof(GetById), new { id = entity.IdPasoTipo }, createdDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete PlantillasBotonesPasosTiposController requested. Id: {Id}", id);
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Delete PlantillasBotonesPasosTiposController succeeded. Id: {Id}", id);
                return Ok(new { message = "Paso tipo eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Delete PlantillasBotonesPasosTiposController failed. Id: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
