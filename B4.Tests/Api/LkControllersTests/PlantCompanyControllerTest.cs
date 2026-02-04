using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using B4.Api.Controllers;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace B4.Tests.Controllers
{
    public class PlantCompanyControllerTest
    {
        private readonly Mock<IPlantCompanyRepository> _mockRepo;
        private readonly PlantCompanyController _controller;

        public PlantCompanyControllerTest()
        {
            _mockRepo = new Mock<IPlantCompanyRepository>();
            _controller = new PlantCompanyController(_mockRepo.Object);
        }

        // -----------------------------------------
        // TEST GET BY ID
        // -----------------------------------------
        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var company = new LkPlantCompany
            {
                IdCompany = 1,
                CompanyCode = "COMP01",
                ManagementCompany = "Gestión A",
                IdCurrency = 1,
                Company = "Company A",
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(company);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<PlantCompanyGetDto>(okResult.Value);

            Assert.Equal("COMP01", dto.CompanyCode);
            Assert.Equal("Company A", dto.Company);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkPlantCompany)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // -----------------------------------------
        // TEST GET ALL
        // -----------------------------------------
        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantCompany>
            {
                new LkPlantCompany { IdCompany = 1, CompanyCode = "COMP01", Company = "Company A" },
                new LkPlantCompany { IdCompany = 2, CompanyCode = "COMP02", Company = "Company B" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsType<List<PlantCompanyGetDto>>(okResult.Value);

            Assert.Equal(2, dtos.Count);
        }

        // -----------------------------------------
        // TEST POST CREATE
        // -----------------------------------------
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new PlantCompanyPostDto
            {
                CompanyCode = "COMP03",
                ManagementCompany = "Gestión C",
                IdCurrency = 1,
                Company = "Company C",
                Active = true,
                IdDivision = 1,
                IdDivisionCompany = 1,
                IdSubdivision = 1,
                IdCountry = 1,
                Location = "Location C",
                Obs = "Observación"
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<LkPlantCompany>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdCompany = Assert.IsType<LkPlantCompany>(createdResult.Value);

            Assert.Equal("COMP03", createdCompany.CompanyCode);
            Assert.Equal("Company C", createdCompany.Company);
        }

        // -----------------------------------------
        // TEST DELETE
        // -----------------------------------------
        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            var company = new LkPlantCompany { IdCompany = 1, CompanyCode = "COMP01", Company = "Company A" };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(company);
            _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var dict = okResult.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("PlantCompany eliminado correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkPlantCompany)null);

            var result = await _controller.Delete(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
