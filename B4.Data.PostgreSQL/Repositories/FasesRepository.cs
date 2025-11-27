using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class FasesRepository : IFasesRepository
    {
        private readonly DapperContext _context;

        public FasesRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LkFases entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_fases (idfase, fase, fasealias)
                VALUES (@IdFase, @Fase, @FaseAlias);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task<LkFases?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM b4.lk_fases WHERE idfase = @Id;";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<LkFases>(sql, new { Id = id });
        }

        public async Task<IEnumerable<LkFases>> GetAllAsync()
        {
            const string sql = "SELECT * FROM b4.lk_fases;";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<LkFases>(sql);
        }

        public async Task UpdateAsync(LkFases entity)
        {
            const string sql = @"
                UPDATE b4.lk_fases SET 
                    fase=@Fase,
                    fasealias=@FaseAlias
                WHERE idfase=@IdFase;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM b4.lk_fases WHERE idfase=@Id;";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
