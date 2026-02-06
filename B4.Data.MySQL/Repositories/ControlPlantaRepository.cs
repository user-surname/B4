using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.RepositoryInterfaces;
using B4.Data.MySQL;
using System;

namespace B4.Data.MySQL.Repositories
{
    public class ControlPlantaRepository : IControlPlantaRepository
    {
        // Nombre fijo de la tabla
        private const string _tableName = "CONTROL_PLANTA";

        private readonly MySQLDapperContext _context;

        public ControlPlantaRepository(MySQLDapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(ControlPlanta entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName}
                (idControlPlanta, idControl, idCompany)
                VALUES (@IdControlPlanta, @IdControl, @IdCompany)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un ControlPlanta en la base de datos.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<ControlPlanta?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE idControlPlanta = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<ControlPlanta>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el ControlPlanta con id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<ControlPlanta>> GetAllAsync()
        {
            const string sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<ControlPlanta>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de ControlPlanta.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(ControlPlanta entity)
        {
            const string sql = $@"
                UPDATE {_tableName} SET
                    idControl = @IdControl,
                    idCompany = @IdCompany
                WHERE idControlPlanta = @IdControlPlanta";

            try
            {
                using var conn = _context.CreateConnection();

                int rows = await conn.ExecuteAsync(sql, entity);

                if (rows == 0)
                    throw new Exception($"No se encontró el ControlPlanta con id {entity.IdControlPlanta} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el ControlPlanta con id {entity.IdControlPlanta}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = $@"DELETE FROM {_tableName} WHERE idControlPlanta = @Id";

            try
            {
                using var conn = _context.CreateConnection();

                int rows = await conn.ExecuteAsync(sql, new { Id = id });

                if (rows == 0)
                    throw new Exception($"No se encontró el ControlPlanta con id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el ControlPlanta con id {id}.", ex);
            }
        }
    }
}

