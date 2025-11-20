using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;
using B4.Data.MySQL;
using System;

namespace B4.Data.MySQL.Repositories
{
    public class EpigrafeRepository : IEpigrafeRepository
    {
        // Nombre fijo de la tabla 
        private const string _tableName = "LK_EPIGRAFES";

        // Contexto que encapsula la cadena de conexión y creación de conexiones Dapper
        private readonly DapperContext _context;

        public EpigrafeRepository(DapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(LkEpigrafe entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName}
                (idEpigrafe, idPlantilla, idHoja, PreEpigrafe, Epigrafe, EpigrafeFull)
                VALUES (@IdEpigrafe, @IdPlantilla, @IdHoja, @PreEpigrafe, @Epigrafe, @EpigrafeFull)";

            try
            {
                using var conn = _context.CreateConnection();

                // Ejecuta la consulta INSERT
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                // Lanzamos una excepción más descriptiva para identificar el origen del error
                throw new Exception("Error al insertar un Epígrafe en la base de datos.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<LkEpigrafe?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE idEpigrafe = @Id";

            try
            {
                using var conn = _context.CreateConnection();

                // Devuelve 1 registro o null si no existe
                return await conn.QuerySingleOrDefaultAsync<LkEpigrafe>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el Epígrafe con id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<LkEpigrafe>> GetAllAsync()
        {
            const string sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();

                // Obtiene todos los registros de la tabla
                return await conn.QueryAsync<LkEpigrafe>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de Epígrafes.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(LkEpigrafe entity)
        {
            const string sql = $@"
                UPDATE {_tableName} SET 
                    idPlantilla = @IdPlantilla,
                    idHoja = @IdHoja,
                    PreEpigrafe = @PreEpigrafe,
                    Epigrafe = @Epigrafe,
                    EpigrafeFull = @EpigrafeFull
                WHERE idEpigrafe = @IdEpigrafe";

            try
            {
                using var conn = _context.CreateConnection();

                int rows = await conn.ExecuteAsync(sql, entity);

                // Comprobamos que realmente se actualizó un registro
                if (rows == 0)
                    throw new Exception($"No se encontró el Epígrafe con id {entity.IdEpigrafe} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el Epígrafe con id {entity.IdEpigrafe}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = $@"DELETE FROM {_tableName} WHERE idEpigrafe = @Id";

            try
            {
                using var conn = _context.CreateConnection();

                int rows = await conn.ExecuteAsync(sql, new { Id = id });

                // Validamos que se eliminó algún registro
                if (rows == 0)
                    throw new Exception($"No se encontró el Epígrafe con id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el Epígrafe con id {id}.", ex);
            }
        }
    }
}