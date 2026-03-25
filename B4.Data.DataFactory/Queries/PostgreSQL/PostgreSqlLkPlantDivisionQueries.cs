using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlLkPlantDivisionQueries : ILkPlantDivisionQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.lk_plant_division VALUES (@IdDivision, @Division, @CreatedAt, @UpdatedAt, @IsActive)";

        public string GetByIdQuery() => "SELECT * FROM b4.lk_plant_division WHERE iddivision = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.lk_plant_division LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.lk_plant_division SET
                division=@Division, updatedat=@UpdatedAt, isactive=@IsActive
            WHERE iddivision=@IdDivision";

        public string DeleteQuery() => "DELETE FROM b4.lk_plant_division WHERE iddivision = @Id";
    }
}
