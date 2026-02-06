using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantCompanyRepository
        : BaseLkRepository<LkPlantCompany>, IPlantCompanyRepository
    {
        public PlantCompanyRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_plant_company", "idcompany") { }

        public override async Task AddAsync(LkPlantCompany entity)
        {
            PrepareForInsert(entity);

            const string sql = @"
                INSERT INTO b4.lk_plant_company
                    (idcompany, company, createdat, updatedat, isactive)
                VALUES
                    (@IdCompany, @Company, @CreatedAt, @UpdatedAt, @IsActive);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkPlantCompany entity)
        {
            PrepareForUpdate(entity);

            const string sql = @"
                UPDATE b4.lk_plant_company 
                SET company=@Company,
                    updatedat=@UpdatedAt,
                    isactive=@IsActive
                WHERE idcompany=@IdCompany;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
