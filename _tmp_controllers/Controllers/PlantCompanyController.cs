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
        private readonly IPlantCompanyService _plantService;
        private readonly IMapper _mapper;

        public PlantCompanyController(IPlantCompanyService plantService, IMapper mapper)
        {
            _plantService = plantService;
            _mapper = mapper;
        }

        // GET /api/v1/PlantCompany/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _plantService.GetByIdAsync(id);
            if (record == null)
                return NotFound($"No existe PlantCompany con id={id}");

            // Mapear entidad -> DTO
            var dto = _mapper.Map<PlantCompanyGetDto>(record);

            return Ok(dto);
        }

        // GET /api/v1/PlantCompany
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _plantService.GetAllAsync();

            // Mapear lista de entidades -> lista de DTOs
            var dtos = _mapper.Map<List<PlantCompanyGetDto>>(records);

            return Ok(dtos);
        }

        // POST /api/v1/PlantCompany
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantCompanyPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapear DTO -> entidad
            var company = _mapper.Map<LkPlantCompany>(dto);

            await _plantService.AddAsync(company);

            // Mapear entidad -> DTO para la respuesta
            var createdDto = _mapper.Map<PlantCompanyGetDto>(company);

            return CreatedAtAction(
                nameof(GetById),
                new { id = company.IdCompany },
                createdDto
            );
        }

        // DELETE /api/v1/PlantCompany/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _plantService.DeleteAsync(id);
                return Ok(new { message = "PlantCompany eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
