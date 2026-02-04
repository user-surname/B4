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
    public class PlantCurrencyController : ControllerBase
    {
        private readonly IPlantCurrencyRepository _repo;

        public PlantCurrencyController(IPlantCurrencyRepository repo)
        {
            _repo = repo;
        }

        // -----------------------------------------
        // GET BY ID
        // -----------------------------------------
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var record = await _repo.GetByIdAsync(id);
                if (record == null)
                    return NotFound($"No existe currency para id={id}");

                var dto = new PlantCurrencyGetDto
                {
                    IdCurrency = record.IdCurrency,
                    Currency = record.Currency,
                    CurrencyAlias = record.CurrencyAlias,
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

        // -----------------------------------------
        // GET ALL
        // -----------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var records = await _repo.GetAllAsync();
                var dtos = records.Select(r => new PlantCurrencyGetDto
                {
                    IdCurrency = r.IdCurrency,
                    Currency = r.Currency,
                    CurrencyAlias = r.CurrencyAlias,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt,
                    IsActive = r.IsActive
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // -----------------------------------------
        // CREATE
        // -----------------------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantCurrencyPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = new LkPlantCurrency
            {
                Currency = dto.Currency,
                CurrencyAlias = dto.CurrencyAlias,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            await _repo.AddAsync(entity);

            return CreatedAtAction(nameof(GetById), new { id = entity.IdCurrency }, entity);
        }

        // -----------------------------------------
        // DELETE
        // -----------------------------------------
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No existe currency con id={id}" });

            await _repo.DeleteAsync(id);

            return Ok(new { message = "Currency eliminado correctamente" });
        }
    }
}
