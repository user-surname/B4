using System;
using B4.Data.DataFactory.Configuration;
using B4.Data.DataFactory.Queries.Common;
using B4.Data.DataFactory.Queries.MySQL;
using B4.Data.DataFactory.Queries.PostgreSQL;
using Microsoft.Extensions.Options;

namespace B4.Data.DataFactory.Providers
{
    /// <summary>
    /// Resolves provider-specific query collections for DataFactory repositories.
    /// </summary>
    public sealed class DataQueryProvider : IDataQueryProvider
    {
        private readonly DataFactoryOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQueryProvider"/> class.
        /// </summary>
        /// <param name="options">DataFactory options.</param>
        public DataQueryProvider(IOptions<DataFactoryOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Gets Usuario queries for the selected provider.
        /// </summary>
        public IUsuarioQueries UsuarioQueries => ResolveUsuarioQueries();

        /// <summary>
        /// Gets ControlPlanta queries for the selected provider.
        /// </summary>
        public IControlPlantaQueries ControlPlantaQueries => ResolveControlPlantaQueries();

        private IUsuarioQueries ResolveUsuarioQueries()
        {
            var provider = GetProvider();

            if (provider.Equals("MySQL", StringComparison.OrdinalIgnoreCase))
            {
                return new MySqlUsuarioQueries();
            }

            if (provider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) ||
                provider.Equals("Postgres", StringComparison.OrdinalIgnoreCase))
            {
                return new PostgreSqlUsuarioQueries();
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

        private IControlPlantaQueries ResolveControlPlantaQueries()
        {
            var provider = GetProvider();

            if (provider.Equals("MySQL", StringComparison.OrdinalIgnoreCase))
            {
                return new MySqlControlPlantaQueries();
            }

            if (provider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) ||
                provider.Equals("Postgres", StringComparison.OrdinalIgnoreCase))
            {
                return new PostgreSqlControlPlantaQueries();
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

        private string GetProvider()
        {
            var provider = (_options.Provider ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(provider))
            {
                throw new InvalidOperationException(
                    $"Falta la clave de configuracion '{DataFactoryOptions.ProviderConfigurationKey}'.");
            }

            return provider;
        }
    }
}
