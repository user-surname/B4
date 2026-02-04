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
    public class PlantTreeControllerTest
    {
        private readonly Mock<IPlantTreeRepository> _mockRepo;
        private readonly PlantTreeController _controller;

        public PlantTreeControllerTest()
        {
            _mockRepo = new Mock<IPlantTreeRepository>();
            _controller = new PlantTreeController(_mockRepo.Object);
        }

        // -----------------------------------------
        // TEST GET BY ID
        // -----------------------------------------
        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var tree = new LkPlantTree
            {
                IdTree = 1,
                IdDivision = 10,
                IdDivisionCompany = 20,
                IdSubdivision = 30,
                IdCountry = 40,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tree);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<PlantTreeGetDto>(okResult.Value);

            Assert.Equal(10, dto.IdDivision);
            Assert.Equal(40, dto.IdCountry);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkPlantTree)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // -----------------------------------------
        // TEST GET ALL
        // -----------------------------------------
        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantTree>
            {
                new LkPlantTree { IdTree = 1, IdDivision = 10, IdDivisionCompany = 20, IdSubdivision = 30, IdCountry = 40 },
                new LkPlantTree { IdTree = 2, IdDivision = 11, IdDivisionCompany = 21, IdSubdivision = 31, IdCountry = 41 }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsType<List<PlantTreeGetDto>>(okResult.Value);

            Assert.Equal(2, dtos.Count);
        }

        // -----------------------------------------
        // TEST POST CREATE
        // -----------------------------------------
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new PlantTreePostDto
            {
                IdTree = 3,
                IdDivision = 12,
                IdDivisionCompany = 22,
                IdSubdivision = 32,
                IdCountry = 42
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<LkPlantTree>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdTree = Assert.IsType<LkPlantTree>(createdResult.Value);

            Assert.Equal(12, createdTree.IdDivision);
            Assert.Equal(42, createdTree.IdCountry);
        }

        // -----------------------------------------
        // TEST DELETE
        // -----------------------------------------
        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            var tree = new LkPlantTree { IdTree = 1, IdDivision = 10, IdCountry = 40 };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tree);
            _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            // Convertimos a diccionario para verificar mensaje
            var dict = okResult.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("Árbol eliminado correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkPlantTree)null);

            var result = await _controller.Delete(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
