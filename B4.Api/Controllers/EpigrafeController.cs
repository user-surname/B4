using AutoMapper;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities.LkEntities;
using B4.Domain.Services;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EpigrafeController : ControllerBase
    {
        private readonly IEpigrafeService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<EpigrafeController> _logger;

        public EpigrafeController(IEpigrafeService service, IMapper mapper, ILogger<EpigrafeController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GetById EpigrafeController requested. Id: {Id}", id);
            var record = await _service.GetByIdAsync(id);
            if (record == null)
            {
                _logger.LogWarning("GetById EpigrafeController not found. Id: {Id}", id);
                return NotFound($"No existe registro para id={id}");
            }
            var dto = _mapper.Map<EpigrafeGetDto>(record);
            _logger.LogInformation("GetById EpigrafeController succeeded. Id: {Id}", id);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll EpigrafeController requested");
            var records = await _service.GetAllAsync();
            var dtos = _mapper.Map<List<EpigrafeGetDto>>(records);
            _logger.LogInformation("GetAll EpigrafeController returned {Count} records", dtos.Count);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EpigrafePostDto dto)
        {
            _logger.LogInformation("Create EpigrafeController requested");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create EpigrafeController rejected: invalid model state");
                return BadRequest(ModelState);
            }
            var entity = _mapper.Map<LkEpigrafe>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;
            await _service.AddAsync(entity);
            var createdDto = _mapper.Map<EpigrafeGetDto>(entity);
            _logger.LogInformation("Create EpigrafeController succeeded. Id: {Id}", entity.IdEpigrafe);
            return CreatedAtAction(nameof(GetById), new { id = entity.IdEpigrafe }, createdDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete EpigrafeController requested. Id: {Id}", id);
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Delete EpigrafeController succeeded. Id: {Id}", id);
                return Ok(new { message = "Epigrafe eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Delete EpigrafeController failed. Id: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
