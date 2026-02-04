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
    public class EpigrafeControllerTest
    {
        private readonly Mock<IEpigrafeRepository> _mockRepo;
        private readonly EpigrafeController _controller;

        public EpigrafeControllerTest()
        {
            _mockRepo = new Mock<IEpigrafeRepository>();
            _controller = new EpigrafeController(_mockRepo.Object);
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

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(epigrafe);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<EpigrafeGetDto>(okResult.Value);

            Assert.Equal("Epigrafe 1", dto.Epigrafe);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkEpigrafe)null);

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
                new LkEpigrafe { IdEpigrafe = 1, IdPlantilla = 10, IdHoja = 5, Epigrafe = "Epigrafe 1", EpigrafeFull = "Pre 1" },
                new LkEpigrafe { IdEpigrafe = 2, IdPlantilla = 11, IdHoja = 6, Epigrafe = "Epigrafe 2", EpigrafeFull = "Pre 2" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

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

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<LkEpigrafe>())).Returns(Task.CompletedTask);

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
            var epigrafe = new LkEpigrafe { IdEpigrafe = 1, Epigrafe = "Epigrafe 1", IdPlantilla = 10 };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(epigrafe);
            _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            // Convertimos a diccionario para leer propiedad anónima
            var dict = okResult.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("Epígrafe eliminado correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkEpigrafe)null);

            var result = await _controller.Delete(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
