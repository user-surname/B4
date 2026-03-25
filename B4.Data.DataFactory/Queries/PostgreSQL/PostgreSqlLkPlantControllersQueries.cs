using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlLkPlantControllersQueries : ILkPlantControllersQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.lk_plant_controllers VALUES
            (@IdCompanyController, @IdCompany, @Controller, @Email, @CreatedAt, @UpdatedAt, @IsActive)";

        public string GetByIdQuery() => "SELECT * FROM b4.lk_plant_controllers WHERE idcompanycontroller = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.lk_plant_controllers LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.lk_plant_controllers SET
                idcompany=@IdCompany, controller=@Controller, email=@Email,
                updatedat=@UpdatedAt, isactive=@IsActive
            WHERE idcompanycontroller=@IdCompanyController";

        public string DeleteQuery() => "DELETE FROM b4.lk_plant_controllers WHERE idcompanycontroller = @Id";
    }
}
