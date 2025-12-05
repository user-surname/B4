using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Interfaces;
using System;
using B4.Models.Entities.DataEtities;

namespace B4.Data.MySQL.Repositories.DataRepositories
{
    public class DataActualsRepository
        : DataFinancialRepository<DataActuals>
    {
        public DataActualsRepository(MySQLDapperContext context)
            : base(context, "DATA_Actuals")
        {



        }
    }
}