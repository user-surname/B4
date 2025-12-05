using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.MySQL.Repositories.LkRepositories
{
    public class PlantTreeRepository : IPlantTreeRepository
    {
        private const string _tableName = "LK_PLANT_TREE";
        private readonly MySQLDapperContext _context;

        public PlantTreeRepository(MySQLDapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(LkPlantTree entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName}
                (idTree, idDivision, idDivisionCompany, idSubdivision, idCountry)
                VALUES (@IdTree, @IdDivision, @IdDivisionCompany, @IdSubdivision, @IdCountry)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un registro en PlantTree.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<LkPlantTree?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE idTree = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<LkPlantTree>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el PlantTree con id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<LkPlantTree>> GetAllAsync()
        {
            const string sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<LkPlantTree>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de PlantTree.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(LkPlantTree entity)
        {
            const string sql = $@"
                UPDATE {_tableName} SET
                    idDivision = @IdDivision,
                    idDivisionCompany = @IdDivisionCompany,
                    idSubdivision = @IdSubdivision,
                    idCountry = @IdCountry
                WHERE idTree = @IdTree";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, entity);

                if (rows == 0)
                    throw new Exception($"No se encontró el PlantTree con id {entity.IdTree} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el PlantTree con id {entity.IdTree}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = $@"DELETE FROM {_tableName} WHERE idTree = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, new { Id = id });

                if (rows == 0)
                    throw new Exception($"No se encontró el PlantTree con id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el PlantTree con id {id}.", ex);
            }
        }
    }
}

