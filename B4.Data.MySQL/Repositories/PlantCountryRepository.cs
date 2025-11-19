using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;
using B4.Data.MySQL;

namespace B4.Data.MySQL.Repositories
{
    public class PlantCountryRepository : IPlantCountryRepository
    {
        // Nombre fijo de la tabla
        private const string _tableName = "LK_PLANT_COUNTRY";

        private readonly DapperContext _context;

        public PlantCountryRepository(DapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(LkPlantCountry entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName} (idCountry, Country)
                VALUES (@IdCountry, @Country)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un Country en la base de datos.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<LkPlantCountry?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE idCountry = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<LkPlantCountry>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el Country con id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<LkPlantCountry>> GetAllAsync()
        {
            const string sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<LkPlantCountry>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de Countries.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(LkPlantCountry entity)
        {
            const string sql = $@"
                UPDATE {_tableName} SET 
                    Country = @Country
                WHERE idCountry = @IdCountry";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, entity);

                if (rows == 0)
                    throw new Exception($"No se encontró el Country con id {entity.IdCountry} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el Country con id {entity.IdCountry}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = $@"DELETE FROM {_tableName} WHERE idCountry = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, new { Id = id });

                if (rows == 0)
                    throw new Exception($"No se encontró el Country con id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el Country con id {id}.", ex);
            }
        }
    }
}
