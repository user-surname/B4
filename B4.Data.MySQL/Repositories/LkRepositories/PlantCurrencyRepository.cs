using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.MySQL.Repositories.LkRepositories
{
    public class PlantCurrencyRepository : IPlantCurrencyRepository
    {
        private const string _tableName = "LK_PLANT_CURRENCY";

        private readonly MySQLDapperContext _context;

        public PlantCurrencyRepository(MySQLDapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(LkPlantCurrency entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName} (idCurrency, Currency, CurrencyAlias)
                VALUES (@IdCurrency, @Currency, @CurrencyAlias)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar una Currency en la base de datos.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<LkPlantCurrency?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE idCurrency = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<LkPlantCurrency>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la Currency con id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<LkPlantCurrency>> GetAllAsync()
        {
            const string sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<LkPlantCurrency>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de Currencies.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(LkPlantCurrency entity)
        {
            const string sql = $@"
                UPDATE {_tableName} SET 
                    Currency = @Currency,
                    CurrencyAlias = @CurrencyAlias
                WHERE idCurrency = @IdCurrency";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, entity);

                if (rows == 0)
                    throw new Exception($"No se encontró la Currency con id {entity.IdCurrency} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar la Currency con id {entity.IdCurrency}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = $@"DELETE FROM {_tableName} WHERE idCurrency = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, new { Id = id });

                if (rows == 0)
                    throw new Exception($"No se encontró la Currency con id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar la Currency con id {id}.", ex);
            }
        }
    }
}
