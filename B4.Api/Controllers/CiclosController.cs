using AutoMapper;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities.LkEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CiclosController : ControllerBase
    {
        private readonly ICiclosService _ciclosService;
        private readonly IMapper _mapper;

        public CiclosController(ICiclosService ciclosService, IMapper mapper)
        {
            _ciclosService = ciclosService;
            _mapper = mapper;
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _ciclosService.GetByIdAsync(id);

            // Respuesta en formato Json y no Raw
            if (record is null)
                return NotFound(new
                {
                    coderror = 404,
                    msg = $"No existe ciclo para id={id}"
                });
            // Mapeo automático a DTO
            var dto = _mapper.Map<CiclosGetDto>(record);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CiclosPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapeo DTO -> Modelo
            var ciclo = _mapper.Map<LkCiclos>(dto);

            await _ciclosService.AddAsync(ciclo);

            // Retornar DTO mapeado
            return CreatedAtAction(
                nameof(GetById),
                new { id = ciclo.IdCiclo },
                _mapper.Map<CiclosGetDto>(ciclo)
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _ciclosService.GetAllAsync();

            // Mapeo lista de modelos -> lista de DTOs
            var dtos = _mapper.Map<List<CiclosGetDto>>(records);

            return Ok(dtos);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ciclosService.DeleteAsync(id);
                return Ok(new { message = "Ciclo eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
