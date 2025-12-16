using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantSubdivisionRepository
        : BaseLkRepository<LkPlantSubdivision>, IPlantSubdivisionRepository
    {
        public PlantSubdivisionRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_plant_subdivision", "idsubdivision") { }

        public override async Task AddAsync(LkPlantSubdivision entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plant_subdivision (idsubdivision, subdivision)
                VALUES (@IdSubdivision, @Subdivision);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkPlantSubdivision entity)
        {
            const string sql = @"
                UPDATE b4.lk_plant_subdivision 
                SET subdivision=@Subdivision
                WHERE idsubdivision=@IdSubdivision;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
