using Dapper;
using Npgsql;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class CiclosRepository : ICiclosRepository
    {
        private readonly DapperContext _context;

        public CiclosRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LkCiclos entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_ciclos (idciclo, ciclo, descripcion)
                VALUES (@IdCiclo, @Ciclo, @Descripcion);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task<LkCiclos?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM b4.lk_ciclos WHERE id = @Id;";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<LkCiclos>(sql, new { Id = id });
        }

        public async Task<IEnumerable<LkCiclos>> GetAllAsync()
        {
            const string sql = "SELECT * FROM b4.lk_ciclos;";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<LkCiclos>(sql);
        }

        public async Task UpdateAsync(LkCiclos entity)
        {
            const string sql = @"
                UPDATE b4.lk_ciclos SET 
                    idciclo=@IdCiclo, 
                    ciclo=@Ciclo,
                    descripcion=@Descripcion
                WHERE id=@Id;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM b4.lk_ciclos WHERE id=@Id;";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
