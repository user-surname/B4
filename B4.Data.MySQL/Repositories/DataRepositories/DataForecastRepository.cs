using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Interfaces;
using System;
using B4.Models.Entities.DataEntities;

namespace B4.Data.MySQL.Repositories.DataRepositories
{
    public class DataForecastRepository
        : DataFinancialRepository<DataForecast>
    {
        public DataForecastRepository(MySQLDapperContext context)
            : base(context, "DATA_Forecast")
        {



        }
    }
}
    
