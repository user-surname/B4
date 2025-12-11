using B4.Models.Entities;
using B4.Models.Interfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class DataActualsRepository
        : BaseFinancialRepository<DataActuals>, IDataActualsRepository
    {
        public DataActualsRepository(string cs)
            : base(cs, "b4.data_actuals") { }
    }
}
