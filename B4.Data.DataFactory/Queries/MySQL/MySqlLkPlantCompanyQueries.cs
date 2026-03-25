using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlLkPlantCompanyQueries : ILkPlantCompanyQueries
    {
        public string AddQuery() => @"
            INSERT INTO LK_PLANT_COMPANY
            (IdCompany, CompanyCode, ManagementCompany, idCurrency, Company, Active,
             idDivision, idDivisionCompany, idSubdivision, idCountry, Location, obs,
             RegionalValidatorPwd, Region, RegionalValidator, RegionalValidatorEmail,
             ContributorPwd, ManagementCompanyBackup, RegionBackup, CountryBackup, idRegValidator)
            VALUES
            (@IdCompany, @CompanyCode, @ManagementCompany, @IdCurrency, @Company, @Active,
             @IdDivision, @IdDivisionCompany, @IdSubdivision, @IdCountry, @Location, @Obs,
             @RegionalValidatorPwd, @Region, @RegionalValidator, @RegionalValidatorEmail,
             @ContributorPwd, @ManagementCompanyBackup, @RegionBackup, @CountryBackup, @IdRegValidator)";

        public string GetByIdQuery() => "SELECT * FROM LK_PLANT_COMPANY WHERE IdCompany = @Id";

        public string GetAllQuery() => "SELECT * FROM LK_PLANT_COMPANY";

        public string UpdateQuery() => @"
            UPDATE LK_PLANT_COMPANY SET
                CompanyCode = @CompanyCode,
                ManagementCompany = @ManagementCompany,
                idCurrency = @IdCurrency,
                Company = @Company,
                Active = @Active,
                idDivision = @IdDivision,
                idDivisionCompany = @IdDivisionCompany,
                idSubdivision = @IdSubdivision,
                idCountry = @IdCountry,
                Location = @Location,
                obs = @Obs,
                RegionalValidatorPwd = @RegionalValidatorPwd,
                Region = @Region,
                RegionalValidator = @RegionalValidator,
                RegionalValidatorEmail = @RegionalValidatorEmail,
                ContributorPwd = @ContributorPwd,
                ManagementCompanyBackup = @ManagementCompanyBackup,
                RegionBackup = @RegionBackup,
                CountryBackup = @CountryBackup,
                idRegValidator = @IdRegValidator
            WHERE IdCompany = @IdCompany";

        public string DeleteQuery() => "DELETE FROM LK_PLANT_COMPANY WHERE IdCompany = @Id";
    }
}
