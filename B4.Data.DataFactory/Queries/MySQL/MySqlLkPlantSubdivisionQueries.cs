using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlLkPlantSubdivisionQueries : ILkPlantSubdivisionQueries
    {
        public string AddQuery() => @"
            INSERT INTO LK_PLANT_SUBDIVISION (idSubdivision, Subdivision)
            VALUES (@IdSubdivision, @Subdivision)";

        public string GetByIdQuery() => "SELECT * FROM LK_PLANT_SUBDIVISION WHERE idSubdivision = @Id";

        public string GetAllQuery() => "SELECT * FROM LK_PLANT_SUBDIVISION";

        public string UpdateQuery() => @"
            UPDATE LK_PLANT_SUBDIVISION SET
                Subdivision = @Subdivision
            WHERE idSubdivision = @IdSubdivision";

        public string DeleteQuery() => "DELETE FROM LK_PLANT_SUBDIVISION WHERE idSubdivision = @Id";
    }
}
