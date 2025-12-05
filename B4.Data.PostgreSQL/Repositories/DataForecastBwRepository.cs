using Dapper;
using Npgsql;
using B4.Models.Entities.DataEtities;
using B4.Models.Interfaces.DataInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class DataForecastBwRepository : IDataForecastBwRepository
    {
        private readonly string _connectionString;
        private const string _table = "b4.data_forecast_bw";

        public DataForecastBwRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddAsync(DataForecastBw entity)
        {
            var sql = $@"
                INSERT INTO {_table}
                (idapicarga, guidcarga, fechaultmodif,
                 idcompany, ejercicio, idciclo, idfase, idcurrency, idepigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06,
                 mes07, mes08, mes09, mes10, mes11, mes12, mes13,
                 idcarga, idcargastgbw, idhoja)
                VALUES
                (@IdAPICarga, @GuidCarga, @FechaUltModif,
                 @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdCurrency, @IdEpigrafe,
                 @Mes00, @Mes01, @Mes02, @Mes03, @Mes04, @Mes05, @Mes06,
                 @Mes07, @Mes08, @Mes09, @Mes10, @Mes11, @Mes12, @Mes13,
                 @IdCarga, @IdCargaSTGBW, @IdHoja)
                RETURNING id;";

            using var conn = new NpgsqlConnection(_connectionString);
            entity.Id = await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<DataForecastBw?> GetByIdAsync(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QuerySingleOrDefaultAsync<DataForecastBw>(
                $"SELECT * FROM {_table} WHERE id=@Id",
                new { Id = id });
        }

        public async Task<IEnumerable<DataForecastBw>> GetAllAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<DataForecastBw>($"SELECT * FROM {_table}");
        }

        public async Task UpdateAsync(DataForecastBw entity)
        {
            var sql = $@"
                UPDATE {_table} SET
                    idapicarga=@IdAPICarga,
                    guidcarga=@GuidCarga,
                    fechaultmodif=@FechaUltModif,
                    idcompany=@IdCompany,
                    ejercicio=@Ejercicio,
                    idciclo=@IdCiclo,
                    idfase=@IdFase,
                    idcurrency=@IdCurrency,
                    idepigrafe=@IdEpigrafe,
                    mes00=@Mes00, mes01=@Mes01, mes02=@Mes02, mes03=@Mes03,
                    mes04=@Mes04, mes05=@Mes05, mes06=@Mes06,
                    mes07=@Mes07, mes08=@Mes08, mes09=@Mes09,
                    mes10=@Mes10, mes11=@Mes11, mes12=@Mes12, mes13=@Mes13,
                    idcarga=@IdCarga, idcargastgbw=@IdCargaSTGBW, idhoja=@IdHoja
                WHERE id=@Id";

            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync($"DELETE FROM {_table} WHERE id=@Id", new { Id = id });
        }
    }
}
