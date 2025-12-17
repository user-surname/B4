using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Interfaces;
using System;
using B4.Models.Entities.DataEntities;
using B4.Models.Interfaces.DataInterfaces;

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