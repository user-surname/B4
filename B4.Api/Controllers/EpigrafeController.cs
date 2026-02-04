using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Api.Middleware;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EpigrafeController : ControllerBase
    {
        private readonly IEpigrafeRepository _epigrafeRepo;

        public EpigrafeController(IEpigrafeRepository epigrafeRepo)
        {
            _epigrafeRepo = epigrafeRepo;
        }

        // GET /api/v1/Epigrafe/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var record = await _epigrafeRepo.GetByIdAsync(id);

                if (record == null)
                    return NotFound($"No existe epígrafe para id={id}");

                var dto = new EpigrafeGetDto
                {
                    IdEpigrafe = record.IdEpigrafe,
                    IdPlantilla = record.IdPlantilla,
                    IdHoja = record.IdHoja,
                    PreEpigrafe = record.PreEpigrafe,
                    Epigrafe = record.Epigrafe,
                    EpigrafeFull = record.EpigrafeFull,
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

        // POST /api/v1/Epigrafe
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EpigrafePostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var epigrafe = new LkEpigrafe
            {
                IdPlantilla = dto.IdPlantilla,
                IdHoja = dto.IdHoja,
                PreEpigrafe = dto.PreEpigrafe,
                Epigrafe = dto.Epigrafe,
                EpigrafeFull = dto.EpigrafeFull,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            await _epigrafeRepo.AddAsync(epigrafe);

            return CreatedAtAction(
                nameof(GetById),
                new { id = epigrafe.IdEpigrafe },
                epigrafe
            );
        }

        // GET /api/v1/Epigrafe
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var records = await _epigrafeRepo.GetAllAsync();

                var dtos = records.Select(record => new EpigrafeGetDto
                {
                    IdEpigrafe = record.IdEpigrafe,
                    IdPlantilla = record.IdPlantilla,
                    IdHoja = record.IdHoja,
                    PreEpigrafe = record.PreEpigrafe,
                    Epigrafe = record.Epigrafe,
                    EpigrafeFull = record.EpigrafeFull,
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

        // DELETE /api/v1/Epigrafe/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _epigrafeRepo.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No existe epígrafe con id={id}" });

            await _epigrafeRepo.DeleteAsync(id);

            return Ok(new { message = "Epígrafe eliminado correctamente" });
        }
    }
}
