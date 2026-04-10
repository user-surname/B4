using AutoMapper;
using B4.Api.Controllers;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Api.Middleware;
using B4.Models.Entities;
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

namespace B4.Data.PostgreSQL.Repositories
{
    public class ControlControllerTest
    {
        private readonly Mock<IControlService> _mockService;
        private readonly IMapper _mapper;
        private readonly ControlController _controller;
        private readonly ILogger<ControlController> _logger;



        public ControlControllerTest()
        {
            _mockService = new Mock<IControlService>();

            var expression = new MapperConfigurationExpression();
            expression.AddProfile<MappingProfile>();

            var config = new MapperConfiguration(
                expression,
                NullLoggerFactory.Instance
            );

            _mapper = config.CreateMapper();

            _controller = new ControlController(_mockService.Object, _mapper, _logger);
        }

        // -------------------------------------------------
        // GET BY ID
        // -------------------------------------------------
        [Fact]
        public async Task GetById_ShouldReturnOk_WhenExists()
        {
            var control = new Control
            {
                IdControl = 1,
                Anyo = 2024,
                IdCiclo = 2,
                IdFaseControl = 3,
                Activo = true
            };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(control);

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ControlGetDto>(ok.Value);

            Assert.Equal(2024, dto.Anyo);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Control)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // -------------------------------------------------
        // GET ALL
        // -------------------------------------------------
        [Fact]
        public async Task GetAll_ShouldReturnOk_WithList()
        {
            var list = new List<Control>
            {
                new Control { IdControl = 1, Anyo = 2024 },
                new Control { IdControl = 2, Anyo = 2025 }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsType<List<ControlGetDto>>(ok.Value);

            Assert.Equal(2, dtos.Count);
        }

        // -------------------------------------------------
        // CREATE
        // -------------------------------------------------
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new ControlPostDto
            {
                Anyo = 2026,
                IdCiclo = 1,
                IdFaseControl = 1,
                Activo = true
            };

            _mockService
                .Setup(s => s.AddAsync(It.IsAny<Control>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var createdDto = Assert.IsType<ControlGetDto>(created.Value);

            Assert.Equal(2026, createdDto.Anyo);
        }

        // -------------------------------------------------
        // DELETE
        // -------------------------------------------------
        [Fact]
        public async Task Delete_ShouldReturnOk_WhenExists()
        {
            _mockService
                .Setup(s => s.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var ok = Assert.IsType<OkObjectResult>(result);

            var dict = ok.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(ok.Value));

            Assert.Equal("Control eliminado correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenThrows()
        {
            _mockService
                .Setup(s => s.DeleteAsync(99))
                .ThrowsAsync(new Exception("No existe control con id=99"));

            var result = await _controller.Delete(99);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);

            var dict = notFound.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(notFound.Value));

            Assert.Equal("No existe control con id=99", dict["message"]);
        }
    }
}
