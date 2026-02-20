using AutoMapper;
using B4.Api.Controllers;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Api.Middleware;
using B4.Models.Entities.LkEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace B4.Tests.Controllers
{
    public class PlantCompanyControllerTest : TestBase
    {
        private readonly Mock<IPlantCompanyService> _mockService;
        private readonly PlantCompanyController _controller;

        public PlantCompanyControllerTest()
        {
            _mockService = new Mock<IPlantCompanyService>();

            _controller = new PlantCompanyController(_mockService.Object, Mapper);
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

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(company);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<PlantCompanyGetDto>(okResult.Value);

            Assert.Equal("COMP01", dto.CompanyCode);
            Assert.Equal("Company A", dto.Company);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((LkPlantCompany)null);

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

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<PlantCompanyGetDto>>(okResult.Value).ToList();

            Assert.Equal(2, dtos.Count);
            Assert.Equal("COMP01", dtos[0].CompanyCode);
            Assert.Equal("COMP02", dtos[1].CompanyCode);
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

            _mockService.Setup(s => s.AddAsync(It.IsAny<LkPlantCompany>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdCompany = Assert.IsType<PlantCompanyGetDto>(createdResult.Value);

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

            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(company);

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
            _mockService.Setup(s => s.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new Exception("No existe PlantCompany con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var dict = notFoundResult.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(notFoundResult.Value));

            Assert.Equal("No existe PlantCompany con id=99", dict["message"]);
        }
    }
}
