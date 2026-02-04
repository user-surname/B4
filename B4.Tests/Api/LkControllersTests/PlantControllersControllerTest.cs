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
    public class PlantControllersControllerTest
    {
        private readonly Mock<IPlantControllersRepository> _mockRepo;
        private readonly PlantControllersController _controller;

        public PlantControllersControllerTest()
        {
            _mockRepo = new Mock<IPlantControllersRepository>();
            _controller = new PlantControllersController(_mockRepo.Object);
        }

        // -----------------------------------------
        // TEST GET BY ID
        // -----------------------------------------
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

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<PlantControllersGetDto>(okResult.Value);

            Assert.Equal("Juan Pérez", dto.Controller);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkPlantControllers)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // -----------------------------------------
        // TEST GET ALL
        // -----------------------------------------
        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantControllers>
            {
                new LkPlantControllers { IdCompanyController = 1, IdCompany = 10, Controller = "Juan Pérez", Email = "juan@test.com" },
                new LkPlantControllers { IdCompanyController = 2, IdCompany = 20, Controller = "Ana Gómez", Email = "ana@test.com" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsType<List<PlantControllersGetDto>>(okResult.Value);

            Assert.Equal(2, dtos.Count);
        }

        // -----------------------------------------
        // TEST POST CREATE
        // -----------------------------------------
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new PlantControllersPostDto
            {
                IdCompany = 30,
                Controller = "Luis Martínez",
                Email = "luis@test.com"
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<LkPlantControllers>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdEntity = Assert.IsType<LkPlantControllers>(createdResult.Value);

            Assert.Equal("Luis Martínez", createdEntity.Controller);
        }

        // -----------------------------------------
        // TEST DELETE
        // -----------------------------------------
        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            var entity = new LkPlantControllers
            {
                IdCompanyController = 1,
                Controller = "Juan Pérez"
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
            _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            // Convertimos a diccionario para verificar el mensaje
            var dict = okResult.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("PlantController eliminado correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkPlantControllers)null);

            var result = await _controller.Delete(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
