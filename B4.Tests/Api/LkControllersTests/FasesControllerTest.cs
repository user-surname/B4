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
    public class FasesControllerTest : TestBase
    {
        private readonly Mock<IFasesService> _mockService;
        private readonly FasesController _controller;
        private readonly ILogger<FasesController> _logger;

        public FasesControllerTest()
        {
            _mockService = new Mock<IFasesService>();

            _controller = new FasesController(_mockService.Object, Mapper, _logger);
        }

        // -----------------------------------------
        // TEST GET BY ID
        // -----------------------------------------
        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var fase = new LkFases
            {
                IdFase = 1,
                Fase = "Fase 1",
                FaseAlias = "F1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(fase);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<FasesGetDto>(okResult.Value);

            Assert.Equal("Fase 1", dto.Fase);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((LkFases)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // -----------------------------------------
        // TEST GET ALL
        // -----------------------------------------
        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkFases>
            {
                new LkFases { IdFase = 1, Fase = "Fase 1", FaseAlias = "F1", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, IsActive = 1 },
                new LkFases { IdFase = 2, Fase = "Fase 2", FaseAlias = "F2", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, IsActive = 1 }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsType<List<FasesGetDto>>(okResult.Value);

            Assert.Equal(2, dtos.Count);
        }

        // -----------------------------------------
        // TEST POST CREATE
        // -----------------------------------------
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new FasesPostDto
            {
                Fase = "Fase 3",
                FaseAlias = "F3"
            };

            _mockService.Setup(s => s.AddAsync(It.IsAny<LkFases>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdFase = Assert.IsType<FasesGetDto>(createdResult.Value);

            Assert.Equal("Fase 3", createdFase.Fase);
        }

        // -----------------------------------------
        // TEST DELETE
        // -----------------------------------------
        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            // Convertimos a diccionario para validar el mensaje
            var dict = okResult.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("Fase eliminada correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService
                .Setup(s => s.DeleteAsync(99))
                .ThrowsAsync(new Exception("No existe fase con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            var dict = notFoundResult.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(notFoundResult.Value));

            Assert.Equal("No existe fase con id=99", dict["message"]);
        }
    }
}
