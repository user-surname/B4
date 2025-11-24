using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;
using B4.Data.MySQL;
using System;

namespace B4.Data.MySQL.Repositories
{
    public class PlantControllersRepository : IPlantControllersRepository
    {
        // Nombre fijo de la tabla
        private const string _tableName = "LK_PLANT_CONTROLLERS";

        private readonly DapperContext _context;

        public PlantControllersRepository(DapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(LkPlantControllers entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName}
                (idCompanyController, IdCompany, Controller, Email)
                VALUES (@IdCompanyController, @IdCompany, @Controller, @Email)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un controlador de empresa en la base de datos.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<LkPlantControllers?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE idCompanyController = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<LkPlantControllers>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el controlador con id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<LkPlantControllers>> GetAllAsync()
        {
            const string sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<LkPlantControllers>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de controladores.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(LkPlantControllers entity)
        {
            const string sql = $@"
                UPDATE {_tableName} SET
                    IdCompany = @IdCompany,
                    Controller = @Controller,
                    Email = @Email
                WHERE idCompanyController = @IdCompanyController";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, entity);

                if (rows == 0)
                    throw new Exception($"No se encontró el controlador con id {entity.IdCompanyController} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el controlador con id {entity.IdCompanyController}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = $@"DELETE FROM {_tableName} WHERE idCompanyController = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, new { Id = id });

                if (rows == 0)
                    throw new Exception($"No se encontró el controlador con id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el controlador con id {id}.", ex);
            }
        }
    }
}
