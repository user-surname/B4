using B4.Models.Entities;
using B4.Models.Interfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class DataActualsBwRepository 
        : BaseFinancialRepository<DataActualsBw>, IDataActualsBwRepository
    {
        public DataActualsBwRepository(string cs)
            : base(cs, "b4.data_actuals_bw") 
        {
        }
    }
}
