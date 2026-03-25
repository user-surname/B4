using System;
using B4.Data.DataFactory.Configuration;
using B4.Data.DataFactory.Queries.Common;
using B4.Data.DataFactory.Queries.MySQL;
using B4.Data.DataFactory.Queries.PostgreSQL;
using Microsoft.Extensions.Options;

namespace B4.Data.DataFactory.Providers
{
    /// <summary>
    /// Resuelve los bloques de queries SQL segun el proveedor configurado en "bbdd"
    /// y devuelve la implementacion MySQL o PostgreSQL correcta para cada repositorio.
    /// </summary>
    public sealed class DataQueryProvider : IDataQueryProvider
    {
        private readonly DataFactoryOptions _options;

        public DataQueryProvider(IOptions<DataFactoryOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        // --- Usuarios / Auth ---
        public IUsuarioQueries UsuarioQueries => Resolve<IUsuarioQueries>(
            () => new MySqlUsuarioQueries(),
            () => new PostgreSqlUsuarioQueries());

        // --- Control ---
        public IControlQueries ControlQueries => Resolve<IControlQueries>(
            () => new MySqlControlQueries(),
            () => new PostgreSqlControlQueries());

        public IControlPlantaQueries ControlPlantaQueries => Resolve<IControlPlantaQueries>(
            () => new MySqlControlPlantaQueries(),
            () => new PostgreSqlControlPlantaQueries());

        // --- LK ---
        public ILkCiclosQueries LkCiclosQueries => Resolve<ILkCiclosQueries>(
            () => new MySqlLkCiclosQueries(),
            () => new PostgreSqlLkCiclosQueries());

        public ILkFasesQueries LkFasesQueries => Resolve<ILkFasesQueries>(
            () => new MySqlLkFasesQueries(),
            () => new PostgreSqlLkFasesQueries());

        public ILkEpigrafeQueries LkEpigrafeQueries => Resolve<ILkEpigrafeQueries>(
            () => new MySqlLkEpigrafeQueries(),
            () => new PostgreSqlLkEpigrafeQueries());

        public ILkPlantCountryQueries LkPlantCountryQueries => Resolve<ILkPlantCountryQueries>(
            () => new MySqlLkPlantCountryQueries(),
            () => new PostgreSqlLkPlantCountryQueries());

        public ILkPlantCurrencyQueries LkPlantCurrencyQueries => Resolve<ILkPlantCurrencyQueries>(
            () => new MySqlLkPlantCurrencyQueries(),
            () => new PostgreSqlLkPlantCurrencyQueries());

        public ILkPlantDivisionQueries LkPlantDivisionQueries => Resolve<ILkPlantDivisionQueries>(
            () => new MySqlLkPlantDivisionQueries(),
            () => new PostgreSqlLkPlantDivisionQueries());

        public ILkPlantDivisionCompanyQueries LkPlantDivisionCompanyQueries => Resolve<ILkPlantDivisionCompanyQueries>(
            () => new MySqlLkPlantDivisionCompanyQueries(),
            () => new PostgreSqlLkPlantDivisionCompanyQueries());

        public ILkPlantSubdivisionQueries LkPlantSubdivisionQueries => Resolve<ILkPlantSubdivisionQueries>(
            () => new MySqlLkPlantSubdivisionQueries(),
            () => new PostgreSqlLkPlantSubdivisionQueries());

        public ILkPlantTreeQueries LkPlantTreeQueries => Resolve<ILkPlantTreeQueries>(
            () => new MySqlLkPlantTreeQueries(),
            () => new PostgreSqlLkPlantTreeQueries());

        public ILkPlantCompanyQueries LkPlantCompanyQueries => Resolve<ILkPlantCompanyQueries>(
            () => new MySqlLkPlantCompanyQueries(),
            () => new PostgreSqlLkPlantCompanyQueries());

        public ILkPlantControllersQueries LkPlantControllersQueries => Resolve<ILkPlantControllersQueries>(
            () => new MySqlLkPlantControllersQueries(),
            () => new PostgreSqlLkPlantControllersQueries());

        public ILkPlantillasBotonesPasosTiposQueries LkPlantillasBotonesPasosTiposQueries =>
            Resolve<ILkPlantillasBotonesPasosTiposQueries>(
                () => new MySqlLkPlantillasBotonesPasosTiposQueries(),
                () => new PostgreSqlLkPlantillasBotonesPasosTiposQueries());

        // --- DATA ---
        public IDataActualsQueries DataActualsQueries => Resolve<IDataActualsQueries>(
            () => new MySqlDataActualsQueries(),
            () => new PostgreSqlDataActualsQueries());

        public IDataActualsBwQueries DataActualsBwQueries => Resolve<IDataActualsBwQueries>(
            () => new MySqlDataActualsBwQueries(),
            () => new PostgreSqlDataActualsBwQueries());

        public IDataBudgetQueries DataBudgetQueries => Resolve<IDataBudgetQueries>(
            () => new MySqlDataBudgetQueries(),
            () => new PostgreSqlDataBudgetQueries());

        public IDataBudgetBwQueries DataBudgetBwQueries => Resolve<IDataBudgetBwQueries>(
            () => new MySqlDataBudgetBwQueries(),
            () => new PostgreSqlDataBudgetBwQueries());

        public IDataForecastQueries DataForecastQueries => Resolve<IDataForecastQueries>(
            () => new MySqlDataForecastQueries(),
            () => new PostgreSqlDataForecastQueries());

        public IDataForecastBwQueries DataForecastBwQueries => Resolve<IDataForecastBwQueries>(
            () => new MySqlDataForecastBwQueries(),
            () => new PostgreSqlDataForecastBwQueries());

        public IDataComentariosQueries DataComentariosQueries => Resolve<IDataComentariosQueries>(
            () => new MySqlDataComentariosQueries(),
            () => new PostgreSqlDataComentariosQueries());

        public IDataTipoCambioQueries DataTipoCambioQueries => Resolve<IDataTipoCambioQueries>(
            () => new MySqlDataTipoCambioQueries(),
            () => new PostgreSqlDataTipoCambioQueries());

        public IDataBridgesFyQueries DataBridgesFyQueries => Resolve<IDataBridgesFyQueries>(
            () => new MySqlDataBridgesFyQueries(),
            () => new PostgreSqlDataBridgesFyQueries());

        public IDataBridgesFyBwQueries DataBridgesFyBwQueries => Resolve<IDataBridgesFyBwQueries>(
            () => new MySqlDataBridgesFyBwQueries(),
            () => new PostgreSqlDataBridgesFyBwQueries());

        public IDataBridgesFyBwEurQueries DataBridgesFyBwEurQueries => Resolve<IDataBridgesFyBwEurQueries>(
            () => new MySqlDataBridgesFyBwEurQueries(),
            () => new PostgreSqlDataBridgesFyBwEurQueries());

        public IDataBridgesMonthQueries DataBridgesMonthQueries => Resolve<IDataBridgesMonthQueries>(
            () => new MySqlDataBridgesMonthQueries(),
            () => new PostgreSqlDataBridgesMonthQueries());

        public IDataBridgesMonthBwQueries DataBridgesMonthBwQueries => Resolve<IDataBridgesMonthBwQueries>(
            () => new MySqlDataBridgesMonthBwQueries(),
            () => new PostgreSqlDataBridgesMonthBwQueries());

        // ------------------------------------------------------------------
        // Helper generico: evita duplicar el if/else en cada propiedad
        // ------------------------------------------------------------------
        private T Resolve<T>(Func<T> mysqlFactory, Func<T> postgresFactory)
        {
            var provider = (_options.Provider ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(provider))
                throw new InvalidOperationException(
                    $"Falta la clave de configuracion '{DataFactoryOptions.ProviderConfigurationKey}'.");

            if (provider.Equals("MySQL", StringComparison.OrdinalIgnoreCase))
                return mysqlFactory();

            if (provider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) ||
                provider.Equals("Postgres", StringComparison.OrdinalIgnoreCase))
                return postgresFactory();

            if (provider.Equals("MSSQL", StringComparison.OrdinalIgnoreCase) ||
                provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
                throw new NotSupportedException(
                    $"El proveedor '{provider}' queda reservado para soporte futuro.");

            throw new InvalidOperationException(
                $"Proveedor no soportado '{provider}'. Proveedores soportados: MySQL, PostgreSQL.");
        }
    }
}
