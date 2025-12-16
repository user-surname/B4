using B4.Models.Entities.DataEntities;
using B4.Models.Interfaces.DataInterfaces;

namespace B4.Data.PostgreSQL.Repositories.DataRepositories
{
    public class DataActualsBwRepository 
        : BaseFinancialRepository<DataActualsBw>, IDataActualsBwRepository
    {
        public DataActualsBwRepository(PostgreSQLDapperContext context)
            : base(context, "b4.data_actuals_bw") 
        {
        }
    }
}
