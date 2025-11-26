using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;
using B4.Data.MySQL;

namespace B4.Data.MySQL.Repositories
{
    public class DataForecastRepository : IDataForecastRepository
    {
        private const string _tableName = "DATA_Forecast";
        private readonly DapperContext _context;

        public DataForecastRepository(DapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(DataForecast entity)
        {
            const string sql = @"
                INSERT INTO DATA_Forecast
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (@IdAPICarga, @GuidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdCurrency, @IdEpigrafe,
                 @Mes00, @Mes01, @Mes02, @Mes03, @Mes04, @Mes05, @Mes06, @Mes07, @Mes08, @Mes09, @Mes10, @Mes11, @Mes12, @Mes13)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un Forecast en la base de datos.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<DataForecast?> GetByIdAsync(int id)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<DataForecast>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el Forecast con id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<DataForecast>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<DataForecast>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de Forecasts.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(DataForecast entity)
        {
            var sql = @"
                UPDATE DATA_Forecast SET
                    idAPICarga = @IdAPICarga,
                    guidCarga = @GuidCarga,
                    FechaUltModif = @FechaUltModif,
                    idCompany = @IdCompany,
                    ejercicio = @Ejercicio,
                    idCiclo = @IdCiclo,
                    idFase = @IdFase,
                    idCurrency = @IdCurrency,
                    idEpigrafe = @IdEpigrafe,
                    mes00 = @Mes00,
                    mes01 = @Mes01,
                    mes02 = @Mes02,
                    mes03 = @Mes03,
                    mes04 = @Mes04,
                    mes05 = @Mes05,
                    mes06 = @Mes06,
                    mes07 = @Mes07,
                    mes08 = @Mes08,
                    mes09 = @Mes09,
                    mes10 = @Mes10,
                    mes11 = @Mes11,
                    mes12 = @Mes12,
                    mes13 = @Mes13
                WHERE id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, entity);
                if (rows == 0)
                    throw new Exception($"No se encontró el Forecast con id {entity.Id} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el Forecast con id {entity.Id}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            var sql = $"DELETE FROM {_tableName} WHERE id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, new { Id = id });
                if (rows == 0)
                    throw new Exception($"No se encontró el Forecast con id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el Forecast con id {id}.", ex);
            }
        }
    }
}

