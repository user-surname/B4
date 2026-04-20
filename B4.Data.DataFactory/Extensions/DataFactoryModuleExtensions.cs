using B4.Data.DataFactory.Repositories;
using B4.Models.RepositoryInterfaces;
using B4.Models.RepositoryInterfaces.DataInterfaces;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace B4.Data.DataFactory.Extensions
{
    /// <summary>
    /// Extension de composicion para B4.Api.
    /// Registra la infraestructura DataFactory y todos los repositorios migrados.
    /// Llamar desde Program.cs con: builder.Services.AddDataFactoryModule(builder.Configuration);
    /// </summary>
    public static class DataFactoryModuleExtensions
    {
        public static IServiceCollection AddDataFactoryModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // 1. Infraestructura base (opciones, conexion, query provider)
            services.AddDataFactory(configuration);

            // 2. Control
            services.AddScoped<IControlRepository, ControlRepository>();
            services.AddScoped<IControlPlantaRepository, ControlPlantaRepository>();

            // 3. LK (tablas maestras)
            services.AddScoped<ICiclosRepository, LkCiclosRepository>();
            services.AddScoped<IFasesRepository, LkFasesRepository>();
            services.AddScoped<IEpigrafeRepository, LkEpigrafeRepository>();
            services.AddScoped<IPlantCountryRepository, LkPlantCountryRepository>();
            services.AddScoped<IPlantCurrencyRepository, LkPlantCurrencyRepository>();
            services.AddScoped<IPlantDivisionRepository, LkPlantDivisionRepository>();
            services.AddScoped<IPlantDivisionCompanyRepository, LkPlantDivisionCompanyRepository>();
            services.AddScoped<IPlantSubdivisionRepository, LkPlantSubdivisionRepository>();
            services.AddScoped<IPlantTreeRepository, LkPlantTreeRepository>();
            services.AddScoped<IPlantCompanyRepository, LkPlantCompanyRepository>();
            services.AddScoped<IPlantControllersRepository, LkPlantControllersRepository>();
            services.AddScoped<IPlantillasBotonesPasosTiposRepository, LkPlantillasBotonesPasosTiposRepository>();

            // 4. DATA (financiero)
            services.AddScoped<IDataActualsRepository, DataActualsRepository>();
            services.AddScoped<IDataActualsBwRepository, DataActualsBwRepository>();
            services.AddScoped<IDataBudgetRepository, DataBudgetRepository>();
            services.AddScoped<IDataBudgetBwRepository, DataBudgetBwRepository>();
            services.AddScoped<IDataForecastRepository, DataForecastRepository>();
            services.AddScoped<IDataForecastBwRepository, DataForecastBwRepository>();
            services.AddScoped<IDataComentariosRepository, DataComentariosRepository>();
            services.AddScoped<ILogActividadRepository, LogActividadRepository>();
            services.AddScoped<IDataTipoCambioRepository, DataTipoCambioRepository>();
            services.AddScoped<IDataBridgesFyRepository, DataBridgesFyRepository>();
            services.AddScoped<IDataBridgesFyBwRepository, DataBridgesFyBwRepository>();
            services.AddScoped<IDataBridgesFyBwEurRepository, DataBridgesFyBwEurRepository>();
            services.AddScoped<IDataBridgesMonthRepository, DataBridgesMonthRepository>();
            services.AddScoped<IDataBridgesMonthBwRepository, DataBridgesMonthBwRepository>();

            return services;
        }
    }
}
