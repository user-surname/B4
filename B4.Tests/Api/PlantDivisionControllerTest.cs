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
    public class PlantDivisionControllerTest : TestBase
    {
        private readonly Mock<IPlantDivisionService> _mockService;
        private readonly PlantDivisionController _controller;
        private readonly ILogger<PlantDivisionController> _logger;

        public PlantDivisionControllerTest()
        {
            _mockService = CreateMock<IPlantDivisionService>();
            _logger = NullLogger<PlantDivisionController>.Instance;

            _controller = new PlantDivisionController(_mockService.Object, Mapper, _logger);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var division = new LkPlantDivision
            {
                IdDivision = 101,
                Division = "Division A",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(division);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<PlantDivisionGetDto>>(okResult.Value);
            var dto = response.Data;

            Assert.Equal("Division A", dto.Division);
        }

[Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((LkPlantDivision)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

[Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantDivision>
            {
                new LkPlantDivision { IdDivision = 101, Division = "Division A" },
                new LkPlantDivision { IdDivision = 102, Division = "Division B" }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<List<PlantDivisionGetDto>>>(okResult.Value);
            var dtos = response.Data;

            Assert.Equal(2, dtos.Count);
        }

[Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new PlantDivisionPostDto
            {
                Division = "Division C"
            };

            _mockService.Setup(s => s.AddAsync(It.IsAny<LkPlantDivision>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<PlantDivisionGetDto>>(createdResult.Value);
            var createdDivision = response.Data;

            Assert.Equal("Division C", createdDivision.Division);
        }

[Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            var division = new LkPlantDivision { IdDivision = 101, Division = "Division A" };

            _mockService.Setup(s => s.DeleteAsync(101)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(101);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

[Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.DeleteAsync(99)).ThrowsAsync(new Exception("No existe división con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
        }
    }
}
