using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System;
using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;

namespace B4.Data.MySQL.Repositories.LkRepositories
{
    public class CiclosRepository : ICiclosRepository
    {
        // Nombre de la tabla
        private const string _tableName = "LK_CICLOS";

        private readonly MySQLDapperContext _context;

        public CiclosRepository(MySQLDapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(LkCiclos entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName}
                (idCiclo, Ciclo, Descripcion)
                VALUES (@IdCiclo, @Ciclo, @Descripcion)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un Ciclo en la base de datos.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<LkCiclos?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<LkCiclos>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el Ciclo con id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<LkCiclos>> GetAllAsync()
        {
            const string sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<LkCiclos>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de Ciclos.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(LkCiclos entity)
        {
            const string sql = $@"
                UPDATE {_tableName} SET
                    idCiclo = @IdCiclo,
                    Ciclo = @Ciclo,
                    Descripcion = @Descripcion
                WHERE id = @Id";

            try
            {
                using var conn = _context.CreateConnection();

                int rows = await conn.ExecuteAsync(sql, entity);

                if (rows == 0)
                    throw new Exception($"No se encontró el Ciclo con id {entity.Id} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el Ciclo con id {entity.Id}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = $@"DELETE FROM {_tableName} WHERE id = @Id";

            try
            {
                using var conn = _context.CreateConnection();

                int rows = await conn.ExecuteAsync(sql, new { Id = id });

                if (rows == 0)
                    throw new Exception($"No se encontró el Ciclo con id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el Ciclo con id {id}.", ex);
            }
        }
    }
}

