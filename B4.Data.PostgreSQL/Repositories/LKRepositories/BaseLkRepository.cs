using Dapper;
using B4.Models.Entities.LkEntities;

namespace B4.Data.PostgreSQL.Repositories
{
    public abstract class BaseLkRepository<T> where T : LkBase
    {
        protected readonly PostgreSQLDapperContext _context;
        protected readonly string _table;
        protected readonly string _idColumn;

        protected BaseLkRepository(PostgreSQLDapperContext context, string table, string idColumn)
        {
            _context = context;
            _table = table;
            _idColumn = idColumn;
        }

        // ---------------------------
        // R - GET ALL (LIMIT 2000)
        // ---------------------------
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<T>($"SELECT * FROM {_table} LIMIT 2000;");
        }

        // ---------------------------
        // R - GET BY ID
        // ---------------------------
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<T>(
                $"SELECT * FROM {_table} WHERE {_idColumn}=@Id;",
                new { Id = id });
        }

        // ---------------------------
        // D - DELETE (hard delete)
        // ---------------------------
        public virtual async Task DeleteAsync(int id)
        {
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(
                $"DELETE FROM {_table} WHERE {_idColumn}=@Id;",
                new { Id = id });
        }

        // ---------------------------
        // Helpers comunes LK
        // ---------------------------
        protected virtual void PrepareForInsert(T entity)
        {
            var now = DateTime.UtcNow;

            entity.CreatedAt = now;
            entity.UpdatedAt = now;

            // Por defecto: activo
            if (entity.IsActive != 0 && entity.IsActive != 1)
                entity.IsActive = 1;
        }

        protected virtual void PrepareForUpdate(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
        }

        // ---------------------------
        // C - CREATE (abstract)
        // ---------------------------
        public abstract Task AddAsync(T entity);

        // ---------------------------
        // U - UPDATE (abstract)
        // ---------------------------
        public abstract Task UpdateAsync(T entity);
    }
}
