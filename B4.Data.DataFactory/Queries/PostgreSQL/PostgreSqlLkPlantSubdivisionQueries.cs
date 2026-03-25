using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlLkPlantSubdivisionQueries : ILkPlantSubdivisionQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.lk_plant_subdivision VALUES (@IdSubdivision, @Subdivision, @CreatedAt, @UpdatedAt, @IsActive)";

        public string GetByIdQuery() => "SELECT * FROM b4.lk_plant_subdivision WHERE idsubdivision = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.lk_plant_subdivision LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.lk_plant_subdivision SET
                subdivision=@Subdivision, updatedat=@UpdatedAt, isactive=@IsActive
            WHERE idsubdivision=@IdSubdivision";

        public string DeleteQuery() => "DELETE FROM b4.lk_plant_subdivision WHERE idsubdivision = @Id";
    }
}
