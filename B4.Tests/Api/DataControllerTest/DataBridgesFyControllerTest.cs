using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using B4.Api.Controllers;
using B4.Models.Entities.DataEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace B4.Tests.Api
{
    public class DataBridgesFyControllerTest
    {
        private readonly Mock<IDataBridgesFyService> _mockService;
        private readonly DataBridgesFyController _controller;

        public DataBridgesFyControllerTest()
        {
            _mockService = new Mock<IDataBridgesFyService>();
            _controller = new DataBridgesFyController(_mockService.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenExists()
        {
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new DataBridgesFy());

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DataBridgesFy>(ok.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((DataBridgesFy)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<DataBridgesFy> { new(), new() });

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsAssignableFrom<IEnumerable<DataBridgesFy>>(ok.Value);
            Assert.Equal(2, list.Count());
        }

        [Fact]
        public async Task Create_ShouldReturnOk()
        {
            _mockService.Setup(s => s.AddAsync(It.IsAny<DataBridgesFy>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(new DataBridgesFy());

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DataBridgesFy>(ok.Value);
        }

        [Fact]
        public async Task Update_ShouldReturnOk()
        {
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<DataBridgesFy>())).Returns(Task.CompletedTask);

            var result = await _controller.Update(1, new DataBridgesFy { Id = 1 });

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DataBridgesFy>(ok.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenThrows()
        {
            _mockService.Setup(s => s.DeleteAsync(99)).ThrowsAsync(new Exception("No existe"));

            var result = await _controller.Delete(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
