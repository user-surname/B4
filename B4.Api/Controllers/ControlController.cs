using AutoMapper;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace B4.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ControlController : ControllerBase
    {
        private readonly IControlService _controlService;
        private readonly IMapper _mapper;

        public ControlController(IControlService controlService, IMapper mapper)
        {
            _controlService = controlService;
            _mapper = mapper;
        }

        // -------------------------------------------------
        // Obtener control por ID
        // -------------------------------------------------
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _controlService.GetByIdAsync(id);

            if (record is null)
                return NotFound($"No existe control para id={id}");

            // Mapeo automático a DTO
            var dto = _mapper.Map<ControlGetDto>(record);

            return Ok(dto);
        }

        // -------------------------------------------------
        // Crear nuevo control
        // -------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ControlPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapeo DTO -> Modelo
            var control = _mapper.Map<Control>(dto);

            await _controlService.AddAsync(control);

            // Retornar DTO mapeado
            return CreatedAtAction(
                nameof(GetById),
                new { id = control.IdControl },
                _mapper.Map<ControlGetDto>(control)
            );
        }

        // -------------------------------------------------
        // Obtener todos los controles
        // -------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _controlService.GetAllAsync();

            // Mapeo lista de modelos -> lista de DTOs
            var dtos = _mapper.Map<List<ControlGetDto>>(records);

            return Ok(dtos);
        }

        // -------------------------------------------------
        // Eliminar control
        // -------------------------------------------------
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _controlService.DeleteAsync(id);
                return Ok(new { message = "Control eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
