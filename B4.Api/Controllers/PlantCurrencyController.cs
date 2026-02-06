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
    public class PlantCurrencyController : ControllerBase
    {
        private readonly IPlantCurrencyService _service;

        public PlantCurrencyController(IPlantCurrencyService service)
        {
            _service = service;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _service.GetByIdAsync(id);
            if (record == null)
                return NotFound($"No existe currency con id={id}");

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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _service.GetAllAsync();

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

            await _service.AddAsync(entity);

            return CreatedAtAction(nameof(GetById), new { id = entity.IdCurrency }, entity);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new { message = "Currency eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
