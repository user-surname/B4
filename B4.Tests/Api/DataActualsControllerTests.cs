using AutoMapper;
using AutoMapper.Internal;
using B4.Api.Controllers;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Api.Middleware;
using B4.Models.Common;
using B4.Models.Entities.DataEntities;
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

namespace B4.Tests.Api.Controllers
{
    public class DataActualsControllerTest
    {
        private readonly Mock<IDataActualsService> _mockService;
        private readonly DataActualsController _controller;
        private readonly ILogger<DataActualsController> _logger;

        public DataActualsControllerTest()
        {
            _mockService = new Mock<IDataActualsService>();
            _logger = NullLogger<DataActualsController>.Instance;

            var expression = new MapperConfigurationExpression();
            expression.AddProfile<MappingProfile>();

            var config = new MapperConfiguration(
                expression,
                NullLoggerFactory.Instance
            );

            var mapper = config.CreateMapper();

            _controller = new DataActualsController(_mockService.Object, mapper, _logger);
        }

        [Fact]
        public async Task GetActualsByPlantaEjercicioEpigrafe_ShouldReturnOk_WhenRecordExists()
        {
            var record = new DataActuals
            {
                IdCompany = 10,
                Ejercicio = 2024,
                IdCiclo = 1,
                IdFase = 2,
                IdCurrency = 2,
                IdEpigrafe = 300,
                Mes00 = 0,
                Mes01 = 1,
                Mes02 = 2,
                Mes03 = 3,
                Mes04 = 4,
                Mes05 = 5,
                Mes06 = 6,
                Mes07 = 7,
                Mes08 = 8,
                Mes09 = 9,
                Mes10 = 10,
                Mes11 = 11,
                Mes12 = 12,
                Mes13 = 13
            };

            _mockService
                .Setup(s => s.GetByPlantaEjercicioEpigrafeAsync(10, 2024, 300))
                .ReturnsAsync(record);

            var result = await _controller.GetActualsByPlantaEjercicioEpigrafe(10, 2024, 300);

            var ok = Assert.IsType<OkObjectResult>(result);

            var response = Assert.IsType<ApiResponse<DataActualsGetDto>>(ok.Value);

            var dto = response.Data;

            Assert.Equal("10", dto.Head.Planta);
            Assert.Equal(2024, dto.Head.Ejercicio);
            Assert.Single(dto.Detail);
            Assert.Equal(300, dto.Detail[0].IdEpigrafe);
            Assert.Equal(14, dto.Detail[0].Valores.Length);
            Assert.Equal(13m, dto.Detail[0].Valores.Last());
        }

        [Fact]
        public async Task GetActualsByPlantaEjercicioEpigrafe_ShouldReturnNotFound_WhenRecordMissing()
        {
            _mockService
                .Setup(s => s.GetByPlantaEjercicioEpigrafeAsync(10, 2024, 300))
                .ReturnsAsync((DataActuals)null);

            var result = await _controller.GetActualsByPlantaEjercicioEpigrafe(10, 2024, 300);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);

            var response = Assert.IsType<ApiResponse<DataActualsGetDto>>(notFound.Value);

            Assert.Contains("No existen actuals", response.Msg);
        }

        [Fact]
        public async Task PostActualsByPlantaEjercicio_ShouldReturnBadRequest_WhenBodyEmpty()
        {
            var result = await _controller.PostActualsByPlantaEjercicio(
                10, 2024, 1, "EUR",
                Array.Empty<DataActualsPostDto>());

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);

            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);

            Assert.Contains("vacío", response.Msg);
        }

        [Fact]
        public async Task PostActualsByPlantaEjercicio_ShouldReturnOk_WithInsertedCount()
        {
            _mockService
                .Setup(s => s.AddRangeAsync(10, 2024, 1, "EUR", It.IsAny<IEnumerable<DataActuals>>()))
                .ReturnsAsync(2);

            var body = new[]
            {
        new DataActualsPostDto { IdEpigrafe = 100, Mes01 = 1, Mes02 = 2, Mes03 = 3, Mes04 = 4, Mes05 = 5, Mes06 = 6, Mes07 = 7, Mes08 = 8, Mes09 = 9, Mes10 = 10, Mes11 = 11, Mes12 = 12, Mes13 = 13 },
        new DataActualsPostDto { IdEpigrafe = 200, Mes01 = 1, Mes02 = 2, Mes03 = 3, Mes04 = 4, Mes05 = 5, Mes06 = 6, Mes07 = 7, Mes08 = 8, Mes09 = 9, Mes10 = 10, Mes11 = 11, Mes12 = 12, Mes13 = 13 }
    };

            var result = await _controller.PostActualsByPlantaEjercicio(10, 2024, 1, "EUR", body);

            var ok = Assert.IsType<OkObjectResult>(result);

            var response = Assert.IsType<ApiResponse<InsertResultDto>>(ok.Value);

            Assert.Equal(2, response.Data.Inserted);

            _mockService.Verify(
                s => s.AddRangeAsync(10, 2024, 1, "EUR", It.IsAny<IEnumerable<DataActuals>>()),
                Times.Once
            );
        }
    }
}