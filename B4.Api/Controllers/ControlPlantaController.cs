using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    /// <summary>
    /// Exposes read endpoints for ControlPlanta.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ControlPlantaController : ControllerBase
    {
        private readonly IControlPlantaService _controlPlantaService;
        private readonly ILogger<ControlPlantaController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlPlantaController"/> class.
        /// </summary>
        /// <param name="controlPlantaService">ControlPlanta service.</param>
        /// <param name="logger">Logger.</param>
        public ControlPlantaController(IControlPlantaService controlPlantaService, ILogger<ControlPlantaController> logger)
        {
            _controlPlantaService = controlPlantaService;
            _logger = logger;
        }

        // GET /api/v1/ControlPlanta/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GetById ControlPlanta requested. Id: {Id}", id);
            var record = await _controlPlantaService.GetByIdAsync(id);

            if (record is null)
            {
                _logger.LogWarning("GetById ControlPlanta not found. Id: {Id}", id);
                return NotFound($"No existe ControlPlanta para id={id}");
            }

            _logger.LogInformation("GetById ControlPlanta succeeded. Id: {Id}", id);
            return Ok(record);
        }

        // GET /api/v1/ControlPlanta
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll ControlPlanta requested");
            var records = await _controlPlantaService.GetAllAsync();
            _logger.LogInformation("GetAll ControlPlanta returned {Count} records", records?.Count() ?? 0);
            return Ok(records);
        }
    }
}
