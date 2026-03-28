using AutoMapper;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ControlController : ControllerBase
    {
        private readonly IControlService _controlService;
        private readonly IMapper _mapper;
        private readonly ILogger<ControlController> _logger;

        public ControlController(IControlService controlService, IMapper mapper, ILogger<ControlController> logger)
        {
            _controlService = controlService;
            _mapper = mapper;
            _logger = logger;
        }

        // -------------------------------------------------
        // Obtener control por ID
        // -------------------------------------------------
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GetById control requested. Id: {Id}", id);
            var record = await _controlService.GetByIdAsync(id);

            if (record is null)
            {
                _logger.LogWarning("GetById control not found. Id: {Id}", id);
                return NotFound($"No existe control para id={id}");
            }

            var dto = _mapper.Map<ControlGetDto>(record);
            _logger.LogInformation("GetById control succeeded. Id: {Id}", id);
            return Ok(dto);
        }

        // -------------------------------------------------
        // Crear nuevo control
        // -------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ControlPostDto dto)
        {
            _logger.LogInformation("Create control requested");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create control rejected: invalid model state");
                return BadRequest(ModelState);
            }

            var control = _mapper.Map<Control>(dto);
            await _controlService.AddAsync(control);

            _logger.LogInformation("Create control succeeded. IdControl: {IdControl}", control.IdControl);
            return CreatedAtAction(
                nameof(GetById),
                new { id = control.IdControl },
                _mapper.Map<ControlGetDto>(control)
            );
        }

        // -------------------------------------------------
        // Obtener todos los controles
        // -------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll controls requested");
            var records = await _controlService.GetAllAsync();
            var dtos = _mapper.Map<List<ControlGetDto>>(records);
            _logger.LogInformation("GetAll controls returned {Count} records", dtos.Count);
            return Ok(dtos);
        }

        // -------------------------------------------------
        // Eliminar control
        // -------------------------------------------------
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete control requested. Id: {Id}", id);
            try
            {
                await _controlService.DeleteAsync(id);
                _logger.LogInformation("Delete control succeeded. Id: {Id}", id);
                return Ok(new { message = "Control eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Delete control failed. Id: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
