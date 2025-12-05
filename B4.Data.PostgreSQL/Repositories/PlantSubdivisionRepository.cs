using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantSubdivisionRepository : IPlantSubdivisionRepository
    {
        private readonly DapperContext _context;

        public PlantSubdivisionRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LkPlantSubdivision entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plant_subdivision (idsubdivision, subdivision)
                VALUES (@IdSubdivision, @Subdivision);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task<LkPlantSubdivision?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM b4.lk_plant_subdivision WHERE idsubdivision=@Id;";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<LkPlantSubdivision>(sql, new { Id = id });
        }

        public async Task<IEnumerable<LkPlantSubdivision>> GetAllAsync()
        {
            const string sql = "SELECT * FROM b4.lk_plant_subdivision;";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<LkPlantSubdivision>(sql);
        }

        public async Task UpdateAsync(LkPlantSubdivision entity)
        {
            const string sql = @"
                UPDATE b4.lk_plant_subdivision SET 
                    subdivision=@Subdivision
                WHERE idsubdivision=@IdSubdivision;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM b4.lk_plant_subdivision WHERE idsubdivision=@Id;";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
