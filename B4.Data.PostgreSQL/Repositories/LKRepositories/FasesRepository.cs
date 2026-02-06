using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class FasesRepository
        : BaseLkRepository<LkFases>, IFasesRepository
    {
        public FasesRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_fases", "idfase") { }

        public override async Task AddAsync(LkFases entity)
        {
            PrepareForInsert(entity);

            const string sql = @"
                INSERT INTO b4.lk_fases
                    (idfase, fase, createdat, updatedat, isactive)
                VALUES
                    (@IdFase, @Fase, @CreatedAt, @UpdatedAt, @IsActive);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkFases entity)
        {
            PrepareForUpdate(entity);

            const string sql = @"
                UPDATE b4.lk_fases 
                SET fase=@Fase,
                    updatedat=@UpdatedAt,
                    isactive=@IsActive
                WHERE idfase=@IdFase;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
