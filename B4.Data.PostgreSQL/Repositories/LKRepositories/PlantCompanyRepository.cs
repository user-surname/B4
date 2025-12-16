using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantCompanyRepository
        : BaseLkRepository<LkPlantCompany>, IPlantCompanyRepository
    {
        public PlantCompanyRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_plant_company", "idcompany") { }

        public override async Task AddAsync(LkPlantCompany entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plant_company (idcompany, company)
                VALUES (@IdCompany, @Company);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkPlantCompany entity)
        {
            const string sql = @"
                UPDATE b4.lk_plant_company 
                SET company=@Company
                WHERE idcompany=@IdCompany;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
