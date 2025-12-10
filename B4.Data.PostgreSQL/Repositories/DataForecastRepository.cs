using B4.Models.Entities;
using B4.Models.Interfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class DataForecastRepository
        : BaseFinancialRepository<DataForecast>, IDataForecastRepository
    {
        public DataForecastRepository(string cs)
            : base(cs, "b4.data_forecast") { }
    }
}
