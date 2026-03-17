using System;
using B4.Data.DataFactory.Extensions;
using B4.Models.RepositoryInterfaces;
using B4.Models.RepositoryInterfaces.DataInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace B4.Api.Extensions
{
    /// <summary>
    /// Agrupa el registro de DataFactory y de los repositorios de la API
    /// que ya usan este patron comun.
    /// </summary>
    public static class DataFactoryModuleExtensions
    {
        /// <summary>
        /// Registra la infraestructura base de DataFactory y los repositorios
        /// de la API que ya trabajan con consultas resueltas por proveedor.
        /// </summary>
        /// <param name="services">Coleccion de servicios de la aplicacion.</param>
        /// <param name="configuration">Configuracion usada para resolver el proveedor activo.</param>
        /// <returns>La misma coleccion de servicios para encadenar registros.</returns>
        public static IServiceCollection AddDataFactoryModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);

            services.AddDataFactory(configuration);

            services.AddScoped<IControlPlantaRepository, B4.Data.DataFactory.Repositories.ControlPlantaRepository>();
            services.AddScoped<IDataActualsRepository, B4.Data.DataFactory.Repositories.DataActualsRepository>();
            services.AddScoped<IDataActualsBwRepository, B4.Data.DataFactory.Repositories.DataActualsBwRepository>();

            return services;
        }
    }
}
