using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantCountryRepository : IPlantCountryRepository
    {
        private readonly DapperContext _context;

        public PlantCountryRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LkPlantCountry entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plant_country (idcountry, country)
                VALUES (@IdCountry, @Country);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task<LkPlantCountry?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM b4.lk_plant_country WHERE idcountry=@Id;";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<LkPlantCountry>(sql, new { Id = id });
        }

        public async Task<IEnumerable<LkPlantCountry>> GetAllAsync()
        {
            const string sql = "SELECT * FROM b4.lk_plant_country;";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<LkPlantCountry>(sql);
        }

        public async Task UpdateAsync(LkPlantCountry entity)
        {
            const string sql = @"
                UPDATE b4.lk_plant_country SET 
                    country=@Country
                WHERE idcountry=@IdCountry;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM b4.lk_plant_country WHERE idcountry=@Id;";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
