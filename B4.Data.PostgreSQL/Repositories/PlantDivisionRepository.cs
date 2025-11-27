using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantDivisionRepository : IPlantDivisionRepository
    {
        private readonly DapperContext _context;

        public PlantDivisionRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LkPlantDivision entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plant_division (iddivision, division)
                VALUES (@IdDivision, @Division);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task<LkPlantDivision?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM b4.lk_plant_division WHERE iddivision=@Id;";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<LkPlantDivision>(sql, new { Id = id });
        }

        public async Task<IEnumerable<LkPlantDivision>> GetAllAsync()
        {
            const string sql = "SELECT * FROM b4.lk_plant_division;";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<LkPlantDivision>(sql);
        }

        public async Task UpdateAsync(LkPlantDivision entity)
        {
            const string sql = @"
                UPDATE b4.lk_plant_division SET 
                    division=@Division
                WHERE iddivision=@IdDivision;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM b4.lk_plant_division WHERE iddivision=@Id;";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
