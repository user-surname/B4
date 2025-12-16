using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantCountryRepository
        : BaseLkRepository<LkPlantCountry>, IPlantCountryRepository
    {
        public PlantCountryRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_plant_country", "idcountry") { }

        public override async Task AddAsync(LkPlantCountry entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plant_country (idcountry, country)
                VALUES (@IdCountry, @Country);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkPlantCountry entity)
        {
            const string sql = @"
                UPDATE b4.lk_plant_country 
                SET country=@Country
                WHERE idcountry=@IdCountry;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
