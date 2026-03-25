using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlLkPlantDivisionCompanyQueries : ILkPlantDivisionCompanyQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.lk_plant_division_company VALUES (@IdDivisionCompany, @DivisionCompany, @CreatedAt, @UpdatedAt, @IsActive)";

        public string GetByIdQuery() => "SELECT * FROM b4.lk_plant_division_company WHERE iddivisioncompany = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.lk_plant_division_company LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.lk_plant_division_company SET
                divisioncompany=@DivisionCompany, updatedat=@UpdatedAt, isactive=@IsActive
            WHERE iddivisioncompany=@IdDivisionCompany";

        public string DeleteQuery() => "DELETE FROM b4.lk_plant_division_company WHERE iddivisioncompany = @Id";
    }
}
