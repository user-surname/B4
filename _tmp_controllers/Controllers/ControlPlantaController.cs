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

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlPlantaController"/> class.
        /// </summary>
        /// <param name="controlPlantaService">ControlPlanta service.</param>
        public ControlPlantaController(IControlPlantaService controlPlantaService)
        {
            _controlPlantaService = controlPlantaService;
        }

        // GET /api/v1/ControlPlanta/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _controlPlantaService.GetByIdAsync(id);

            if (record is null)
                return NotFound($"No existe ControlPlanta para id={id}");

            return Ok(record);
        }

        // GET /api/v1/ControlPlanta
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _controlPlantaService.GetAllAsync();
            return Ok(records);
        }
    }
}
