using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantDivisionCompanyRepository : IPlantDivisionCompanyRepository
    {
        private readonly DapperContext _context;

        public PlantDivisionCompanyRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LkPlantDivisionCompany entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plant_division_company (iddivisioncompany, divisioncompany)
                VALUES (@IdDivisionCompany, @DivisionCompany);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task<LkPlantDivisionCompany?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM b4.lk_plant_division_company WHERE iddivisioncompany=@Id;";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<LkPlantDivisionCompany>(sql, new { Id = id });
        }

        public async Task<IEnumerable<LkPlantDivisionCompany>> GetAllAsync()
        {
            const string sql = "SELECT * FROM b4.lk_plant_division_company;";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<LkPlantDivisionCompany>(sql);
        }

        public async Task UpdateAsync(LkPlantDivisionCompany entity)
        {
            const string sql = @"
                UPDATE b4.lk_plant_division_company SET 
                    divisioncompany=@DivisionCompany
                WHERE iddivisioncompany=@IdDivisionCompany;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM b4.lk_plant_division_company WHERE iddivisioncompany=@Id;";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
