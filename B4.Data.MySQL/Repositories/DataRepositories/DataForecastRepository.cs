using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces;
using B4.Models.RepositoryInterfaces.DataInterfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace B4.Data.MySQL.Repositories.DataRepositories
{
    public class DataForecastRepository
        : DataFinancialRepository<DataForecast>, IDataForecastRepository
    {
        public DataForecastRepository(MySQLDapperContext context)
            : base(context, "DATA_Forecast")
        {



        }
    }
}
    
