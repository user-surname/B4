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
            if (record is null)
                return NotFound(new { message = $"No existe ciclo para id={id}" });
            var dto = _mapper.Map<CiclosGetDto>(record);
            return Ok(dto);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CiclosPostDto dto)
        {
            if (dto == null)
                throw new ArgumentException("El cuerpo de la petición no puede estar vacío");

            if (!ModelState.IsValid)
                throw new ArgumentException("Los datos enviados no son válidos");

            var ciclo = _mapper.Map<LkCiclos>(dto);
            await _ciclosService.AddAsync(ciclo);

            return CreatedAtAction(nameof(GetById), new { id = ciclo.IdCiclo }, _mapper.Map<CiclosGetDto>(ciclo));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _ciclosService.GetAllAsync();
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

    }
}
