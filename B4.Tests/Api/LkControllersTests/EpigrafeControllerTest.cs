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
    public class EpigrafeControllerTest
    {
        private readonly Mock<IEpigrafeService> _mockService;
        private readonly EpigrafeController _controller;

        public EpigrafeControllerTest()
        {
            _mockService = new Mock<IEpigrafeService>();
            _controller = new EpigrafeController(_mockService.Object);
        }

        // -----------------------------------------
        // TEST GET BY ID
        // -----------------------------------------
        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var epigrafe = new LkEpigrafe
            {
                IdEpigrafe = 1,
                IdPlantilla = 10,
                IdHoja = 5,
                PreEpigrafe = "Pre",
                Epigrafe = "Epigrafe 1",
                EpigrafeFull = "Pre Epigrafe 1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(epigrafe);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<EpigrafeGetDto>(okResult.Value);

            Assert.Equal("Epigrafe 1", dto.Epigrafe);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((LkEpigrafe)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // -----------------------------------------
        // TEST GET ALL
        // -----------------------------------------
        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkEpigrafe>
            {
                new LkEpigrafe { IdEpigrafe = 1, IdPlantilla = 10, IdHoja = 5, Epigrafe = "Epigrafe 1", EpigrafeFull = "Pre 1", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, IsActive = 1 },
                new LkEpigrafe { IdEpigrafe = 2, IdPlantilla = 11, IdHoja = 6, Epigrafe = "Epigrafe 2", EpigrafeFull = "Pre 2", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, IsActive = 1 }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsType<List<EpigrafeGetDto>>(okResult.Value);

            Assert.Equal(2, dtos.Count);
        }

        // -----------------------------------------
        // TEST POST CREATE
        // -----------------------------------------
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new EpigrafePostDto
            {
                IdPlantilla = 12,
                IdHoja = 7,
                PreEpigrafe = "Pre",
                Epigrafe = "Epigrafe 3",
                EpigrafeFull = "Pre Epigrafe 3"
            };

            _mockService.Setup(s => s.AddAsync(It.IsAny<LkEpigrafe>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdEpigrafe = Assert.IsType<LkEpigrafe>(createdResult.Value);

            Assert.Equal("Epigrafe 3", createdEpigrafe.Epigrafe);
        }

        // -----------------------------------------
        // TEST DELETE
        // -----------------------------------------
        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var dict = okResult.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("Epigrafe eliminado correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockService
                .Setup(s => s.DeleteAsync(99))
                .ThrowsAsync(new Exception("No existe ciclo con id=99"));

            var result = await _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var dict = notFoundResult.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(notFoundResult.Value));

            Assert.Equal("No existe ciclo con id=99", dict["message"]);
        }
    }
}