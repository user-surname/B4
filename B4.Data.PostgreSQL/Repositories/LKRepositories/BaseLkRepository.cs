using Dapper;

namespace B4.Data.PostgreSQL.Repositories
{
    public abstract class BaseLkRepository<T>
    {
        protected readonly PostgreSQLDapperContext _context;
        private readonly string _table;
        private readonly string _idColumn;

        protected BaseLkRepository(PostgreSQLDapperContext context, string table, string idColumn)
        {
            _context = context;
            _table = table;
            _idColumn = idColumn;
        }

        // GET ALL (LIMIT 2000)
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<T>($"SELECT * FROM {_table} LIMIT 2000;");
        }

        // GET BY ID
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<T>(
                $"SELECT * FROM {_table} WHERE {_idColumn}=@Id;",
                new { Id = id });
        }

        // DELETE
        public virtual async Task DeleteAsync(int id)
        {
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(
                $"DELETE FROM {_table} WHERE {_idColumn}=@Id;",
                new { Id = id });
        }

        // INSERT 
        public abstract Task AddAsync(T entity);

        // UPDATE 
        public abstract Task UpdateAsync(T entity);
    }
}
