using Dapper;
using Npgsql;
using B4.Models.Entities;
using B4.Models.Interfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class DataTipoCambioRepository : IDataTipoCambioRepository
    {
        private readonly string _connectionString;
        private const string _table = "b4.data_tipo_cambio";

        public DataTipoCambioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddAsync(DataTipoCambio entity)
        {
            var sql = $@"
                INSERT INTO {_table}
                (idapicarga, guidcarga, fechaultmodif,
                 ejercicio, idcurrency, calendarday, mes,
                 p, fc, fb, idcarga, idcargastgbw, idhoja)
                VALUES
                (@IdAPICarga, @GuidCarga, @FechaUltModif,
                 @Ejercicio, @IdCurrency, @CalendarDay, @Mes,
                 @P, @FC, @FB, @IdCarga, @IdCargaSTGBW, @IdHoja)
                RETURNING id;";

            using var conn = new NpgsqlConnection(_connectionString);
            entity.Id = await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<DataTipoCambio?> GetByIdAsync(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QuerySingleOrDefaultAsync<DataTipoCambio>(
                $"SELECT * FROM {_table} WHERE id=@Id", new { Id = id });
        }

        public async Task<IEnumerable<DataTipoCambio>> GetAllAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<DataTipoCambio>($"SELECT * FROM {_table}");
        }

        public async Task UpdateAsync(DataTipoCambio entity)
        {
            var sql = $@"
                UPDATE {_table} SET
                    idapicarga=@IdAPICarga,
                    guidcarga=@GuidCarga,
                    fechaultmodif=@FechaUltModif,
                    ejercicio=@Ejercicio,
                    idcurrency=@IdCurrency,
                    calendarday=@CalendarDay,
                    mes=@Mes,
                    p=@P, fc=@FC, fb=@FB,
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
