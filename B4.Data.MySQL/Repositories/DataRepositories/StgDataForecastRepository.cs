using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Interfaces;
using System;
using B4.Models.Entities.DataEntities;

namespace B4.Data.MySQL.Repositories.DataRepositories
{
    public class StgDataForecastRepository
        : DataFinancialRepository<StgDataForecast>
    {
        public StgDataForecastRepository(MySQLDapperContext context)
            : base(context, "STG_DATA_Forecast")
        {



        }

    }
}