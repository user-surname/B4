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

namespace B4.Tests.Api.Controllers
{
    public class PlantSubdivisionControllerTest : TestBase
    {
        private readonly Mock<IPlantSubdivisionService> _mockService;
        private readonly PlantSubdivisionController _controller;
        private readonly ILogger<PlantSubdivisionController> _logger;

        public PlantSubdivisionControllerTest()
        {
            _mockService = CreateMock<IPlantSubdivisionService>();
            _logger = NullLogger<PlantSubdivisionController>.Instance;

            _controller = new PlantSubdivisionController(_mockService.Object, Mapper, _logger);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var subdivision = new LkPlantSubdivision
            {
                IdSubdivision = 100,
                Subdivision = "Subdivisión A",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(subdivision);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<PlantSubdivisionGetDto>>(okResult.Value);
            var dto = response.Data;

            Assert.Equal("Subdivisión A", dto.Subdivision);
        }

[Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((LkPlantSubdivision)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

[Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantSubdivision>
            {
                new LkPlantSubdivision { IdSubdivision = 100, Subdivision = "Subdivisión A" },
                new LkPlantSubdivision { IdSubdivision = 101, Subdivision = "Subdivisión B" }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<List<PlantSubdivisionGetDto>>>(okResult.Value);
            var dtos = response.Data;

            Assert.Equal(2, dtos.Count);
        }

[Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new PlantSubdivisionPostDto
            {
                IdSubdivision = 102,
                Subdivision = "Subdivisión C"
            };

            _mockService.Setup(s => s.AddAsync(It.IsAny<LkPlantSubdivision>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<PlantSubdivisionGetDto>>(createdResult.Value);
            var createdSubdivision = response.Data;

            Assert.Equal("Subdivisión C", createdSubdivision.Subdivision);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            _mockService.Setup(s => s.GetByIdAsync(1))
                        .ReturnsAsync(new LkPlantSubdivision { IdSubdivision = 1, Subdivision = "Subdivisión A" });

            _mockService.Setup(s => s.DeleteAsync(1))
                        .Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.NotNull(okResult.Value);
        }

[Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.DeleteAsync(99))
                        .ThrowsAsync(new Exception("No existe subdivisión con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
        }
    }
}
