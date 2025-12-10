using Dapper;
using Npgsql;

namespace B4.Data.PostgreSQL.Repositories
{
    public abstract class BaseRepository<T>
    {
        protected readonly string _connectionString;
        protected readonly string _table;

        protected BaseRepository(string connectionString, string table)
        {
            _connectionString = connectionString;
            _table = table;
        }

        protected NpgsqlConnection GetConnection()
            => new NpgsqlConnection(_connectionString);

        // ---------------------------
        // R - GET BY ID
        // ---------------------------
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            using var conn = GetConnection();
            var sql = $"SELECT * FROM {_table} WHERE id = @Id";
            return await conn.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });
        }

        // ---------------------------
        // R - GET ALL (LIMIT 2000)
        // ---------------------------
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using var conn = GetConnection();
            return await conn.QueryAsync<T>($"SELECT * FROM {_table} LIMIT 2000");
        }

        // ---------------------------
        // D - DELETE
        // ---------------------------
        public virtual async Task DeleteAsync(int id)
        {
            using var conn = GetConnection();
            var sql = $"DELETE FROM {_table} WHERE id = @Id";
            int affected = await conn.ExecuteAsync(sql, new { Id = id });

            if (affected == 0)
                throw new Exception($"No existe registro con ID {id} en {_table}");
        }

        // ---------------------------
        // C - CREATE (virtual)
        // ---------------------------
        public virtual Task AddAsync(T entity)
        {
            throw new NotImplementedException(
                $"AddAsync no está implementado para '{typeof(T).Name}'");
        }

        // ---------------------------
        // U - UPDATE (virtual)
        // ---------------------------
        public virtual Task UpdateAsync(T entity)
        {
            throw new NotImplementedException(
                $"UpdateAsync no está implementado para '{typeof(T).Name}'");
        }
    }
}
