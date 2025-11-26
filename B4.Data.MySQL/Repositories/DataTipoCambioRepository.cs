using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;
using B4.Data.MySQL;

namespace B4.Data.MySQL.Repositories
{
    public class DataTipoCambioRepository : IDataTipoCambioRepository
    {
        private const string _tableName = "DATA_Tipo_Cambio";
        private readonly DapperContext _context;

        public DataTipoCambioRepository(DapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(DataTipoCambio entity)
        {
            var sql = $@"
                INSERT INTO {_tableName} 
                (idAPICarga, guidCarga, FechaUltModif, ejercicio, idCurrency, CalendarDay, mes, P, FC, FB, idCarga, idCargaSTGBW, idHoja)
                VALUES
                (@idAPICarga, @guidCarga, @FechaUltModif, @Ejercicio, @IdCurrency, @CalendarDay, @Mes, @P, @FC, @FB, @IdCarga, @IdCargaSTGBW, @IdHoja)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un registro en DATA_Tipo_Cambio.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<DataTipoCambio?> GetByIdAsync(int id)
        {
            var sql = $@"SELECT * FROM {_tableName} WHERE id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<DataTipoCambio>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el registro con Id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<DataTipoCambio>> GetAllAsync()
        {
            var sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<DataTipoCambio>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener todos los registros de DATA_Tipo_Cambio.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(DataTipoCambio entity)
        {
            var sql = $@"
                UPDATE {_tableName} SET
                    idAPICarga = @idAPICarga,
                    guidCarga = @guidCarga,
                    FechaUltModif = @FechaUltModif,
                    ejercicio = @Ejercicio,
                    idCurrency = @IdCurrency,
                    CalendarDay = @CalendarDay,
                    mes = @Mes,
                    P = @P,
                    FC = @FC,
                    FB = @FB,
                    idCarga = @IdCarga,
                    idCargaSTGBW = @IdCargaSTGBW,
                    idHoja = @IdHoja
                WHERE id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                var rows = await conn.ExecuteAsync(sql, entity);
                if (rows == 0)
                    throw new Exception($"No se encontró el registro con Id {entity.Id} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el registro con Id {entity.Id}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            var sql = $@"DELETE FROM {_tableName} WHERE id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                var rows = await conn.ExecuteAsync(sql, new { Id = id });
                if (rows == 0)
                    throw new Exception($"No se encontró el registro con Id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el registro con Id {id}.", ex);
            }
        }
    }
}

