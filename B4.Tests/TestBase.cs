using AutoMapper;
using B4.Api.Middleware;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace B4.Tests
{
    public abstract class TestBase
    {
        protected readonly IMapper Mapper;

        protected TestBase()
        {
            // Configuración de AutoMapper usando tu constructor
            var expression = new MapperConfigurationExpression();
            expression.AddProfile<MappingProfile>();

            var config = new MapperConfiguration(
                expression,
                NullLoggerFactory.Instance // evita logs en los tests
            );

            Mapper = config.CreateMapper();
        }

        // Método helper para crear mocks fácilmente
        protected Mock<T> CreateMock<T>() where T : class
        {
            return new Mock<T>();
        }
    }
}


