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
    public class PlantillasBotonesPasosTiposControllerTest
    {
        private readonly Mock<IPlantillasBotonesPasosTiposRepository> _mockRepo;
        private readonly PlantillasBotonesPasosTiposController _controller;

        public PlantillasBotonesPasosTiposControllerTest()
        {
            _mockRepo = new Mock<IPlantillasBotonesPasosTiposRepository>();
            _controller = new PlantillasBotonesPasosTiposController(_mockRepo.Object);
        }

        // -----------------------------------------
        // TEST GET BY ID
        // -----------------------------------------
        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var pasoTipo = new LkPlantillasBotonesPasosTipos
            {
                IdPasoTipo = 101,
                Pasotipo = "Tipo A",
                Descripcion = "Desc A",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pasoTipo);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<PlantillasBotonesPasosTiposGetDto>(okResult.Value);

            Assert.Equal("Tipo A", dto.Pasotipo);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkPlantillasBotonesPasosTipos)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // -----------------------------------------
        // TEST GET ALL
        // -----------------------------------------
        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantillasBotonesPasosTipos>
            {
                new LkPlantillasBotonesPasosTipos { IdPasoTipo = 101, Pasotipo = "Tipo A", Descripcion = "Desc A" },
                new LkPlantillasBotonesPasosTipos { IdPasoTipo = 102, Pasotipo = "Tipo B", Descripcion = "Desc B" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsType<List<PlantillasBotonesPasosTiposGetDto>>(okResult.Value);

            Assert.Equal(2, dtos.Count);
        }

        // -----------------------------------------
        // TEST POST CREATE
        // -----------------------------------------
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new PlantillasBotonesPasosTiposPostDto
            {
                IdPasoTipo = 103,
                Pasotipo = "Tipo C",
                Descripcion = "Desc C"
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<LkPlantillasBotonesPasosTipos>()))
                     .Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdEntity = Assert.IsType<LkPlantillasBotonesPasosTipos>(createdResult.Value);

            Assert.Equal("Tipo C", createdEntity.Pasotipo);
        }

        // -----------------------------------------
        // TEST DELETE
        // -----------------------------------------
        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            var pasoTipo = new LkPlantillasBotonesPasosTipos { IdPasoTipo = 101, Pasotipo = "Tipo A" };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pasoTipo);
            _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            // Convertimos a diccionario para verificar el mensaje
            var dict = okResult.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("Paso tipo eliminado correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkPlantillasBotonesPasosTipos)null);

            var result = await _controller.Delete(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
