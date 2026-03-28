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
    public class FasesController : ControllerBase
    {
        private readonly IFasesService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<FasesController> _logger;

        public FasesController(IFasesService service, IMapper mapper, ILogger<FasesController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GetById FasesController requested. Id: {Id}", id);
            var record = await _service.GetByIdAsync(id);
            if (record == null)
            {
                _logger.LogWarning("GetById FasesController not found. Id: {Id}", id);
                return NotFound($"No existe registro para id={id}");
            }
            var dto = _mapper.Map<FasesGetDto>(record);
            _logger.LogInformation("GetById FasesController succeeded. Id: {Id}", id);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll FasesController requested");
            var records = await _service.GetAllAsync();
            var dtos = _mapper.Map<List<FasesGetDto>>(records);
            _logger.LogInformation("GetAll FasesController returned {Count} records", dtos.Count);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FasesPostDto dto)
        {
            _logger.LogInformation("Create FasesController requested");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create FasesController rejected: invalid model state");
                return BadRequest(ModelState);
            }
            var entity = _mapper.Map<LkFases>(dto);
            await _service.AddAsync(entity);
            var createdDto = _mapper.Map<FasesGetDto>(entity);
            _logger.LogInformation("Create FasesController succeeded. Id: {Id}", entity.IdFase);
            return CreatedAtAction(nameof(GetById), new { id = entity.IdFase }, createdDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete FasesController requested. Id: {Id}", id);
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Delete FasesController succeeded. Id: {Id}", id);
                return Ok(new { message = "Fase eliminada correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Delete FasesController failed. Id: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
