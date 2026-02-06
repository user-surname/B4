using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System;
using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;

namespace B4.Data.MySQL.Repositories.DataRepositories
{
    public class DataForecastBwRepository
        : DataFinancialRepository<DataForecastBw>, IDataForecastBwRepository
    {
        public DataForecastBwRepository(MySQLDapperContext context)
            : base(context, "DATA_Forecast_BW")
        {



        }
    }
}

