using AutoMapper;
using B4.Api.Controllers;
using B4.Models.Entities.DataEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace B4.Tests.Api
{
    public class DataBridgesMonthBwControllerTest
    {
        private readonly Mock<IDataBridgesMonthBwService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DataBridgesMonthBwController _controller;
        private readonly ILogger<DataBridgesMonthBwController> _logger;


        public DataBridgesMonthBwControllerTest()
        {
            _mockService = new Mock<IDataBridgesMonthBwService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new DataBridgesMonthBwController(_mockService.Object, _mockMapper.Object, _logger);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenExists()
        {
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new DataBridgesMonthBw());

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DataBridgesMonthBw>(ok.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((DataBridgesMonthBw)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetAllAsync())
                        .ReturnsAsync(new List<DataBridgesMonthBw> { new(), new() });

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsAssignableFrom<IEnumerable<DataBridgesMonthBw>>(ok.Value);
            Assert.Equal(2, list.Count());
        }
    }
}
