using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FasesController : ControllerBase
    {
        private readonly IFasesRepository _fasesRepo;

        public FasesController(IFasesRepository fasesRepo)
        {
            _fasesRepo = fasesRepo;
        }

        // GET /api/v1/Fases/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var record = await _fasesRepo.GetByIdAsync(id);

                if (record == null)
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
                FaseAlias = dto.FaseAlias,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            await _fasesRepo.AddAsync(fase);

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
            try
            {
                var records = await _fasesRepo.GetAllAsync();

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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE /api/v1/Fases/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _fasesRepo.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No existe fase con id={id}" });

            await _fasesRepo.DeleteAsync(id);

            return Ok(new { message = "Fase eliminada correctamente" });
        }
    }
}
