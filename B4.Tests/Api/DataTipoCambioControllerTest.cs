using AutoMapper;
using B4.Api.Controllers;
using B4.Models.Common;
using B4.Models.Entities.DataEntities;
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

namespace B4.Tests.Api.Controllers
{
    public class DataTipoCambioControllerTest : TestBase
    {
        private readonly Mock<IDataTipoCambioService> _mockService;
        private readonly DataTipoCambioController _controller;
        private readonly ILogger<DataTipoCambioController> _logger;
public DataTipoCambioControllerTest()
        {
            _logger = NullLogger<DataTipoCambioController>.Instance;
            _mockService = CreateMock<IDataTipoCambioService>();
            _controller = new DataTipoCambioController(_mockService.Object, Mapper, _logger);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenExists()
        {
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new DataTipoCambio());

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((DataTipoCambio)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetAllAsync())
                        .ReturnsAsync(new List<DataTipoCambio> { new(), new() });

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
        }

        [Fact]
        public async Task GetByEjercicio_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetByEjercicioAsync(2024))
                        .ReturnsAsync(new List<DataTipoCambio> { new(), new(), new() });

            var result = await _controller.GetByEjercicio(2024);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
        }

        [Fact]
        public async Task GetByEjercicioCurrency_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetByEjercicioCurrencyAsync(2024, 2))
                        .ReturnsAsync(new List<DataTipoCambio> { new() });

            var result = await _controller.GetByEjercicioCurrency(2024, 2);

            var ok = Assert.IsType<OkObjectResult>(result);

            var response = Assert.IsType<ApiResponse<IEnumerable<DataTipoCambio>>>(ok.Value);

            var list = response.Data;

            Assert.Single(list);
        }

        [Fact]
        public async Task Create_ShouldReturnOk()
        {
            _mockService.Setup(s => s.AddAsync(It.IsAny<DataTipoCambio>()))
                        .Returns(Task.CompletedTask);

            var result = await _controller.Create(new DataTipoCambio());

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
        }

        [Fact]
        public async Task Update_ShouldReturnOk()
        {
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<DataTipoCambio>()))
                        .Returns(Task.CompletedTask);

            var result = await _controller.Update(1, new DataTipoCambio { Id = 1 });

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
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
