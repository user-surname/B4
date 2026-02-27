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
    // Clase base abstracta para los tests, proporciona utilidades comunes
    public abstract class TestBase
    {
        // Instancia de AutoMapper que se puede usar en todos los tests que hereden de esta clase
        protected readonly IMapper Mapper;

        protected TestBase()
        {
            // Configuración de AutoMapper usando el perfil definido en el proyecto
            var expression = new MapperConfigurationExpression();
            expression.AddProfile<MappingProfile>(); // Agrega el perfil de mapeo

            // Configuración del Mapper, evitando logs durante los tests
            var config = new MapperConfiguration(
                expression,
                NullLoggerFactory.Instance // Se utiliza para que no se generen logs en los tests
            );

            // Crear el objeto IMapper que se usará en los tests
            Mapper = config.CreateMapper();
        }

        // Método helper genérico para crear mocks fácilmente con Moq
        protected Mock<T> CreateMock<T>() where T : class
        {
            return new Mock<T>();
        }
    }
}


