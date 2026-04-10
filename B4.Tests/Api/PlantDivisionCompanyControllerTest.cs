using AutoMapper;
using B4.Api.Controllers;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Api.Middleware;
using B4.Models.Common;
using B4.Models.Entities.LkEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace B4.Tests.Controllers
{
    public class PlantDivisionCompanyControllerTest : TestBase
    {
        private readonly Mock<IPlantDivisionCompanyService> _mockService;
        private readonly PlantDivisionCompanyController _controller;
        private readonly ILogger<PlantDivisionCompanyController> _logger;

        public PlantDivisionCompanyControllerTest()
        {
            _mockService = CreateMock<IPlantDivisionCompanyService>();
            _logger = NullLogger<PlantDivisionCompanyController>.Instance;

            _controller = new PlantDivisionCompanyController(_mockService.Object, Mapper, _logger);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var division = new LkPlantDivisionCompany
            {
                IdDivisionCompany = 1,
                DivisionCompany = "Division A",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(division);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<PlantDivisionCompanyGetDto>>(okResult.Value);
            var dto = response.Data;

            Assert.Equal("Division A", dto.DivisionCompany);
        }

[Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((LkPlantDivisionCompany)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

[Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantDivisionCompany>
            {
                new LkPlantDivisionCompany { IdDivisionCompany = 1, DivisionCompany = "Division A" },
                new LkPlantDivisionCompany { IdDivisionCompany = 2, DivisionCompany = "Division B" }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<List<PlantDivisionCompanyGetDto>>>(okResult.Value);
            var dtos = response.Data;

            Assert.Equal(2, dtos.Count);
        }

[Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new PlantDivisionCompanyPostDto
            {
                DivisionCompany = "Division C"
            };

            _mockService.Setup(s => s.AddAsync(It.IsAny<LkPlantDivisionCompany>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<PlantDivisionCompanyGetDto>>(createdResult.Value);
            var createdDivision = response.Data;

            Assert.Equal("Division C", createdDivision.DivisionCompany);
        }

[Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            var entity = new LkPlantDivisionCompany { IdDivisionCompany = 1, DivisionCompany = "Division A" };

            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(entity);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

[Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new Exception("No existe division company con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
        }
    }
}
