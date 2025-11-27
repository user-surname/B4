using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;
using B4.Data.MySQL;

namespace B4.Data.MySQL.Repositories
{
    public class DataBudgetRepository : IDataBudgetRepository
    {
        private const string _tableName = "DATA_Budget";
        private readonly MySQLDapperContext _context;

        public DataBudgetRepository(MySQLDapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(DataBudget entity)
        {
            var sql = $@"
                INSERT INTO {_tableName}
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (@idAPICarga, @guidCarga, @FechaUltModif, @idCompany, @ejercicio, @idCiclo, @idFase, @idCurrency, @idEpigrafe,
                 @mes00, @mes01, @mes02, @mes03, @mes04, @mes05, @mes06, @mes07, @mes08, @mes09, @mes10, @mes11, @mes12, @mes13)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un registro de DATA_Budget en la base de datos.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<DataBudget?> GetByIdAsync(int id)
        {
            var sql = $@"SELECT * FROM {_tableName} WHERE id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<DataBudget>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el registro de DATA_Budget con id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<DataBudget>> GetAllAsync()
        {
            var sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<DataBudget>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de DATA_Budget.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(DataBudget entity)
        {
            var sql = $@"
                UPDATE {_tableName} SET
                    idAPICarga = @idAPICarga,
                    guidCarga = @guidCarga,
                    FechaUltModif = @FechaUltModif,
                    idCompany = @idCompany,
                    ejercicio = @ejercicio,
                    idCiclo = @idCiclo,
                    idFase = @idFase,
                    idCurrency = @idCurrency,
                    idEpigrafe = @idEpigrafe,
                    mes00 = @mes00,
                    mes01 = @mes01,
                    mes02 = @mes02,
                    mes03 = @mes03,
                    mes04 = @mes04,
                    mes05 = @mes05,
                    mes06 = @mes06,
                    mes07 = @mes07,
                    mes08 = @mes08,
                    mes09 = @mes09,
                    mes10 = @mes10,
                    mes11 = @mes11,
                    mes12 = @mes12,
                    mes13 = @mes13
                WHERE id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, entity);

                if (rows == 0)
                    throw new Exception($"No se encontró el registro de DATA_Budget con id {entity.Id} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el registro de DATA_Budget con id {entity.Id}.", ex);
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
                int rows = await conn.ExecuteAsync(sql, new { Id = id });

                if (rows == 0)
                    throw new Exception($"No se encontró el registro de DATA_Budget con id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el registro de DATA_Budget con id {id}.", ex);
            }
        }
    }
}

