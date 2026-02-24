using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using B4.Api.Controllers;
using B4.Models.Entities.DataEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace B4.Tests.Api
{
    public class DataBridgesFyBwEurControllerTest
    {
        private readonly Mock<IDataBridgesFyBwEurService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DataBridgesFyBwEurController _controller;

        public DataBridgesFyBwEurControllerTest()
        {
            _mockService = new Mock<IDataBridgesFyBwEurService>();
            _mockMapper = new Mock<IMapper>();

            _controller = new DataBridgesFyBwEurController(_mockService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenExists()
        {
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new DataBridgesFyBwEur());

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DataBridgesFyBwEur>(ok.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((DataBridgesFyBwEur)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetAllAsync())
                        .ReturnsAsync(new List<DataBridgesFyBwEur> { new(), new() });

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsAssignableFrom<IEnumerable<DataBridgesFyBwEur>>(ok.Value);
            Assert.Equal(2, list.Count());
        }
    }
}
