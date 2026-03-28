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
    public class PlantCompanyController : ControllerBase
    {
        private readonly IPlantCompanyService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<PlantCompanyController> _logger;

        public PlantCompanyController(IPlantCompanyService service, IMapper mapper, ILogger<PlantCompanyController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GetById PlantCompanyController requested. Id: {Id}", id);
            var record = await _service.GetByIdAsync(id);
            if (record == null)
            {
                _logger.LogWarning("GetById PlantCompanyController not found. Id: {Id}", id);
                return NotFound($"No existe registro para id={id}");
            }
            var dto = _mapper.Map<PlantCompanyGetDto>(record);
            _logger.LogInformation("GetById PlantCompanyController succeeded. Id: {Id}", id);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll PlantCompanyController requested");
            var records = await _service.GetAllAsync();
            var dtos = _mapper.Map<List<PlantCompanyGetDto>>(records);
            _logger.LogInformation("GetAll PlantCompanyController returned {Count} records", dtos.Count);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantCompanyPostDto dto)
        {
            _logger.LogInformation("Create PlantCompanyController requested");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create PlantCompanyController rejected: invalid model state");
                return BadRequest(ModelState);
            }
            var entity = _mapper.Map<LkPlantCompany>(dto);
            await _service.AddAsync(entity);
            var createdDto = _mapper.Map<PlantCompanyGetDto>(entity);
            _logger.LogInformation("Create PlantCompanyController succeeded. Id: {Id}", entity.IdCompany);
            return CreatedAtAction(nameof(GetById), new { id = entity.IdCompany }, createdDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete PlantCompanyController requested. Id: {Id}", id);
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Delete PlantCompanyController succeeded. Id: {Id}", id);
                return Ok(new { message = "PlantCompany eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Delete PlantCompanyController failed. Id: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
