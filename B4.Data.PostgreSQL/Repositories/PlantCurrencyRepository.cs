using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantCurrencyRepository : IPlantCurrencyRepository
    {
        private readonly DapperContext _context;

        public PlantCurrencyRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LkPlantCurrency entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plant_currency (idcurrency, currency, currencyalias)
                VALUES (@IdCurrency, @Currency, @CurrencyAlias);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task<LkPlantCurrency?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM b4.lk_plant_currency WHERE idcurrency=@Id;";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<LkPlantCurrency>(sql, new { Id = id });
        }

        public async Task<IEnumerable<LkPlantCurrency>> GetAllAsync()
        {
            const string sql = "SELECT * FROM b4.lk_plant_currency;";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<LkPlantCurrency>(sql);
        }

        public async Task UpdateAsync(LkPlantCurrency entity)
        {
            const string sql = @"
                UPDATE b4.lk_plant_currency SET 
                    currency=@Currency,
                    currencyalias=@CurrencyAlias
                WHERE idcurrency=@IdCurrency;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM b4.lk_plant_currency WHERE idcurrency=@Id;";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
