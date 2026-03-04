using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantCompanyRepository
        : BaseLkRepository<LkPlantCompany>, IPlantCompanyRepository
    {
        public PlantCompanyRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_plant_company", "idcompany") { }

        public override async Task AddAsync(LkPlantCompany entity)
        {
            PrepareForInsert(entity);

            // ✅ IMPORTANTE: la tabla tiene MUCHOS NOT NULL, hay que insertarlos.
            const string sql = @"
                INSERT INTO b4.lk_plant_company
                (
                    idcompany,
                    companycode,
                    managementcompany,
                    idcurrency,
                    company,
                    active,
                    iddivision,
                    iddivisioncompany,
                    idsubdivision,
                    idcountry,
                    location,
                    obs,
                    regionalvalidatorpwd,
                    region,
                    regionalvalidator,
                    regionalvalidatoremail,
                    contributorpwd,
                    managementcompanybackup,
                    regionbackup,
                    countrybackup,
                    idregvalidator,
                    createdat,
                    updatedat,
                    isactive
                )
                VALUES
                (
                    @IdCompany,
                    @CompanyCode,
                    @ManagementCompany,
                    @IdCurrency,
                    @Company,
                    @Active,
                    @IdDivision,
                    @IdDivisionCompany,
                    @IdSubdivision,
                    @IdCountry,
                    @Location,
                    @Obs,
                    @RegionalValidatorPwd,
                    @Region,
                    @RegionalValidator,
                    @RegionalValidatorEmail,
                    @ContributorPwd,
                    @ManagementCompanyBackup,
                    @RegionBackup,
                    @CountryBackup,
                    @IdRegValidator,
                    @CreatedAt,
                    @UpdatedAt,
                    @IsActive
                );";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkPlantCompany entity)
        {
            PrepareForUpdate(entity);

            const string sql = @"
                UPDATE b4.lk_plant_company
                SET
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
                    idregvalidator=@IdRegValidator,
                    updatedat=@UpdatedAt,
                    isactive=@IsActive
                WHERE idcompany=@IdCompany;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}