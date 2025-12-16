using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantDivisionCompanyRepository
        : BaseLkRepository<LkPlantDivisionCompany>, IPlantDivisionCompanyRepository
    {
        public PlantDivisionCompanyRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_plant_division_company", "iddivisioncompany") { }

        public override async Task AddAsync(LkPlantDivisionCompany entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plant_division_company
                (iddivisioncompany, divisioncompany)
                VALUES (@IdDivisionCompany, @DivisionCompany);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkPlantDivisionCompany entity)
        {
            const string sql = @"
                UPDATE b4.lk_plant_division_company 
                SET divisioncompany=@DivisionCompany
                WHERE iddivisioncompany=@IdDivisionCompany;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
