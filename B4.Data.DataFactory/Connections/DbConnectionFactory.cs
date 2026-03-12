using System;
using System.Data;
using B4.Data.DataFactory.Configuration;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Npgsql;

namespace B4.Data.DataFactory.Connections
{
    /// <summary>
    /// Creates database connections based on configured provider.
    /// </summary>
    public sealed class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly DataFactoryOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionFactory"/> class.
        /// </summary>
        /// <param name="options">DataFactory options.</param>
        public DbConnectionFactory(IOptions<DataFactoryOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Creates a provider-specific database connection.
        /// </summary>
        /// <returns>A database connection instance.</returns>
        public IDbConnection CreateConnection()
        {
            var provider = (_options.Provider ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(provider))
            {
                throw new InvalidOperationException(
                    $"Falta la clave de configuración '{DataFactoryOptions.ProviderConfigurationKey}'.");
            }

            if (provider.Equals("MySQL", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(_options.MySqlConnectionString))
                {
                    throw new InvalidOperationException(
                        $"Falta la cadena de conexión '{DataFactoryOptions.MySqlConnectionStringName}'.");
                }

                return new MySqlConnection(_options.MySqlConnectionString);
            }

            if (provider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) ||
                provider.Equals("Postgres", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(_options.PostgreSqlConnectionString))
                {
                    throw new InvalidOperationException(
                        $"Falta la cadena de conexión '{DataFactoryOptions.PostgreSqlConnectionStringName}'.");
                }

                return new NpgsqlConnection(_options.PostgreSqlConnectionString);
            }

            if (provider.Equals("MSSQL", StringComparison.OrdinalIgnoreCase) ||
                provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException(
                    $"El proveedor '{provider}' queda reservado para soporte futuro y todavia no esta implementado.");
            }

            throw new InvalidOperationException(
                $"Proveedor no soportado '{provider}'. Proveedores soportados: MySQL, PostgreSQL.");
        }
    }
}
