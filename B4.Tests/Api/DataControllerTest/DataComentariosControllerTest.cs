using AutoMapper;
using B4.Api.Controllers;
using B4.Models.Entities.DataEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace B4.Tests.Api
{
    public class DataComentariosControllerTest
    {
        private readonly Mock<IDataComentariosService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DataComentariosController _controller;
        private readonly ILogger<DataComentariosController> _logger;


        public DataComentariosControllerTest()
        {
            _mockService = new Mock<IDataComentariosService>();
            _mockMapper = new Mock<IMapper>();

            _controller = new DataComentariosController(_mockService.Object, _mockMapper.Object, _logger);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenExists()
        {
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new DataComentarios());

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DataComentarios>(ok.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((DataComentarios)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetAllAsync())
                        .ReturnsAsync(new List<DataComentarios> { new(), new() });

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsAssignableFrom<IEnumerable<DataComentarios>>(ok.Value);
            Assert.Equal(2, new List<DataComentarios>(list).Count);
        }

        [Fact]
        public async Task Create_ShouldReturnOk()
        {
            _mockService.Setup(s => s.AddAsync(It.IsAny<DataComentarios>()))
                        .Returns(Task.CompletedTask);

            var result = await _controller.Create(new DataComentarios());

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DataComentarios>(ok.Value);
        }

        [Fact]
        public async Task Update_ShouldReturnOk()
        {
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<DataComentarios>()))
                        .Returns(Task.CompletedTask);

            var result = await _controller.Update(1, new DataComentarios { Id = 1 });

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DataComentarios>(ok.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk()
        {
            _mockService.Setup(s => s.DeleteAsync(1))
                        .Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenThrows()
        {
            _mockService.Setup(s => s.DeleteAsync(99))
                        .ThrowsAsync(new Exception("No existe"));

            var result = await _controller.Delete(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
