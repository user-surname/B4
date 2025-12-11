using B4.Models.Entities;
using B4.Models.Interfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class DataForecastBwRepository
        : BaseFinancialRepository<DataForecastBw>, IDataForecastBwRepository
    {
        public DataForecastBwRepository(string cs)
            : base(cs, "b4.data_forecast_bw") { }
    }
}
