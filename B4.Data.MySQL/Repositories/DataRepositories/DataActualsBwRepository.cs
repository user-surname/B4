using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;

namespace B4.Data.MySQL.Repositories.DataRepositories
{
    public class DataActualsBwRepository
        : DataFinancialRepository<DataActualsBw>, IDataActualsBwRepository
    {
        public DataActualsBwRepository(MySQLDapperContext context)
            : base(context, "DATA_Actuals_BW")
        {
        }

        // Si tu interfaz tiene métodos extra, implementa aquí.
        // Si solo hereda CRUD del base, esto compilará sin añadir nada.
    }
}
