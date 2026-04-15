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
using Microsoft.Extensions.Logging.Abstractions;

namespace B4.Tests.Api.Controllers
{
    public class DataForecastBwControllerTest : TestBase
    {
        private readonly Mock<IDataForecastBwService> _mockService;
        private readonly DataForecastBwController _controller;
        private readonly ILogger<DataForecastBwController> _logger;
public DataForecastBwControllerTest()
        {
            _logger = NullLogger<DataForecastBwController>.Instance;
            _mockService = CreateMock<IDataForecastBwService>();
            _controller = new DataForecastBwController(_mockService.Object, Mapper, _logger);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenExists()
        {
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new DataForecastBw());

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((DataForecastBw)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetAllAsync())
                        .ReturnsAsync(new List<DataForecastBw> { new(), new() });

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
        }
    }
}
