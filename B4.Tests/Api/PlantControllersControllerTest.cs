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
    public class PlantControllersControllerTest : TestBase
    {
        private readonly Mock<IPlantControllersService> _mockService;
        private readonly PlantControllersController _controller;
        private readonly ILogger<PlantControllersController> _logger;

        public PlantControllersControllerTest()
        {
            _mockService = CreateMock<IPlantControllersService>();
            _logger = NullLogger<PlantControllersController>.Instance;

            _controller = new PlantControllersController(_mockService.Object, Mapper, _logger);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var entity = new LkPlantControllers
            {
                IdCompanyController = 1,
                IdCompany = 10,
                Controller = "Juan Pérez",
                Email = "juan.perez@test.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(entity);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<PlantControllersGetDto>>(okResult.Value);
            var dto = response.Data;

            Assert.Equal("Juan Pérez", dto.Controller);
        }

[Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((LkPlantControllers)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

[Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantControllers>
            {
                new LkPlantControllers { IdCompanyController = 1, IdCompany = 10, Controller = "Juan Pérez", Email = "juan@test.com" },
                new LkPlantControllers { IdCompanyController = 2, IdCompany = 20, Controller = "Ana Gómez", Email = "ana@test.com" }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<List<PlantControllersGetDto>>>(okResult.Value);
            var dtos = response.Data;

            Assert.Equal(2, dtos.Count);
        }

[Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new PlantControllersPostDto
            {
                IdCompany = 30,
                Controller = "Luis Martínez",
                Email = "luis@test.com"
            };

            _mockService.Setup(s => s.AddAsync(It.IsAny<LkPlantControllers>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<PlantControllersGetDto>>(createdResult.Value);
            var createdEntity = response.Data;

            Assert.Equal("Luis Martínez", createdEntity.Controller);
        }

[Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            var entity = new LkPlantControllers { IdCompanyController = 1, Controller = "Juan Pérez" };

            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(entity);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

[Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new Exception("No existe PlantController con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
        }
    }
}
