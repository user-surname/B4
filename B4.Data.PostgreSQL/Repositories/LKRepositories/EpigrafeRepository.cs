using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class EpigrafeRepository
        : BaseLkRepository<LkEpigrafe>, IEpigrafeRepository
    {
        public EpigrafeRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_epigrafe", "idepigrafe") { }

        public override async Task AddAsync(LkEpigrafe entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_epigrafe (idepigrafe, epigrafe)
                VALUES (@IdEpigrafe, @Epigrafe);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkEpigrafe entity)
        {
            const string sql = @"
                UPDATE b4.lk_epigrafe SET 
                    epigrafe=@Epigrafe
                WHERE idepigrafe=@IdEpigrafe;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
