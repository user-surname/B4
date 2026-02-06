using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantDivisionRepository
        : BaseLkRepository<LkPlantDivision>, IPlantDivisionRepository
    {
        public PlantDivisionRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_plant_division", "iddivision") { }

        public override async Task AddAsync(LkPlantDivision entity)
        {
            PrepareForInsert(entity);

            const string sql = @"
                INSERT INTO b4.lk_plant_division
                    (iddivision, division, createdat, updatedat, isactive)
                VALUES
                    (@IdDivision, @Division, @CreatedAt, @UpdatedAt, @IsActive);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkPlantDivision entity)
        {
            PrepareForUpdate(entity);

            const string sql = @"
                UPDATE b4.lk_plant_division 
                SET division=@Division,
                    updatedat=@UpdatedAt,
                    isactive=@IsActive
                WHERE iddivision=@IdDivision;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
