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
    public class DataActualsBwControllerTest
    {
        private readonly Mock<IDataActualsBwService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DataActualsBwController _controller;

        public DataActualsBwControllerTest()
        {
            _mockService = new Mock<IDataActualsBwService>();
            _mockMapper = new Mock<IMapper>();

            _controller = new DataActualsBwController(_mockService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenExists()
        {
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new DataActualsBw());

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DataActualsBw>(ok.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((DataActualsBw)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<DataActualsBw> { new(), new() });

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsAssignableFrom<IEnumerable<DataActualsBw>>(ok.Value);
            Assert.Equal(2, list.Count());
        }
    }
}
