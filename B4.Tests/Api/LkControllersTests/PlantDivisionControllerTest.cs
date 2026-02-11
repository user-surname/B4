using AutoMapper;
using B4.Api.Controllers;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Api.Middleware;
using B4.Models.Entities.LkEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace B4.Tests.Controllers
{
    public class PlantDivisionControllerTest
    {
        private readonly Mock<IPlantDivisionService> _mockService;
        private readonly PlantDivisionController _controller;

        public PlantDivisionControllerTest()
        {
            _mockService = new Mock<IPlantDivisionService>();

            var expression = new MapperConfigurationExpression();
            expression.AddProfile<MappingProfile>();

            var config = new MapperConfiguration(
                expression,
                NullLoggerFactory.Instance
            );

            var mapper = config.CreateMapper();

            _controller = new PlantDivisionController(_mockService.Object, mapper);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var division = new LkPlantDivision
            {
                IdDivision = 101,
                Division = "Division A",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(division);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<PlantDivisionGetDto>(okResult.Value);

            Assert.Equal("Division A", dto.Division);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((LkPlantDivision)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantDivision>
            {
                new LkPlantDivision { IdDivision = 101, Division = "Division A" },
                new LkPlantDivision { IdDivision = 102, Division = "Division B" }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<PlantDivisionGetDto>>(okResult.Value).ToList();

            Assert.Equal(2, dtos.Count);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new PlantDivisionPostDto
            {
                Division = "Division C"
            };

            _mockService.Setup(s => s.AddAsync(It.IsAny<LkPlantDivision>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdDivision = Assert.IsType<PlantDivisionGetDto>(createdResult.Value);

            Assert.Equal("Division C", createdDivision.Division);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            var division = new LkPlantDivision { IdDivision = 101, Division = "Division A" };

            _mockService.Setup(s => s.DeleteAsync(101)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(101);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dict = okResult.Value.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("División eliminada correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.DeleteAsync(99)).ThrowsAsync(new Exception("No existe división con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var dict = notFoundResult.Value.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(notFoundResult.Value));

            Assert.Equal("No existe división con id=99", dict["message"]);
        }
    }
}
