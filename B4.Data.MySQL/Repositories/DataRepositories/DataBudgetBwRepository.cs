using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System;
using B4.Models.Entities.DataEntities;
using B4.Models.Interfaces.DataInterfaces;

namespace B4.Data.MySQL.Repositories.DataRepositories
{
    public class DataBudgetBwRepository
        : DataFinancialRepository<DataBudgetBw>, IDataBudgetBwRepository
    {
        public DataBudgetBwRepository(MySQLDapperContext context)
            : base(context, "DATA_Budget_BW")
        {



        }
    }
}

