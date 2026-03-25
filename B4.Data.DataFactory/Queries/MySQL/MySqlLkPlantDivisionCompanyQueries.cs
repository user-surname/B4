using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlLkPlantDivisionCompanyQueries : ILkPlantDivisionCompanyQueries
    {
        public string AddQuery() => @"
            INSERT INTO LK_PLANT_DIVISION_COMPANY (idDivisionCompany, DivisionCompany)
            VALUES (@IdDivisionCompany, @DivisionCompany)";

        public string GetByIdQuery() => "SELECT * FROM LK_PLANT_DIVISION_COMPANY WHERE idDivisionCompany = @Id";

        public string GetAllQuery() => "SELECT * FROM LK_PLANT_DIVISION_COMPANY";

        public string UpdateQuery() => @"
            UPDATE LK_PLANT_DIVISION_COMPANY SET
                DivisionCompany = @DivisionCompany
            WHERE idDivisionCompany = @IdDivisionCompany";

        public string DeleteQuery() => "DELETE FROM LK_PLANT_DIVISION_COMPANY WHERE idDivisionCompany = @Id";
    }
}
