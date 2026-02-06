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

        public FasesController(IFasesService fasesService)
        {
            _fasesService = fasesService;
        }

        // GET /api/v1/Fases/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _fasesService.GetByIdAsync(id);

            if (record is null)
                return NotFound($"No existe fase para id={id}");

            var dto = new FasesGetDto
            {
                IdFase = record.IdFase,
                Fase = record.Fase,
                FaseAlias = record.FaseAlias,
                CreatedAt = record.CreatedAt,
                UpdatedAt = record.UpdatedAt,
                IsActive = record.IsActive
            };

            return Ok(dto);
        }

        // POST /api/v1/Fases
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FasesPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fase = new LkFases
            {
                Fase = dto.Fase,
                FaseAlias = dto.FaseAlias
            };

            await _fasesService.AddAsync(fase);

            return CreatedAtAction(
                nameof(GetById),
                new { id = fase.IdFase },
                fase
            );
        }

        // GET /api/v1/Fases
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _fasesService.GetAllAsync();

            var dtos = records.Select(record => new FasesGetDto
            {
                IdFase = record.IdFase,
                Fase = record.Fase,
                FaseAlias = record.FaseAlias,
                CreatedAt = record.CreatedAt,
                UpdatedAt = record.UpdatedAt,
                IsActive = record.IsActive
            }).ToList();

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
