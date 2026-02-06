using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using B4.Api.Controllers;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities.LkEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace B4.Tests.Controllers
{
    public class PlantCurrencyControllerTest
    {
        private readonly Mock<IPlantCurrencyService> _mockService;
        private readonly PlantCurrencyController _controller;

        public PlantCurrencyControllerTest()
        {
            _mockService = new Mock<IPlantCurrencyService>();
            _controller = new PlantCurrencyController(_mockService.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var currency = new LkPlantCurrency
            {
                IdCurrency = 1,
                Currency = "USD",
                CurrencyAlias = "Dollar",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(currency);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<PlantCurrencyGetDto>(okResult.Value);

            Assert.Equal("USD", dto.Currency);
            Assert.Equal("Dollar", dto.CurrencyAlias);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((LkPlantCurrency)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantCurrency>
            {
                new LkPlantCurrency { IdCurrency = 1, Currency = "USD", CurrencyAlias = "Dollar" },
                new LkPlantCurrency { IdCurrency = 2, Currency = "EUR", CurrencyAlias = "Euro" }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<PlantCurrencyGetDto>>(okResult.Value).ToList();

            Assert.Equal(2, dtos.Count);
            Assert.Contains(dtos, d => d.Currency == "USD");
            Assert.Contains(dtos, d => d.Currency == "EUR");
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new PlantCurrencyPostDto
            {
                Currency = "GBP",
                CurrencyAlias = "Pound"
            };

            _mockService.Setup(s => s.AddAsync(It.IsAny<LkPlantCurrency>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdCurrency = Assert.IsType<LkPlantCurrency>(createdResult.Value);

            Assert.Equal("GBP", createdCurrency.Currency);
            Assert.Equal("Pound", createdCurrency.CurrencyAlias);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            var entity = new LkPlantCurrency { IdCurrency = 1, Currency = "USD", CurrencyAlias = "Dollar" };

            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(entity);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dict = okResult.Value.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("Currency eliminado correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new Exception("No existe currency con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var dict = notFoundResult.Value.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(notFoundResult.Value));

            Assert.Equal("No existe currency con id=99", dict["message"]);
        }
    }
}
