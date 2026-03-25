using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlLkPlantCountryQueries : ILkPlantCountryQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.lk_plant_country VALUES (@IdCountry, @Country, @CreatedAt, @UpdatedAt, @IsActive)";

        public string GetByIdQuery() => "SELECT * FROM b4.lk_plant_country WHERE idcountry = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.lk_plant_country LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.lk_plant_country SET
                country=@Country, updatedat=@UpdatedAt, isactive=@IsActive
            WHERE idcountry=@IdCountry";

        public string DeleteQuery() => "DELETE FROM b4.lk_plant_country WHERE idcountry = @Id";
    }
}
