using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlLkPlantCompanyQueries : ILkPlantCompanyQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.lk_plant_company
            (idcompany, companycode, managementcompany, idcurrency, company, active,
             iddivision, iddivisioncompany, idsubdivision, idcountry, location, obs,
             regionalvalidatorpwd, region, regionalvalidator, regionalvalidatoremail,
             contributorpwd, managementcompanybackup, regionbackup, countrybackup,
             idregvalidator, createdat, updatedat, isactive)
            VALUES
            (@IdCompany, @CompanyCode, @ManagementCompany, @IdCurrency, @Company, @Active,
             @IdDivision, @IdDivisionCompany, @IdSubdivision, @IdCountry, @Location, @Obs,
             @RegionalValidatorPwd, @Region, @RegionalValidator, @RegionalValidatorEmail,
             @ContributorPwd, @ManagementCompanyBackup, @RegionBackup, @CountryBackup,
             @IdRegValidator, @CreatedAt, @UpdatedAt, @IsActive)";

        public string GetByIdQuery() => "SELECT * FROM b4.lk_plant_company WHERE idcompany = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.lk_plant_company LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.lk_plant_company SET
                companycode=@CompanyCode, managementcompany=@ManagementCompany, idcurrency=@IdCurrency,
                company=@Company, active=@Active, iddivision=@IdDivision, iddivisioncompany=@IdDivisionCompany,
                idsubdivision=@IdSubdivision, idcountry=@IdCountry, location=@Location, obs=@Obs,
                regionalvalidatorpwd=@RegionalValidatorPwd, region=@Region, regionalvalidator=@RegionalValidator,
                regionalvalidatoremail=@RegionalValidatorEmail, contributorpwd=@ContributorPwd,
                managementcompanybackup=@ManagementCompanyBackup, regionbackup=@RegionBackup,
                countrybackup=@CountryBackup, idregvalidator=@IdRegValidator,
                updatedat=@UpdatedAt, isactive=@IsActive
            WHERE idcompany=@IdCompany";

        public string DeleteQuery() => "DELETE FROM b4.lk_plant_company WHERE idcompany = @Id";
    }
}
