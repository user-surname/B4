using B4.Models.Interfaces;
using Dapper;
using System.Data;

namespace B4.Data.PostgreSQL.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T>
        where T : class
    {
        protected readonly PostgreSQLDapperContext _context;
        protected readonly string _table;

        protected BaseRepository(PostgreSQLDapperContext context, string table)
        {
            _context = context;
            _table = table;
        }

        protected IDbConnection GetConnection()
            => _context.CreateConnection();

        // ---------------------------
        // R - GET BY ID
        // ---------------------------
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            var sql = $"SELECT * FROM {_table} WHERE id = @Id";

            using var conn = GetConnection();
            return await conn.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });
        }

        // ---------------------------
        // R - GET ALL (LIMIT 2000)
        // ---------------------------
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {_table} LIMIT 2000";

            using var conn = GetConnection();
            return await conn.QueryAsync<T>(sql);
        }

        // ---------------------------
        // D - DELETE
        // ---------------------------
        public virtual async Task DeleteAsync(int id)
        {
            var sql = $"DELETE FROM {_table} WHERE id = @Id";

            using var conn = GetConnection();
            var affected = await conn.ExecuteAsync(sql, new { Id = id });

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
