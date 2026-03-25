using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlLkPlantControllersQueries : ILkPlantControllersQueries
    {
        public string AddQuery() => @"
            INSERT INTO LK_PLANT_CONTROLLERS (idCompanyController, IdCompany, Controller, Email)
            VALUES (@IdCompanyController, @IdCompany, @Controller, @Email)";

        public string GetByIdQuery() => "SELECT * FROM LK_PLANT_CONTROLLERS WHERE idCompanyController = @Id";

        public string GetAllQuery() => "SELECT * FROM LK_PLANT_CONTROLLERS";

        public string UpdateQuery() => @"
            UPDATE LK_PLANT_CONTROLLERS SET
                IdCompany = @IdCompany,
                Controller = @Controller,
                Email = @Email
            WHERE idCompanyController = @IdCompanyController";

        public string DeleteQuery() => "DELETE FROM LK_PLANT_CONTROLLERS WHERE idCompanyController = @Id";
    }
}
