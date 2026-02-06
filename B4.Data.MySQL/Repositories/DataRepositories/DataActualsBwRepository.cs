using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.RepositoryInterfaces;
using System;
using B4.Models.Entities.DataEntities;

namespace B4.Data.MySQL.Repositories.DataRepositories
{
    public class DataActualsBwRepository
        : DataFinancialRepository<DataActualsBw>
    {
        public DataActualsBwRepository(MySQLDapperContext context)
            : base(context, "DATA_Actuals_BW")
        {

            

        }
    }
}
