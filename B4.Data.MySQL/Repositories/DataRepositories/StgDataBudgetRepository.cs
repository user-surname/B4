using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Interfaces;
using System;
using B4.Models.Entities.DataEtities;

namespace B4.Data.MySQL.Repositories.DataRepositories
{
    public class StgDataBudgetRepository
        : DataFinancialRepository<StgDataBudget>
    {
        public StgDataBudgetRepository(MySQLDapperContext context)
            : base(context, "STG_DATA_Budget")
        {



        }

    }
}