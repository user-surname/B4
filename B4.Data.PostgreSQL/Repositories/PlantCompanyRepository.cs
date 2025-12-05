using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantCompanyRepository : IPlantCompanyRepository
    {
        private readonly DapperContext _context;

        public PlantCompanyRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LkPlantCompany entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plant_company
                (idcompany, companycode, managementcompany, idcurrency, company,
                 active, iddivision, iddivisioncompany, idsubdivision, idcountry,
                 location, obs, regionalvalidatorpwd, region, regionalvalidator,
                 regionalvalidatoremail, contributorpwd, managementcompanybackup,
                 regionbackup, countrybackup, idregvalidator)
                VALUES
                (@IdCompany, @CompanyCode, @ManagementCompany, @IdCurrency, @Company,
                 @Active, @IdDivision, @IdDivisionCompany, @IdSubdivision, @IdCountry,
                 @Location, @Obs, @RegionalValidatorPwd, @Region, @RegionalValidator,
                 @RegionalValidatorEmail, @ContributorPwd, @ManagementCompanyBackup,
                 @RegionBackup, @CountryBackup, @IdRegValidator);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task<LkPlantCompany?> GetByIdAsync(int id)
        {
            const string sql =
                "SELECT * FROM b4.lk_plant_company WHERE idcompany=@Id;";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<LkPlantCompany>(sql, new { Id = id });
        }

        public async Task<IEnumerable<LkPlantCompany>> GetAllAsync()
        {
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<LkPlantCompany>(
                "SELECT * FROM b4.lk_plant_company;");
        }

        public async Task UpdateAsync(LkPlantCompany entity)
        {
            const string sql = @"
                UPDATE b4.lk_plant_company SET
                    companycode=@CompanyCode,
                    managementcompany=@ManagementCompany,
                    idcurrency=@IdCurrency,
                    company=@Company,
                    active=@Active,
                    iddivision=@IdDivision,
                    iddivisioncompany=@IdDivisionCompany,
                    idsubdivision=@IdSubdivision,
                    idcountry=@IdCountry,
                    location=@Location,
                    obs=@Obs,
                    regionalvalidatorpwd=@RegionalValidatorPwd,
                    region=@Region,
                    regionalvalidator=@RegionalValidator,
                    regionalvalidatoremail=@RegionalValidatorEmail,
                    contributorpwd=@ContributorPwd,
                    managementcompanybackup=@ManagementCompanyBackup,
                    regionbackup=@RegionBackup,
                    countrybackup=@CountryBackup,
                    idregvalidator=@IdRegValidator
                WHERE idcompany=@IdCompany;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            const string sql =
                "DELETE FROM b4.lk_plant_company WHERE idcompany=@Id;";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
