using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System;
using B4.Models.Entities.DataEtities;
using B4.Models.Interfaces.DataInterfaces;

namespace B4.Data.MySQL.Repositories.DataRepositories
{
    public class DataComentariosRepository : IDataComentariosRepository
    {
        private const string _tableName = "DATA_Comentarios";

        private readonly MySQLDapperContext _context;

        public DataComentariosRepository(MySQLDapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(DataComentarios entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName}
                (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdEpigrafe, Etiqueta, Comentario)
                VALUES (@IdAPICarga, @guidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdEpigrafe, @Etiqueta, @Comentario)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un comentario en la base de datos.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<DataComentarios?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE Id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<DataComentarios>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el comentario con Id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<DataComentarios>> GetAllAsync()
        {
            const string sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<DataComentarios>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de comentarios.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(DataComentarios entity)
        {
            const string sql = $@"
                UPDATE {_tableName} SET
                    IdAPICarga = @IdAPICarga,
                    guidCarga = @guidCarga,
                    FechaUltModif = @FechaUltModif,
                    IdCompany = @IdCompany,
                    Ejercicio = @Ejercicio,
                    IdCiclo = @IdCiclo,
                    IdFase = @IdFase,
                    IdEpigrafe = @IdEpigrafe,
                    Etiqueta = @Etiqueta,
                    Comentario = @Comentario
                WHERE Id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, entity);

                if (rows == 0)
                    throw new Exception($"No se encontró el comentario con Id {entity.Id} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el comentario con Id {entity.Id}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = $@"DELETE FROM {_tableName} WHERE Id = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, new { Id = id });

                if (rows == 0)
                    throw new Exception($"No se encontró el comentario con Id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el comentario con Id {id}.", ex);
            }
        }
    }
}

