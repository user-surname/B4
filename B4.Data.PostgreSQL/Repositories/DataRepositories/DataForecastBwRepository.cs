using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;

namespace B4.Data.PostgreSQL.Repositories.DataRepositories
{
    public class DataForecastBwRepository
        : BaseFinancialRepository<DataForecastBw>, IDataForecastBwRepository
    {
        public DataForecastBwRepository(PostgreSQLDapperContext context)
            : base(context, "b4.data_forecast_bw") { }
    }
}
