using Dapper;
using Npgsql;
using B4.Models.Entities;
using B4.Models.Interfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class DataComentariosRepository : IDataComentariosRepository
    {
        private readonly string _connectionString;
        private const string _table = "b4.data_comentarios";

        public DataComentariosRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // --------------------------
        // C - CREATE
        // --------------------------
        public async Task AddAsync(DataComentarios entity)
        {
            var sql = $@"
                INSERT INTO {_table}
                (idapicarga, guidcarga, fechaultmodif, idcompany, ejercicio, idciclo, idfase, idepigrafe, etiqueta, comentario)
                VALUES 
                (@IdAPICarga, @GuidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdEpigrafe, @Etiqueta, @Comentario)
            RETURNING id;";

            using var conn = new NpgsqlConnection(_connectionString);

            var newId = await conn.ExecuteScalarAsync<int>(sql, entity);
            entity.Id = newId;    
        }

        // --------------------------
        // R - GET BY ID
        // --------------------------
        public async Task<DataComentarios?> GetByIdAsync(int id)
        {
            var sql = $@"SELECT * FROM {_table} WHERE id = @Id";

            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QuerySingleOrDefaultAsync<DataComentarios>(sql, new { Id = id });
        }

        // --------------------------
        // R - GET ALL
        // --------------------------
        public async Task<IEnumerable<DataComentarios>> GetAllAsync()
        {
            var sql = $@"SELECT * FROM {_table}";

            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<DataComentarios>(sql);
        }

        // --------------------------
        // U - UPDATE
        // --------------------------
        public async Task UpdateAsync(DataComentarios entity)
        {
            var sql = $@"
                UPDATE {_table} SET
                    idapicarga     = @IdAPICarga,
                    guidcarga      = @GuidCarga,
                    fechaultmodif  = @FechaUltModif,
                    idcompany      = @IdCompany,
                    ejercicio      = @Ejercicio,
                    idciclo        = @IdCiclo,
                    idfase         = @IdFase,
                    idepigrafe     = @IdEpigrafe,
                    etiqueta       = @Etiqueta,
                    comentario     = @Comentario
                WHERE id = @Id";

            using var conn = new NpgsqlConnection(_connectionString);
            int affected = await conn.ExecuteAsync(sql, entity);

            if (affected == 0)
                throw new Exception($"No existe comentario con ID {entity.Id}");
        }

        // --------------------------
        // D - DELETE
        // --------------------------
        public async Task DeleteAsync(int id)
        {
            var sql = $@"DELETE FROM {_table} WHERE id = @Id";

            using var conn = new NpgsqlConnection(_connectionString);
            int affected = await conn.ExecuteAsync(sql, new { Id = id });

            if (affected == 0)
                throw new Exception($"No existe comentario con ID {id}");
        }
    }
}
