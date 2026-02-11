using AutoMapper;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Api.Middleware;
using B4.Domain.Services;
using B4.Models.Entities.LkEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EpigrafeController : ControllerBase
    {
        private readonly IEpigrafeService _epigrafeService;
        private readonly IMapper _mapper;

        public EpigrafeController(IEpigrafeService epigrafeService, IMapper mapper)
        {
            _epigrafeService = epigrafeService;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        [DisabledEndpoint]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _epigrafeService.GetByIdAsync(id);

            if (record == null)
                return NotFound($"No existe epígrafe para id={id}");

            // Mapeo automático a DTO
            var dto = _mapper.Map<EpigrafeGetDto>(record);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EpigrafePostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapear DTO -> entidad
            var epigrafe = _mapper.Map<LkEpigrafe>(dto);

            await _epigrafeService.AddAsync(epigrafe);

            // Retornar DTO mapeado
            var createdDto = _mapper.Map<EpigrafeGetDto>(epigrafe);

            return CreatedAtAction(
                nameof(GetById),
                new { id = epigrafe.IdEpigrafe },
                createdDto
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _epigrafeService.GetAllAsync();

            // Mapear lista de entidades -> lista de DTOs
            var dtos = _mapper.Map<List<EpigrafeGetDto>>(records);

            return Ok(dtos);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _epigrafeService.DeleteAsync(id);
                return Ok(new { message = "Epigrafe eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
