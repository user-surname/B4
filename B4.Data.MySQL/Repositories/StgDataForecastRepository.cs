using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;
using B4.Data.MySQL;

namespace B4.Data.MySQL.Repositories
{
    public class StgDataForecastRepository : IStgDataForecastRepository
    {
        private const string _tableName = "STG_DATA_Forecast";
        private readonly DapperContext _context;

        public StgDataForecastRepository(DapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(StgDataForecast entity)
        {
            var sql = $@"
                INSERT INTO {_tableName}
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
                throw new Exception("Error al insertar un registro en STG_DATA_Forecast.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<StgDataForecast?> GetByIdAsync(int id)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<StgDataForecast>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el registro de STG_DATA_Forecast con id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<StgDataForecast>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<StgDataForecast>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de STG_DATA_Forecast.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(StgDataForecast entity)
        {
            var sql = $@"
                UPDATE {_tableName} SET
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
                    throw new Exception($"No se encontró el registro de STG_DATA_Forecast con id {entity.Id} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el registro de STG_DATA_Forecast con id {entity.Id}.", ex);
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
                    throw new Exception($"No se encontró el registro de STG_DATA_Forecast con id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el registro de STG_DATA_Forecast con id {id}.", ex);
            }
        }
    }
}

