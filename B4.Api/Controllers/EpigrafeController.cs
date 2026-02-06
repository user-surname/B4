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

        public EpigrafeController(IEpigrafeService epigrafeService)
        {
            _epigrafeService = epigrafeService;
        }


        [HttpGet("{id:int}")]
        [DisabledEndpoint]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _epigrafeService.GetByIdAsync(id);

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
                EpigrafeFull = dto.EpigrafeFull
            };

            await _epigrafeService.AddAsync(epigrafe);

            return CreatedAtAction(
                nameof(GetById),
                new { id = epigrafe.IdEpigrafe },
                epigrafe
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _epigrafeService.GetAllAsync();

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
