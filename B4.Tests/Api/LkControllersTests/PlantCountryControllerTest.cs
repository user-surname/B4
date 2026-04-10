using AutoMapper;
using B4.Api.Controllers;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Api.Middleware;
using B4.Models.Entities.LkEntities;
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

namespace B4.Tests.Controllers
{
    public class PlantCountryControllerTest : TestBase
    {
        private readonly Mock<IPlantCountryService> _mockService;
        private readonly PlantCountryController _controller;
        private readonly ILogger<PlantCountryController> _logger;


        public PlantCountryControllerTest()
        {
            _mockService = new Mock<IPlantCountryService>();

            _controller = new PlantCountryController(_mockService.Object, Mapper, _logger);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var country = new LkPlantCountry
            {
                IdCountry = 1,
                Country = "España",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(country);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<PlantCountryGetDto>(okResult.Value);

            Assert.Equal("España", dto.Country);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((LkPlantCountry)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantCountry>
            {
                new LkPlantCountry { IdCountry = 1, Country = "España" },
                new LkPlantCountry { IdCountry = 2, Country = "Francia" }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<PlantCountryGetDto>>(okResult.Value).ToList();

            Assert.Equal(2, dtos.Count);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new PlantCountryPostDto { Country = "Italia" };

            _mockService.Setup(s => s.AddAsync(It.IsAny<LkPlantCountry>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdCountry = Assert.IsType<PlantCountryGetDto>(createdResult.Value);

            Assert.Equal("Italia", createdCountry.Country);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            var entity = new LkPlantCountry { IdCountry = 1, Country = "España" };

            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(entity);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dict = okResult.Value.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("País eliminado correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new Exception("No existe país con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var dict = notFoundResult.Value.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(notFoundResult.Value));

            Assert.Equal("No existe país con id=99", dict["message"]);
        }
    }
}
