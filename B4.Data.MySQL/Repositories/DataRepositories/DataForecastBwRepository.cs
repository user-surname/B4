using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System;
using B4.Models.Entities.DataEtities;
using B4.Models.Interfaces.DataInterfaces;

namespace B4.Data.MySQL.Repositories.DataRepositories
{
    public class DataForecastBwRepository
        : DataBwBaseRepository<DataForecastBw>, IDataForecastBwRepository
    {
        public DataForecastBwRepository(MySQLDapperContext context)
            : base(context, "DATA_Forecast_BW")
        {



        }
    }
}

