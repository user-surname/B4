using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using B4.Api.Controllers;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace B4.Tests.Controllers
{
    public class PlantCurrencyControllerTest
    {
        private readonly Mock<IPlantCurrencyRepository> _mockRepo;
        private readonly PlantCurrencyController _controller;

        public PlantCurrencyControllerTest()
        {
            _mockRepo = new Mock<IPlantCurrencyRepository>();
            _controller = new PlantCurrencyController(_mockRepo.Object);
        }

        // -----------------------------------------
        // TEST GET BY ID
        // -----------------------------------------
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

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(currency);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<PlantCurrencyGetDto>(okResult.Value);

            Assert.Equal("USD", dto.Currency);
            Assert.Equal("Dollar", dto.CurrencyAlias);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkPlantCurrency)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // -----------------------------------------
        // TEST GET ALL
        // -----------------------------------------
        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantCurrency>
            {
                new LkPlantCurrency { IdCurrency = 1, Currency = "USD", CurrencyAlias = "Dollar" },
                new LkPlantCurrency { IdCurrency = 2, Currency = "EUR", CurrencyAlias = "Euro" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsType<List<PlantCurrencyGetDto>>(okResult.Value);

            Assert.Equal(2, dtos.Count);
            Assert.Contains(dtos, d => d.Currency == "USD");
            Assert.Contains(dtos, d => d.Currency == "EUR");
        }

        // -----------------------------------------
        // TEST POST CREATE
        // -----------------------------------------
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new PlantCurrencyPostDto
            {
                Currency = "GBP",
                CurrencyAlias = "Pound"
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<LkPlantCurrency>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdCurrency = Assert.IsType<LkPlantCurrency>(createdResult.Value);

            Assert.Equal("GBP", createdCurrency.Currency);
            Assert.Equal("Pound", createdCurrency.CurrencyAlias);
        }

        // -----------------------------------------
        // TEST DELETE
        // -----------------------------------------
        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            var currency = new LkPlantCurrency { IdCurrency = 1, Currency = "USD", CurrencyAlias = "Dollar" };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(currency);
            _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var dict = okResult.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("Currency eliminado correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkPlantCurrency)null);

            var result = await _controller.Delete(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
