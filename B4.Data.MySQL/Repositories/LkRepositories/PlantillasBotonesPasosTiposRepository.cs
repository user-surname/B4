using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System;
using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;

namespace B4.Data.MySQL.Repositories.LkRepositories
{
    public class PlantillasBotonesPasosTiposRepository : IPlantillasBotonesPasosTiposRepository
    {
        private const string _tableName = "LK_PLANTILLAS_BOTONES_PASOS_TIPOS";
        private readonly MySQLDapperContext _context;

        public PlantillasBotonesPasosTiposRepository(MySQLDapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(LkPlantillasBotonesPasosTipos entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName}
                (idPasoTipo, Pasotipo, descripcion)
                VALUES (@IdPasoTipo, @Pasotipo, @Descripcion)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un PasoTipo en la base de datos.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<LkPlantillasBotonesPasosTipos?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE idPasoTipo = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<LkPlantillasBotonesPasosTipos>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el PasoTipo con id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<LkPlantillasBotonesPasosTipos>> GetAllAsync()
        {
            const string sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<LkPlantillasBotonesPasosTipos>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de PasoTipos.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(LkPlantillasBotonesPasosTipos entity)
        {
            const string sql = $@"
                UPDATE {_tableName} SET 
                    Pasotipo = @Pasotipo,
                    descripcion = @Descripcion
                WHERE idPasoTipo = @IdPasoTipo";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, entity);

                if (rows == 0)
                    throw new Exception($"No se encontró el PasoTipo con id {entity.IdPasoTipo} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el PasoTipo con id {entity.IdPasoTipo}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = $@"DELETE FROM {_tableName} WHERE idPasoTipo = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, new { Id = id });

                if (rows == 0)
                    throw new Exception($"No se encontró el PasoTipo con id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el PasoTipo con id {id}.", ex);
            }
        }
    }
}

