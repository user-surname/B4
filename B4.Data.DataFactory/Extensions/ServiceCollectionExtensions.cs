using System;
using B4.Data.DataFactory.Configuration;
using B4.Data.DataFactory.Connections;
using B4.Data.DataFactory.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace B4.Data.DataFactory.Extensions
{
    /// <summary>
    /// Registra en DI los servicios base de DataFactory.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Añade las opciones, la factoria de conexiones y el proveedor de queries.
        /// Llamar a este metodo desde AddDataFactoryModule en B4.Api.
        /// </summary>
        public static IServiceCollection AddDataFactory(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);

            services.Configure<DataFactoryOptions>(options =>
            {
                options.Provider = configuration[DataFactoryOptions.ProviderConfigurationKey]?.Trim() ?? string.Empty;
                options.MySqlConnectionString =
                    configuration.GetConnectionString(DataFactoryOptions.MySqlConnectionStringName) ?? string.Empty;
                options.PostgreSqlConnectionString =
                    configuration.GetConnectionString(DataFactoryOptions.PostgreSqlConnectionStringName) ?? string.Empty;
                options.SqlServerConnectionString =
                    configuration.GetConnectionString(DataFactoryOptions.SqlServerConnectionStringName) ?? string.Empty;
            });

            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.AddScoped<IDataQueryProvider, DataQueryProvider>();

            return services;
        }
    }
}
