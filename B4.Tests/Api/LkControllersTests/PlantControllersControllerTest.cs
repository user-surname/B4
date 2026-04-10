using AutoMapper;
using B4.Api.Controllers;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Api.Middleware;
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
            _mockService = new Mock<IPlantControllersService>();

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
            var dto = Assert.IsType<PlantControllersGetDto>(okResult.Value);

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
            var dtos = Assert.IsAssignableFrom<IEnumerable<PlantControllersGetDto>>(okResult.Value).ToList();

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
            var createdEntity = Assert.IsType<PlantControllersGetDto>(createdResult.Value);

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
            var dict = okResult.Value.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("PlantController eliminado correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new Exception("No existe PlantController con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var dict = notFoundResult.Value.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(notFoundResult.Value));

            Assert.Equal("No existe PlantController con id=99", dict["message"]);
        }
    }
}
