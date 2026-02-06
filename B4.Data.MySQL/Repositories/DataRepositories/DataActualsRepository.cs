using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;

namespace B4.Data.MySQL.Repositories.DataRepositories
{
    public class DataActualsRepository
        : DataFinancialRepository<DataActuals>, IDataActualsRepository
    {
        public DataActualsRepository(MySQLDapperContext context)
            : base(context, "DATA_Actuals")
        {
        }

        // ------------------------------------------------------------
        // TODO: -Funciones implementadas para el controller (DataActuals)
        // NOTA: luego se moverán a la clase padre / base repository
        // ------------------------------------------------------------
        public async Task<IEnumerable<DataActuals>> GetByPlantaEjercicioAsync(int planta, int ejercicio)
        {
            var sql = $@"
                SELECT *
                FROM {_tableName}
                WHERE idCompany = @planta
                  AND ejercicio = @ejercicio
                ORDER BY idEpigrafe;";

            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<DataActuals>(sql, new { planta, ejercicio });
        }

        public async Task<DataActuals?> GetByPlantaEjercicioEpigrafeAsync(int planta, int ejercicio, int epigrafe)
        {
            var sql = $@"
                SELECT *
                FROM {_tableName}
                WHERE idCompany = @planta
                  AND ejercicio = @ejercicio
                  AND idEpigrafe = @epigrafe
                LIMIT 1;";

            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<DataActuals>(sql, new { planta, ejercicio, epigrafe });
        }
    }
}
