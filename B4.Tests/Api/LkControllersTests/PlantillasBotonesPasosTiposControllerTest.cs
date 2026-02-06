using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using B4.Api.Controllers;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities.LkEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace B4.Tests.Controllers
{
    public class PlantillasBotonesPasosTiposControllerTest
    {
        private readonly Mock<IPlantillasBotonesPasosTiposService> _mockService;
        private readonly PlantillasBotonesPasosTiposController _controller;

        public PlantillasBotonesPasosTiposControllerTest()
        {
            _mockService = new Mock<IPlantillasBotonesPasosTiposService>();
            _controller = new PlantillasBotonesPasosTiposController(_mockService.Object);
        }

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

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(pasoTipo);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<PlantillasBotonesPasosTiposGetDto>(okResult.Value);

            Assert.Equal("Tipo A", dto.Pasotipo);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((LkPlantillasBotonesPasosTipos)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantillasBotonesPasosTipos>
            {
                new LkPlantillasBotonesPasosTipos { IdPasoTipo = 101, Pasotipo = "Tipo A", Descripcion = "Desc A" },
                new LkPlantillasBotonesPasosTipos { IdPasoTipo = 102, Pasotipo = "Tipo B", Descripcion = "Desc B" }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<PlantillasBotonesPasosTiposGetDto>>(okResult.Value).ToList();

            Assert.Equal(2, dtos.Count);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new PlantillasBotonesPasosTiposPostDto
            {
                IdPasoTipo = 103,
                Pasotipo = "Tipo C",
                Descripcion = "Desc C"
            };

            _mockService.Setup(s => s.AddAsync(It.IsAny<LkPlantillasBotonesPasosTipos>()))
                        .Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdEntity = Assert.IsType<LkPlantillasBotonesPasosTipos>(createdResult.Value);

            Assert.Equal("Tipo C", createdEntity.Pasotipo);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            _mockService.Setup(s => s.DeleteAsync(101)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(101);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dict = okResult.Value.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("Paso tipo eliminado correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.DeleteAsync(99))
                        .ThrowsAsync(new Exception("No existe paso tipo con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var dict = notFoundResult.Value.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(notFoundResult.Value));

            Assert.Equal("No existe paso tipo con id=99", dict["message"]);
        }
    }
}
