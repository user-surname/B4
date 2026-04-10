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
    public class PlantTreeControllerTest : TestBase
    {
        private readonly Mock<IPlantTreeService> _mockService;
        private readonly PlantTreeController _controller;
        private readonly ILogger<PlantTreeController> _logger;

        public PlantTreeControllerTest()
        {
            _mockService = CreateMock<IPlantTreeService>();
            _logger = NullLogger<PlantTreeController>.Instance;

            _controller = new PlantTreeController(_mockService.Object, Mapper, _logger);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var tree = new LkPlantTree
            {
                IdTree = 1,
                IdDivision = 10,
                IdDivisionCompany = 20,
                IdSubdivision = 30,
                IdCountry = 40,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(tree);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<PlantTreeGetDto>>(okResult.Value);
            var dto = response.Data;

            Assert.Equal(10, dto.IdDivision);
            Assert.Equal(40, dto.IdCountry);
        }

[Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((LkPlantTree)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

[Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantTree>
            {
                new LkPlantTree { IdTree = 1, IdDivision = 10, IdDivisionCompany = 20, IdSubdivision = 30, IdCountry = 40 },
                new LkPlantTree { IdTree = 2, IdDivision = 11, IdDivisionCompany = 21, IdSubdivision = 31, IdCountry = 41 }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<List<PlantTreeGetDto>>>(okResult.Value);
            var dtos = response.Data;

            Assert.Equal(2, dtos.Count);
        }

[Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new PlantTreePostDto
            {
                IdTree = 3,
                IdDivision = 12,
                IdDivisionCompany = 22,
                IdSubdivision = 32,
                IdCountry = 42
            };

            _mockService.Setup(s => s.AddAsync(It.IsAny<LkPlantTree>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<PlantTreeGetDto>>(createdResult.Value);
            var createdTree = response.Data;

            Assert.Equal(12, createdTree.IdDivision);
            Assert.Equal(42, createdTree.IdCountry);
        }

[Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

[Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.DeleteAsync(99))
                        .ThrowsAsync(new Exception("No existe árbol con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
        }
    }
}
