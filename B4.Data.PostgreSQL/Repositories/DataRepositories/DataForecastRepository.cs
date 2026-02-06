using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;

namespace B4.Data.PostgreSQL.Repositories.DataRepositories
{
    public class DataForecastRepository
        : BaseFinancialRepository<DataForecast>, IDataForecastRepository
    {
        public DataForecastRepository(PostgreSQLDapperContext context)
            : base(context, "b4.data_forecast") { }
    }
}
