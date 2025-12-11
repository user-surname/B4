using B4.Models.Entities;
using B4.Models.Interfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class DataBudgetRepository
        : BaseFinancialRepository<DataBudget>, IDataBudgetRepository
    {
        public DataBudgetRepository(string cs)
            : base(cs, "b4.data_budget") { }
    }
}
