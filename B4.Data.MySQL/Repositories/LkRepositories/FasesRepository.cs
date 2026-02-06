using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using Dapper;
using System.Data;

namespace B4.Data.MySQL.Repositories.LkRepositories
{
    public class FasesRepository : IFasesRepository
    {
        private readonly MySQLDapperContext _context;

        public FasesRepository(MySQLDapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------------------
        // INSERT
        // -------------------------------------------------------------
        public async Task AddAsync(LkFases fase)
        {
            var sql = @"
                INSERT INTO LK_FASES (idFase, Fase, FaseAlias)
                VALUES (@IdFase, @Fase, @FaseAlias);
            ";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, fase);
        }

        // -------------------------------------------------------------
        // SELECT BY ID
        // -------------------------------------------------------------
        public async Task<LkFases?> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT idFase AS IdFase, Fase, FaseAlias
                FROM LK_FASES
                WHERE idFase = @Id;
            ";

            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<LkFases>(sql, new { Id = id });
        }

        // -------------------------------------------------------------
        // SELECT ALL
        // -------------------------------------------------------------
        public async Task<IEnumerable<LkFases>> GetAllAsync()
        {
            var sql = @"
                SELECT idFase AS IdFase, Fase, FaseAlias
                FROM LK_FASES;
            ";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<LkFases>(sql);
        }

        // -------------------------------------------------------------
        // UPDATE
        // -------------------------------------------------------------
        public async Task UpdateAsync(LkFases fase)
        {
            var sql = @"
                UPDATE LK_FASES
                SET 
                    Fase = @Fase,
                    FaseAlias = @FaseAlias
                WHERE idFase = @IdFase;
            ";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, fase);
        }

        // -------------------------------------------------------------
        // DELETE
        // -------------------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            var sql = @"
                DELETE FROM LK_FASES
                WHERE idFase = @Id;
            ";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}

