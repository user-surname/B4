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
    public class PlantDivisionControllerTest
    {
        private readonly Mock<IPlantDivisionRepository> _mockRepo;
        private readonly PlantDivisionController _controller;

        public PlantDivisionControllerTest()
        {
            _mockRepo = new Mock<IPlantDivisionRepository>();
            _controller = new PlantDivisionController(_mockRepo.Object);
        }

        // -----------------------------------------
        // TEST GET BY ID
        // -----------------------------------------
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

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(division);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<PlantDivisionGetDto>(okResult.Value);

            Assert.Equal("Division A", dto.Division);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkPlantDivision)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // -----------------------------------------
        // TEST GET ALL
        // -----------------------------------------
        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantDivision>
            {
                new LkPlantDivision { IdDivision = 101, Division = "Division A" },
                new LkPlantDivision { IdDivision = 102, Division = "Division B" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsType<List<PlantDivisionGetDto>>(okResult.Value);

            Assert.Equal(2, dtos.Count);
        }

        // -----------------------------------------
        // TEST POST CREATE
        // -----------------------------------------
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new PlantDivisionPostDto
            {
                Division = "Division C"
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<LkPlantDivision>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdDivision = Assert.IsType<LkPlantDivision>(createdResult.Value);

            Assert.Equal("Division C", createdDivision.Division);
        }

        // -----------------------------------------
        // TEST DELETE
        // -----------------------------------------
        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            var division = new LkPlantDivision { Division = "Division A", IdDivision = 101 };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(division);
            _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var dict = okResult.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("División eliminada correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkPlantDivision)null);

            var result = await _controller.Delete(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
