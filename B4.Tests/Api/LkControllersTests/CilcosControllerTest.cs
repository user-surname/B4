using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class CilcosControllerTest
    {
        private readonly Mock<ICiclosRepository> _mockRepo;
        private readonly CilcosController _controller;

        public CilcosControllerTest()
        {
            _mockRepo = new Mock<ICiclosRepository>();
            _controller = new CilcosController(_mockRepo.Object);
        }

        // -----------------------------------------
        // TEST GET BY ID
        // -----------------------------------------
        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            var ciclo = new LkCiclos
            {
                Id = 1,
                IdCiclo = 2024,
                Ciclo = "Ciclo 2024",
                Descripcion = "Desc 2024",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(ciclo);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<CiclosGetDto>(okResult.Value);

            Assert.Equal("Ciclo 2024", dto.Ciclo);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkCiclos)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // -----------------------------------------
        // TEST GET ALL
        // -----------------------------------------
        [Fact]
        public async Task GetAll_ShouldReturnOk_WithAllRecords()
        {
            var list = new List<LkCiclos>
            {
                new LkCiclos { Id = 1, IdCiclo = 2024, Ciclo = "Ciclo 2024", Descripcion = "Desc 1" },
                new LkCiclos { Id = 2, IdCiclo = 2025, Ciclo = "Ciclo 2025", Descripcion = "Desc 2" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsType<List<CiclosGetDto>>(okResult.Value);

            Assert.Equal(2, dtos.Count);
        }

        // -----------------------------------------
        // TEST POST CREATE
        // -----------------------------------------
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new CiclosPostDto
            {
                IdCiclo = 2026,
                Ciclo = "Ciclo 2026",
                Descripcion = "Desc 2026"
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<LkCiclos>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdCiclo = Assert.IsType<LkCiclos>(createdResult.Value);

            Assert.Equal("Ciclo 2026", createdCiclo.Ciclo);
        }

        // -----------------------------------------
        // TEST DELETE
        // -----------------------------------------
        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRecordExists()
        {
            var ciclo = new LkCiclos { Id = 1, Ciclo = "Ciclo 1", IdCiclo = 2024 };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(ciclo);
            _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            // Convertimos a diccionario
            var dict = okResult.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.Equal("Ciclo eliminado correctamente", dict["message"]);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((LkCiclos)null);

            var result = await _controller.Delete(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}

