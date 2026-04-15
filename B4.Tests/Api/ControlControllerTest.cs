using AutoMapper;
using B4.Api.Controllers;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Api.Middleware;
using B4.Models.Common;
using B4.Models.Entities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace B4.Tests.Api.Controllers
{
    public class ControlControllerTest : TestBase
    {
        private readonly Mock<IControlService> _mockService;
        private readonly ControlController _controller;
        private readonly ILogger<ControlController> _logger;

        public ControlControllerTest()
        {
            _mockService = CreateMock<IControlService>();
            _logger = NullLogger<ControlController>.Instance;

            _controller = new ControlController(_mockService.Object, Mapper, _logger);
        }

        // -----------------------------------------
        // TEST GET BY ID
        // -----------------------------------------
        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var control = new Control
            {
                IdControl = 1,
                Anyo = 2024,
                IdCiclo = 10,
                IdFaseControl = 2,
                Inicio = DateTime.UtcNow.AddDays(-1),
                Final = DateTime.UtcNow,
                Activo = true
            };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(control);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var response = Assert.IsType<ApiResponse<ControlGetDto>>(okResult.Value);

            Assert.Equal(2024, response.Data.Anyo);
            Assert.True(response.Data.Activo);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Control)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // -----------------------------------------
        // TEST GET ALL
        // -----------------------------------------
        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<Control>
            {
                new Control { IdControl = 1, Anyo = 2024, IdCiclo = 10, IdFaseControl = 2, Activo = true },
                new Control { IdControl = 2, Anyo = 2025, IdCiclo = 11, IdFaseControl = 3, Activo = true }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);

            var response = Assert.IsType<ApiResponse<List<ControlGetDto>>>(okResult.Value);

            Assert.Equal(2, response.Data.Count);
        }

        // -----------------------------------------
        // TEST POST CREATE
        // -----------------------------------------
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new ControlPostDto
            {
                IdControl = 3,
                Anyo = 2026,
                IdCiclo = 12,
                IdFaseControl = 4,
                Inicio = DateTime.UtcNow,
                Final = DateTime.UtcNow.AddDays(1),
                Activo = true
            };

            _mockService.Setup(s => s.AddAsync(It.IsAny<Control>()))
                        .Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);

            var response = Assert.IsType<ApiResponse<ControlGetDto>>(createdResult.Value);

            Assert.Equal(2026, response.Data.Anyo);
            Assert.True(response.Data.Activo);
        }

        // -----------------------------------------
        // TEST DELETE
        // -----------------------------------------
        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            _mockService.Setup(s => s.GetByIdAsync(1))
                        .ReturnsAsync(new Control { IdControl = 1 });

            _mockService.Setup(s => s.DeleteAsync(1))
                        .Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService
                .Setup(s => s.DeleteAsync(99))
                .ThrowsAsync(new KeyNotFoundException("No existe control con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            var response = Assert.IsType<ApiResponse<ControlGetDto>>(notFoundResult.Value);

            Assert.Equal("No existe control con id=99", response.Msg);
        }
    }
}