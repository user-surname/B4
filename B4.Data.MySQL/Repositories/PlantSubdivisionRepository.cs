using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;
using B4.Data.MySQL;
using System;

namespace B4.Data.MySQL.Repositories
{
    public class PlantSubdivisionRepository : IPlantSubdivisionRepository
    {
        private const string _tableName = "LK_PLANT_SUBDIVISION";
        private readonly MySQLDapperContext _context;

        public PlantSubdivisionRepository(MySQLDapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(LkPlantSubdivision entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName} (idSubdivision, Subdivision)
                VALUES (@IdSubdivision, @Subdivision)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un Subdivision en la base de datos.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<LkPlantSubdivision?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE idSubdivision = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<LkPlantSubdivision>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la Subdivision con id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<LkPlantSubdivision>> GetAllAsync()
        {
            const string sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<LkPlantSubdivision>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de Subdivisions.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(LkPlantSubdivision entity)
        {
            const string sql = $@"
                UPDATE {_tableName} 
                SET Subdivision = @Subdivision
                WHERE idSubdivision = @IdSubdivision";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, entity);

                if (rows == 0)
                    throw new Exception($"No se encontró la Subdivision con id {entity.IdSubdivision} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar la Subdivision con id {entity.IdSubdivision}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = $@"DELETE FROM {_tableName} WHERE idSubdivision = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, new { Id = id });

                if (rows == 0)
                    throw new Exception($"No se encontró la Subdivision con id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar la Subdivision con id {id}.", ex);
            }
        }
    }
}

