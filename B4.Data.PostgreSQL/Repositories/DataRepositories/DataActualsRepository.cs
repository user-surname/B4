using B4.Models.Entities.DataEntities;
using B4.Models.Interfaces.DataInterfaces;
using Dapper;

namespace B4.Data.PostgreSQL.Repositories.DataRepositories
{
    public class DataActualsRepository
        : BaseFinancialRepository<DataActuals>, IDataActualsRepository
    {
        public DataActualsRepository(PostgreSQLDapperContext context)
            : base(context, "b4.data_actuals") { }

        // Funciones implementadas para el controller (DataActuals)
        // NOTA: Estas funciones probablemente se reutilizarán en otros
        // controllers (Budget/Forecast/Bridges...)

        public async Task<IEnumerable<DataActuals>> GetByPlantaEjercicioAsync(int planta, int ejercicio)
        {
            var sql = $@"
                SELECT *
                FROM {_table}
                WHERE idcompany = @planta
                  AND ejercicio = @ejercicio
                ORDER BY idepigrafe;";

            using var conn = GetConnection();
            return await conn.QueryAsync<DataActuals>(sql, new { planta, ejercicio });
        }

        public async Task<DataActuals?> GetByPlantaEjercicioEpigrafeAsync(int planta, int ejercicio, int epigrafe)
        {
            var sql = $@"
                SELECT *
                FROM {_table}
                WHERE idcompany = @planta
                  AND ejercicio = @ejercicio
                  AND idepigrafe = @epigrafe
                LIMIT 1;";

            using var conn = GetConnection();
            return await conn.QuerySingleOrDefaultAsync<DataActuals>(sql, new { planta, ejercicio, epigrafe });
        }
    }
}
