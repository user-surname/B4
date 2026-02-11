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
    public class PlantTreeControllerTest
    {
        private readonly Mock<IPlantTreeService> _mockService;
        private readonly PlantTreeController _controller;

        public PlantTreeControllerTest()
        {
            _mockService = new Mock<IPlantTreeService>();

            var expression = new MapperConfigurationExpression();
            expression.AddProfile<MappingProfile>();

            var config = new MapperConfiguration(
                expression,
                NullLoggerFactory.Instance
            );

            var mapper = config.CreateMapper();

            _controller = new PlantTreeController(_mockService.Object, mapper);
        }

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

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(tree);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<PlantTreeGetDto>(okResult.Value);

            Assert.Equal(10, dto.IdDivision);
            Assert.Equal(40, dto.IdCountry);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((LkPlantTree)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkPlantTree>
            {
                new LkPlantTree { IdTree = 1, IdDivision = 10, IdDivisionCompany = 20, IdSubdivision = 30, IdCountry = 40 },
                new LkPlantTree { IdTree = 2, IdDivision = 11, IdDivisionCompany = 21, IdSubdivision = 31, IdCountry = 41 }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsType<List<PlantTreeGetDto>>(okResult.Value);

            Assert.Equal(2, dtos.Count);
        }

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

            _mockService.Setup(s => s.AddAsync(It.IsAny<LkPlantTree>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdTree = Assert.IsType<PlantTreeGetDto>(createdResult.Value);

            Assert.Equal(12, createdTree.IdDivision);
            Assert.Equal(42, createdTree.IdCountry);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dict = okResult.Value.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("Árbol eliminado correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.DeleteAsync(99))
                        .ThrowsAsync(new Exception("No existe árbol con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var dict = notFoundResult.Value.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(notFoundResult.Value));

            Assert.Equal("No existe árbol con id=99", dict["message"]);
        }
    }
}
