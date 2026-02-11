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
    public class DataBridgesFyBwControllerTest
    {
        private readonly Mock<IDataBridgesFyBwService> _mockService;
        private readonly DataBridgesFyBwController _controller;

        public DataBridgesFyBwControllerTest()
        {
            _mockService = new Mock<IDataBridgesFyBwService>();
            _controller = new DataBridgesFyBwController(_mockService.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenExists()
        {
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new DataBridgesFyBw());

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DataBridgesFyBw>(ok.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((DataBridgesFyBw)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<DataBridgesFyBw> { new(), new() });

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsAssignableFrom<IEnumerable<DataBridgesFyBw>>(ok.Value);
            Assert.Equal(2, list.Count());
        }
    }
}
