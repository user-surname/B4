using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlLkPlantTreeQueries : ILkPlantTreeQueries
    {
        public string AddQuery() => @"
            INSERT INTO LK_PLANT_TREE (idTree, idDivision, idDivisionCompany, idSubdivision, idCountry)
            VALUES (@IdTree, @IdDivision, @IdDivisionCompany, @IdSubdivision, @IdCountry)";

        public string GetByIdQuery() => "SELECT * FROM LK_PLANT_TREE WHERE idTree = @Id";

        public string GetAllQuery() => "SELECT * FROM LK_PLANT_TREE";

        public string UpdateQuery() => @"
            UPDATE LK_PLANT_TREE SET
                idDivision = @IdDivision,
                idDivisionCompany = @IdDivisionCompany,
                idSubdivision = @IdSubdivision,
                idCountry = @IdCountry
            WHERE idTree = @IdTree";

        public string DeleteQuery() => "DELETE FROM LK_PLANT_TREE WHERE idTree = @Id";
    }
}
