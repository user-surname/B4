using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories.LKRepositories
{
    public class EpigrafeRepository
        : BaseLkRepository<LkEpigrafe>, IEpigrafeRepository
    {
        public EpigrafeRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_epigrafe", "idepigrafe") { }

        public override async Task AddAsync(LkEpigrafe entity)
        {
            PrepareForInsert(entity);

            const string sql = @"
                INSERT INTO b4.lk_epigrafe
                    (idepigrafe, epigrafe, createdat, updatedat, isactive)
                VALUES
                    (@IdEpigrafe, @Epigrafe, @CreatedAt, @UpdatedAt, @IsActive);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkEpigrafe entity)
        {
            PrepareForUpdate(entity);

            const string sql = @"
                UPDATE b4.lk_epigrafe SET 
                    epigrafe=@Epigrafe,
                    updatedat=@UpdatedAt,
                    isactive=@IsActive
                WHERE idepigrafe=@IdEpigrafe;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
