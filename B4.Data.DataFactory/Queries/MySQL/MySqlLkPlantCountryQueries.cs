using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlLkPlantCountryQueries : ILkPlantCountryQueries
    {
        public string AddQuery() => @"
            INSERT INTO LK_PLANT_COUNTRY (idCountry, Country)
            VALUES (@IdCountry, @Country)";

        public string GetByIdQuery() => "SELECT * FROM LK_PLANT_COUNTRY WHERE idCountry = @Id";

        public string GetAllQuery() => "SELECT * FROM LK_PLANT_COUNTRY";

        public string UpdateQuery() => @"
            UPDATE LK_PLANT_COUNTRY SET
                Country = @Country
            WHERE idCountry = @IdCountry";

        public string DeleteQuery() => "DELETE FROM LK_PLANT_COUNTRY WHERE idCountry = @Id";
    }
}
