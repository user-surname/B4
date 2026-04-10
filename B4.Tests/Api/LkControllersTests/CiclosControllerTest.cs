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
    public class CiclosControllerTest : TestBase
    {
        private readonly Mock<ICiclosService> _mockService;
        private readonly CiclosController _controller;
        private readonly ILogger<CiclosController> _logger;

        public CiclosControllerTest()
        {
            _mockService = CreateMock<ICiclosService>();
            _logger = NullLogger<CiclosController>.Instance;

            _controller = new CiclosController(_mockService.Object, Mapper, _logger);
        }


        // -----------------------------------------
        // TEST GET BY ID
        // -----------------------------------------
        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var ciclo = new LkCiclos
            {
                Id = 1,
                IdCiclo = 2024,
                Ciclo = "Ciclo 2024",
                Descripcion = "Desc 2024",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(ciclo);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var response = Assert.IsType<ApiResponse<CiclosGetDto>>(okResult.Value);

            Assert.Equal("Ciclo 2024", response.Data.Ciclo);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((LkCiclos)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // -----------------------------------------
        // TEST GET ALL
        // -----------------------------------------
        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkCiclos>
            {
                new LkCiclos { Id = 1, IdCiclo = 2024, Ciclo = "Ciclo 2024", Descripcion = "Desc 1", IsActive = 1 },
                new LkCiclos { Id = 2, IdCiclo = 2025, Ciclo = "Ciclo 2025", Descripcion = "Desc 2", IsActive = 1 }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);

            var response = Assert.IsType<ApiResponse<List<CiclosGetDto>>>(okResult.Value);

            Assert.Equal(2, response.Data.Count);
        }

        // -----------------------------------------
        // TEST POST CREATE
        // -----------------------------------------
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var dto = new CiclosPostDto
            {
                IdCiclo = 2026,
                Ciclo = "Ciclo 2026",
                Descripcion = "Desc 2026"
            };

            _mockService.Setup(s => s.AddAsync(It.IsAny<LkCiclos>()))
                        .Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<CiclosGetDto>>(createdResult.Value);

            Assert.Equal("Ciclo 2026", response.Data.Ciclo);
        }

        // -----------------------------------------
        // TEST DELETE
        // -----------------------------------------
        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            // Arrange
            _mockService.Setup(s => s.GetByIdAsync(1))
                        .ReturnsAsync(new LkCiclos { Id = 1 });

            _mockService.Setup(s => s.DeleteAsync(1))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService
                .Setup(s => s.DeleteAsync(99))
                .ThrowsAsync(new KeyNotFoundException("No existe ciclo con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            var response = Assert.IsType<ApiResponse<CiclosGetDto>>(notFoundResult.Value);

            Assert.Equal("No existe ciclo con id=99", response.Msg);
        }
    }
}
