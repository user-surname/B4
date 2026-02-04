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
    public class PlantCompanyController : ControllerBase
    {
        private readonly IPlantCompanyRepository _plantRepo;

        public PlantCompanyController(IPlantCompanyRepository plantRepo)
        {
            _plantRepo = plantRepo;
        }

        // -----------------------------------------
        // GET BY ID
        // -----------------------------------------
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var record = await _plantRepo.GetByIdAsync(id);
                if (record is null)
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
                var records = await _plantRepo.GetAllAsync();
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // -----------------------------------------
        // CREATE / POST
        // -----------------------------------------
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
                Obs = dto.Obs,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            await _plantRepo.AddAsync(company);

            return CreatedAtAction(
                nameof(GetById),
                new { id = company.IdCompany },
                company
            );
        }

        // -----------------------------------------
        // DELETE
        // -----------------------------------------
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _plantRepo.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No existe PlantCompany con id={id}" });

            await _plantRepo.DeleteAsync(id);

            return Ok(new { message = "PlantCompany eliminado correctamente" });
        }
    }
}
