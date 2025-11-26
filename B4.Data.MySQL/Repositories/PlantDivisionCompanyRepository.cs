using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;
using B4.Data.MySQL;

namespace B4.Data.MySQL.Repositories
{
    public class PlantDivisionCompanyRepository : IPlantDivisionCompanyRepository
    {
        private const string _tableName = "LK_PLANT_DIVISION_COMPANY";
        private readonly MySQLDapperContext _context;

        public PlantDivisionCompanyRepository(MySQLDapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(LkPlantDivisionCompany entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName} (idDivisionCompany, DivisionCompany)
                VALUES (@IdDivisionCompany, @DivisionCompany)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar una DivisionCompany en la base de datos.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<LkPlantDivisionCompany?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE idDivisionCompany = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<LkPlantDivisionCompany>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la DivisionCompany con id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<LkPlantDivisionCompany>> GetAllAsync()
        {
            const string sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<LkPlantDivisionCompany>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de DivisionCompany.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(LkPlantDivisionCompany entity)
        {
            const string sql = $@"
                UPDATE {_tableName} SET 
                    DivisionCompany = @DivisionCompany
                WHERE idDivisionCompany = @IdDivisionCompany";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, entity);

                if (rows == 0)
                    throw new Exception($"No se encontró la DivisionCompany con id {entity.IdDivisionCompany} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar la DivisionCompany con id {entity.IdDivisionCompany}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = $@"DELETE FROM {_tableName} WHERE idDivisionCompany = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, new { Id = id });

                if (rows == 0)
                    throw new Exception($"No se encontró la DivisionCompany con id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar la DivisionCompany con id {id}.", ex);
            }
        }
    }
}

