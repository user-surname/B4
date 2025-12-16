using B4.Models.Entities.DataEntities;
using B4.Models.Interfaces.DataInterfaces;

namespace B4.Data.PostgreSQL.Repositories.DataRepositories
{
    public class DataBudgetRepository
        : BaseFinancialRepository<DataBudget>, IDataBudgetRepository
    {
        public DataBudgetRepository(PostgreSQLDapperContext context)
            : base(context, "b4.data_budget") { }
    }
}
