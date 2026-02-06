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
    public class PlantCompanyController : ControllerBase
    {
        private readonly IPlantCompanyService _plantService;

        public PlantCompanyController(IPlantCompanyService plantService)
        {
            _plantService = plantService;
        }

        // GET /api/v1/PlantCompany/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _plantService.GetByIdAsync(id);
            if (record == null)
                return NotFound($"No existe PlantCompany con id={id}");

            var dto = new PlantCompanyGetDto
            {
                IdCompany = record.IdCompany,
                CompanyCode = record.CompanyCode,
                ManagementCompany = record.ManagementCompany,
                IdCurrency = record.IdCurrency,
                Company = record.Company,
                Active = record.Active,
                IdDivision = record.IdDivision,
                IdDivisionCompany = record.IdDivisionCompany,
                IdSubdivision = record.IdSubdivision,
                IdCountry = record.IdCountry,
                Location = record.Location,
                Obs = record.Obs,
                CreatedAt = record.CreatedAt,
                UpdatedAt = record.UpdatedAt,
                IsActive = record.IsActive
            };

            return Ok(dto);
        }

        // GET /api/v1/PlantCompany
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _plantService.GetAllAsync();

            var dtos = records.Select(record => new PlantCompanyGetDto
            {
                IdCompany = record.IdCompany,
                CompanyCode = record.CompanyCode,
                ManagementCompany = record.ManagementCompany,
                IdCurrency = record.IdCurrency,
                Company = record.Company,
                Active = record.Active,
                IdDivision = record.IdDivision,
                IdDivisionCompany = record.IdDivisionCompany,
                IdSubdivision = record.IdSubdivision,
                IdCountry = record.IdCountry,
                Location = record.Location,
                Obs = record.Obs,
                CreatedAt = record.CreatedAt,
                UpdatedAt = record.UpdatedAt,
                IsActive = record.IsActive
            }).ToList();

            return Ok(dtos);
        }

        // POST /api/v1/PlantCompany
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlantCompanyPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var company = new LkPlantCompany
            {
                CompanyCode = dto.CompanyCode,
                ManagementCompany = dto.ManagementCompany,
                IdCurrency = dto.IdCurrency,
                Company = dto.Company,
                Active = dto.Active,
                IdDivision = dto.IdDivision,
                IdDivisionCompany = dto.IdDivisionCompany,
                IdSubdivision = dto.IdSubdivision,
                IdCountry = dto.IdCountry,
                Location = dto.Location,
                Obs = dto.Obs
            };

            await _plantService.AddAsync(company);

            return CreatedAtAction(nameof(GetById), new { id = company.IdCompany }, company);
        }

        // DELETE /api/v1/PlantCompany/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _plantService.DeleteAsync(id);
                return Ok(new { message = "PlantCompany eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
