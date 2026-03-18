using System;
using B4.Data.DataFactory.Configuration;
using B4.Data.DataFactory.Queries.Common;
using B4.Data.DataFactory.Queries.MySQL;
using B4.Data.DataFactory.Queries.PostgreSQL;
using Microsoft.Extensions.Options;

namespace B4.Data.DataFactory.Providers
{
    /// <summary>
    /// Resuelve las colecciones de queries SQL segun el proveedor configurado
    /// en la clave "bbdd" y devuelve la version correcta para MySQL o PostgreSQL.
    /// </summary>
    public sealed class DataQueryProvider : IDataQueryProvider
    {
        private readonly DataFactoryOptions _options;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DataQueryProvider"/>.
        /// </summary>
        /// <param name="options">Opciones de configuración de DataFactory.</param>
        public DataQueryProvider(IOptions<DataFactoryOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Obtiene las queries de Usuario para el proveedor seleccionado.
        /// </summary>
        public IUsuarioQueries UsuarioQueries => ResolveUsuarioQueries();

        /// <summary>
        /// Obtiene las queries de ControlPlanta para el proveedor seleccionado.
        /// </summary>
        public IControlPlantaQueries ControlPlantaQueries => ResolveControlPlantaQueries();

        /// <summary>
        /// Obtiene las queries de DataActualsBw para el proveedor seleccionado.
        /// </summary>
        public IDataActualsBwQueries DataActualsBwQueries => ResolveDataActualsBwQueries();

        /// <summary>
        /// Obtiene las queries de DataActuals para el proveedor seleccionado.
        /// </summary>
        public IDataActualsQueries DataActualsQueries => ResolveDataActualsQueries();

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

        private IDataActualsBwQueries ResolveDataActualsBwQueries()
        {
            var provider = GetProvider();

            if (provider.Equals("MySQL", StringComparison.OrdinalIgnoreCase))
            {
                return new MySqlDataActualsBwQueries();
            }

            if (provider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) ||
                provider.Equals("Postgres", StringComparison.OrdinalIgnoreCase))
            {
                return new PostgreSqlDataActualsBwQueries();
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

        private IDataActualsQueries ResolveDataActualsQueries()
        {
            var provider = GetProvider();

            if (provider.Equals("MySQL", StringComparison.OrdinalIgnoreCase))
            {
                return new MySqlDataActualsQueries();
            }

            if (provider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) ||
                provider.Equals("Postgres", StringComparison.OrdinalIgnoreCase))
            {
                return new PostgreSqlDataActualsQueries();
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
