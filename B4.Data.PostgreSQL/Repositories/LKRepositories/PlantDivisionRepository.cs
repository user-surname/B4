using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantDivisionRepository
        : BaseLkRepository<LkPlantDivision>, IPlantDivisionRepository
    {
        public PlantDivisionRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_plant_division", "iddivision") { }

        public override async Task AddAsync(LkPlantDivision entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plant_division (iddivision, division)
                VALUES (@IdDivision, @Division);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkPlantDivision entity)
        {
            const string sql = @"
                UPDATE b4.lk_plant_division 
                SET division=@Division
                WHERE iddivision=@IdDivision;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
