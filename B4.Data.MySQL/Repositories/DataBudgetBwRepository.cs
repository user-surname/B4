using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;
using B4.Data.MySQL;
using System;

namespace B4.Data.MySQL.Repositories
{
    public class DataBudgetBwRepository
        : DataBwBaseRepository<DataBudgetBw>, IDataBudgetBwRepository
    {
        public DataBudgetBwRepository(MySQLDapperContext context)
            : base(context, "DATA_Budget_BW")
        {



        }
    }
}

