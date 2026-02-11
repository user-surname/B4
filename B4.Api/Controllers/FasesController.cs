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
        private readonly IFasesService _fasesService;
        private readonly IMapper _mapper;

        public FasesController(IFasesService fasesService, IMapper mapper)
        {
            _fasesService = fasesService;
            _mapper = mapper;
        }

        // GET /api/v1/Fases/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _fasesService.GetByIdAsync(id);

            if (record is null)
                return NotFound($"No existe fase para id={id}");

            // Mapear entidad -> DTO
            var dto = _mapper.Map<FasesGetDto>(record);

            return Ok(dto);
        }

        // POST /api/v1/Fases
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FasesPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapear DTO -> entidad
            var fase = _mapper.Map<LkFases>(dto);

            await _fasesService.AddAsync(fase);

            // Mapear entidad -> DTO para la respuesta
            var createdDto = _mapper.Map<FasesGetDto>(fase);

            return CreatedAtAction(
                nameof(GetById),
                new { id = fase.IdFase },
                createdDto
            );
        }

        // GET /api/v1/Fases
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _fasesService.GetAllAsync();

            // Mapear lista de entidades -> lista de DTOs
            var dtos = _mapper.Map<List<FasesGetDto>>(records);

            return Ok(dtos);
        }

        // DELETE /api/v1/Fases/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _fasesService.DeleteAsync(id);
                return Ok(new { message = "Fase eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
