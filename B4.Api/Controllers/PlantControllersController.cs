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
    public class PlantControllersController : ControllerBase
    {
        private readonly IPlantControllersService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<PlantControllersController> _logger;

        public PlantControllersController(IPlantControllersService service, IMapper mapper, ILogger<PlantControllersController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GetById PlantControllersController requested. Id: {Id}", id);
            var record = await _service.GetByIdAsync(id);
            if (record == null)
            {
                _logger.LogWarning("GetById PlantControllersController not found. Id: {Id}", id);
                return NotFound($"No existe registro para id={id}");
            }
            var dto = _mapper.Map<PlantControllersGetDto>(record);
            _logger.LogInformation("GetById PlantControllersController succeeded. Id: {Id}", id);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll PlantControllersController requested");
            var records = await _service.GetAllAsync();
            var dtos = _mapper.Map<List<PlantControllersGetDto>>(records);
            _logger.LogInformation("GetAll PlantControllersController returned {Count} records", dtos.Count);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantControllersPostDto dto)
        {
            _logger.LogInformation("Create PlantControllersController requested");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create PlantControllersController rejected: invalid model state");
                return BadRequest(ModelState);
            }
            var entity = _mapper.Map<LkPlantControllers>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;
            await _service.AddAsync(entity);
            var createdDto = _mapper.Map<PlantControllersGetDto>(entity);
            _logger.LogInformation("Create PlantControllersController succeeded. Id: {Id}", entity.IdCompanyController);
            return CreatedAtAction(nameof(GetById), new { id = entity.IdCompanyController }, createdDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete PlantControllersController requested. Id: {Id}", id);
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Delete PlantControllersController succeeded. Id: {Id}", id);
                return Ok(new { message = "PlantController eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Delete PlantControllersController failed. Id: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
