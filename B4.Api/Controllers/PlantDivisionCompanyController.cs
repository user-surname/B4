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
    public class PlantDivisionCompanyController : ControllerBase
    {
        private readonly IPlantDivisionCompanyService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<PlantDivisionCompanyController> _logger;

        public PlantDivisionCompanyController(IPlantDivisionCompanyService service, IMapper mapper, ILogger<PlantDivisionCompanyController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GetById PlantDivisionCompanyController requested. Id: {Id}", id);
            var record = await _service.GetByIdAsync(id);
            if (record == null)
            {
                _logger.LogWarning("GetById PlantDivisionCompanyController not found. Id: {Id}", id);
                return NotFound($"No existe registro para id={id}");
            }
            var dto = _mapper.Map<PlantDivisionCompanyGetDto>(record);
            _logger.LogInformation("GetById PlantDivisionCompanyController succeeded. Id: {Id}", id);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll PlantDivisionCompanyController requested");
            var records = await _service.GetAllAsync();
            var dtos = _mapper.Map<List<PlantDivisionCompanyGetDto>>(records);
            _logger.LogInformation("GetAll PlantDivisionCompanyController returned {Count} records", dtos.Count);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantDivisionCompanyPostDto dto)
        {
            _logger.LogInformation("Create PlantDivisionCompanyController requested");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create PlantDivisionCompanyController rejected: invalid model state");
                return BadRequest(ModelState);
            }
            var entity = _mapper.Map<LkPlantDivisionCompany>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = 1;
            await _service.AddAsync(entity);
            var createdDto = _mapper.Map<PlantDivisionCompanyGetDto>(entity);
            _logger.LogInformation("Create PlantDivisionCompanyController succeeded. Id: {Id}", entity.IdDivisionCompany);
            return CreatedAtAction(nameof(GetById), new { id = entity.IdDivisionCompany }, createdDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete PlantDivisionCompanyController requested. Id: {Id}", id);
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Delete PlantDivisionCompanyController succeeded. Id: {Id}", id);
                return Ok(new { message = "Division Company eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Delete PlantDivisionCompanyController failed. Id: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
