using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Providers
{
    /// <summary>
    /// Expone los grupos de queries SQL resueltos para el proveedor activo.
    /// Los repositorios migrados consumen este contrato sin conocer si trabajan
    /// contra MySQL o PostgreSQL.
    /// </summary>
    public interface IDataQueryProvider
    {
        // --- Usuarios / Auth ---
        IUsuarioQueries UsuarioQueries { get; }

        // --- Control ---
        IControlQueries ControlQueries { get; }
        IControlPlantaQueries ControlPlantaQueries { get; }

        // --- LK (tablas maestras) ---
        ILkCiclosQueries LkCiclosQueries { get; }
        ILkFasesQueries LkFasesQueries { get; }
        ILkEpigrafeQueries LkEpigrafeQueries { get; }
        ILkPlantCountryQueries LkPlantCountryQueries { get; }
        ILkPlantCurrencyQueries LkPlantCurrencyQueries { get; }
        ILkPlantDivisionQueries LkPlantDivisionQueries { get; }
        ILkPlantDivisionCompanyQueries LkPlantDivisionCompanyQueries { get; }
        ILkPlantSubdivisionQueries LkPlantSubdivisionQueries { get; }
        ILkPlantTreeQueries LkPlantTreeQueries { get; }
        ILkPlantCompanyQueries LkPlantCompanyQueries { get; }
        ILkPlantControllersQueries LkPlantControllersQueries { get; }
        ILkPlantillasBotonesPasosTiposQueries LkPlantillasBotonesPasosTiposQueries { get; }

        // --- DATA (financiero) ---
        IDataActualsQueries DataActualsQueries { get; }
        IDataActualsBwQueries DataActualsBwQueries { get; }
        IDataBudgetQueries DataBudgetQueries { get; }
        IDataBudgetBwQueries DataBudgetBwQueries { get; }
        IDataForecastQueries DataForecastQueries { get; }
        IDataForecastBwQueries DataForecastBwQueries { get; }
        IDataComentariosQueries DataComentariosQueries { get; }
        IDataTipoCambioQueries DataTipoCambioQueries { get; }
        IDataBridgesFyQueries DataBridgesFyQueries { get; }
        IDataBridgesFyBwQueries DataBridgesFyBwQueries { get; }
        IDataBridgesFyBwEurQueries DataBridgesFyBwEurQueries { get; }
        IDataBridgesMonthQueries DataBridgesMonthQueries { get; }
        IDataBridgesMonthBwQueries DataBridgesMonthBwQueries { get; }
    }
}
