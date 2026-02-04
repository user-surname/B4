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
    public class PlantTreeController : ControllerBase
    {
        private readonly IPlantTreeRepository _treeRepo;

        public PlantTreeController(IPlantTreeRepository treeRepo)
        {
            _treeRepo = treeRepo;
        }

        // GET /api/v1/PlantTree/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var record = await _treeRepo.GetByIdAsync(id);

                if (record is null)
                    return NotFound($"No existe árbol para id={id}");

                var dto = new PlantTreeGetDto
                {
                    IdTree = record.IdTree,
                    IdDivision = record.IdDivision,
                    IdDivisionCompany = record.IdDivisionCompany,
                    IdSubdivision = record.IdSubdivision,
                    IdCountry = record.IdCountry,
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

        // GET /api/v1/PlantTree
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var records = await _treeRepo.GetAllAsync();

                var dtos = records.Select(record => new PlantTreeGetDto
                {
                    IdTree = record.IdTree,
                    IdDivision = record.IdDivision,
                    IdDivisionCompany = record.IdDivisionCompany,
                    IdSubdivision = record.IdSubdivision,
                    IdCountry = record.IdCountry,
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

        // POST /api/v1/PlantTree
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantTreePostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tree = new LkPlantTree
            {
                IdTree = dto.IdTree,
                IdDivision = dto.IdDivision,
                IdDivisionCompany = dto.IdDivisionCompany,
                IdSubdivision = dto.IdSubdivision,
                IdCountry = dto.IdCountry,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            await _treeRepo.AddAsync(tree);

            return CreatedAtAction(
                nameof(GetById),
                new { id = tree.IdTree },
                tree
            );
        }

        // DELETE /api/v1/PlantTree/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _treeRepo.GetByIdAsync(id);

            if (existing == null)
                return NotFound(new { message = $"No existe árbol con id={id}" });

            await _treeRepo.DeleteAsync(id);

            return Ok(new { message = "Árbol eliminado correctamente" });
        }
    }
}
