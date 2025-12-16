using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantCurrencyRepository
        : BaseLkRepository<LkPlantCurrency>, IPlantCurrencyRepository
    {
        public PlantCurrencyRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_plant_currency", "idcurrency") { }

        public override async Task AddAsync(LkPlantCurrency entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plant_currency (idcurrency, currency)
                VALUES (@IdCurrency, @Currency);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkPlantCurrency entity)
        {
            const string sql = @"
                UPDATE b4.lk_plant_currency 
                SET currency=@Currency
                WHERE idcurrency=@IdCurrency;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
