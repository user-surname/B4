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
    public class PlantillasBotonesPasosTiposControllerTest : TestBase
    {
        private readonly Mock<IPlantillasBotonesPasosTiposService> _mockService;
        private readonly PlantillasBotonesPasosTiposController _controller;
        private readonly ILogger<PlantillasBotonesPasosTiposController> _logger;

        public PlantillasBotonesPasosTiposControllerTest()
        {
            _mockService = CreateMock<IPlantillasBotonesPasosTiposService>();
            _logger = NullLogger<PlantillasBotonesPasosTiposController>.Instance;

            _controller = new PlantillasBotonesPasosTiposController(_mockService.Object, Mapper, _logger);
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
            var response = Assert.IsType<ApiResponse<PlantillasBotonesPasosTiposGetDto>>(okResult.Value);
            var dto = response.Data;

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
            var response = Assert.IsType<ApiResponse<List<PlantillasBotonesPasosTiposGetDto>>>(okResult.Value);
            var dtos = response.Data;

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
            var response = Assert.IsType<ApiResponse<PlantillasBotonesPasosTiposGetDto>>(createdResult.Value);
            var createdEntity = response.Data;

            Assert.Equal("Tipo C", createdEntity.Pasotipo);
        }

[Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            _mockService.Setup(s => s.DeleteAsync(101)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(101);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

[Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.DeleteAsync(99))
                        .ThrowsAsync(new Exception("No existe paso tipo con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
        }
    }
}
