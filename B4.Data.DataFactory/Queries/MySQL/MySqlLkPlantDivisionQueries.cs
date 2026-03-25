using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlLkPlantDivisionQueries : ILkPlantDivisionQueries
    {
        public string AddQuery() => @"
            INSERT INTO LK_PLANT_DIVISION (idDivision, Division)
            VALUES (@IdDivision, @Division)";

        public string GetByIdQuery() => "SELECT * FROM LK_PLANT_DIVISION WHERE idDivision = @Id";

        public string GetAllQuery() => "SELECT * FROM LK_PLANT_DIVISION";

        public string UpdateQuery() => @"
            UPDATE LK_PLANT_DIVISION SET
                Division = @Division
            WHERE idDivision = @IdDivision";

        public string DeleteQuery() => "DELETE FROM LK_PLANT_DIVISION WHERE idDivision = @Id";
    }
}
