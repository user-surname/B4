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
    public class PlantSubdivisionControllerTest
    {
        private readonly Mock<IPlantSubdivisionRepository> _mockRepo;
        private readonly PlantSubdivisionController _controller;

        public PlantSubdivisionControllerTest()
        {
            _mockRepo = new Mock<IPlantSubdivisionRepository>();
            _controller = new PlantSubdivisionController(_mockRepo.Object);
        }

        // -----------------------------------------
        // TEST GET BY ID
        // -----------------------------------------
        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var subdivision = new LkPlantSubdivision
            {
                IdSubdivision = 100,
                Subdivision = "Subdivisión A",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(subdivision);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<PlantSubdivisionGetDto>(okResult.Value);

            Assert.Equal("Subdivisión A", dto.Subdivision);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkPlantSubdivision)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // -----------------------------------------
        // TEST GET ALL
        // -----------------------------------------
        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantSubdivision>
            {
                new LkPlantSubdivision { IdSubdivision = 100, Subdivision = "Subdivisión A" },
                new LkPlantSubdivision { IdSubdivision = 101, Subdivision = "Subdivisión B" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsType<List<PlantSubdivisionGetDto>>(okResult.Value);

            Assert.Equal(2, dtos.Count);
        }

        // -----------------------------------------
        // TEST POST CREATE
        // -----------------------------------------
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new PlantSubdivisionPostDto
            {
                IdSubdivision = 102,
                Subdivision = "Subdivisión C"
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<LkPlantSubdivision>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdSubdivision = Assert.IsType<LkPlantSubdivision>(createdResult.Value);

            Assert.Equal("Subdivisión C", createdSubdivision.Subdivision);
        }

        // -----------------------------------------
        // TEST DELETE
        // -----------------------------------------
        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            var subdivision = new LkPlantSubdivision { IdSubdivision = 100, Subdivision = "Subdivisión A" };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(subdivision);
            _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var dict = okResult.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("Subdivisión eliminada correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkPlantSubdivision)null);

            var result = await _controller.Delete(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
