using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.RepositoryInterfaces;
using System;
using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;

namespace B4.Data.MySQL.Repositories.DataRepositories
{
    public class DataBudgetRepository
        : DataFinancialRepository<DataBudget>, IDataBudgetRepository
    {
        public DataBudgetRepository(MySQLDapperContext context)
            : base(context, "DATA_Budget")
        {



        }
    }
}