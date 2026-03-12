using System;
using B4.Data.DataFactory.Configuration;
using B4.Data.DataFactory.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace B4.Data.DataFactory.Extensions
{
    /// <summary>
    /// Registers DataFactory services and options in dependency injection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds DataFactory options and connection factory services.
        /// </summary>
        /// <param name="services">Service collection to update.</param>
        /// <param name="configuration">Application configuration source.</param>
        /// <returns>The same service collection for chaining.</returns>
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

            return services;
        }
    }
}
