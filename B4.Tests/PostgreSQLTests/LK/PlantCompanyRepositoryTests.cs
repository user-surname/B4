using Xunit;
using Npgsql;
using Dapper;
using B4.Data.PostgreSQL;
using B4.Data.PostgreSQL.Repositories;
using B4.Models.Entities.LkEntities;

public class PlantCompanyRepositoryTests : IDisposable
{
    private readonly PlantCompanyRepository _repo;
    private readonly List<int> _ids = new();

    public PlantCompanyRepositoryTests()
    {
        _repo = new PlantCompanyRepository(new DapperContext(TestConfig.Configuration));
    }

    public void Dispose()
    {
        using var conn = new NpgsqlConnection(TestConfig.Conn);
        conn.Execute("DELETE FROM b4.lk_plant_company WHERE idcompany = ANY(@Ids)",
            new { Ids = _ids.ToArray() });
    }

    [Fact]
    public async Task Insert_And_GetById_Should_Work()
    {
        var e = CreateSample();
        await _repo.AddAsync(e);
        _ids.Add(e.IdCompany);

        var result = await _repo.GetByIdAsync(e.IdCompany);

        Assert.NotNull(result);
        Assert.Equal(e.CompanyCode, result!.CompanyCode);
    }

    [Fact]
    public async Task Update_Should_Work()
    {
        var e = CreateSample();
        await _repo.AddAsync(e);
        _ids.Add(e.IdCompany);

        e.Company = "UPD";

        await _repo.UpdateAsync(e);

        var result = await _repo.GetByIdAsync(e.IdCompany);

        Assert.Equal("UPD", result!.Company);
    }

    [Fact]
    public async Task Delete_Should_Work()
    {
        var e = CreateSample();
        await _repo.AddAsync(e);
        _ids.Add(e.IdCompany);

        await _repo.DeleteAsync(e.IdCompany);

        Assert.Null(await _repo.GetByIdAsync(e.IdCompany));
    }

    private LkPlantCompany CreateSample()
    {
        return new LkPlantCompany
        {
            IdCompany = new Random().Next(1_000_000, 9_999_999),
            CompanyCode = "TESTC",
            ManagementCompany = "MgmtCo",
            IdCurrency = 1,
            Company = "TC",
            Active = true,
            IdDivision = 1,
            IdDivisionCompany = 2,
            IdSubdivision = 3,
            IdCountry = 4,
            Location = "Somewhere",
            Obs = "Testing",
            RegionalValidatorPwd = "pwd",
            Region = "EU",
            RegionalValidator = "Validator",
            RegionalValidatorEmail = "valid@test.com",
            ContributorPwd = "123",
            ManagementCompanyBackup = "BCK",
            RegionBackup = "RB",
            CountryBackup = "CB",
            IdRegValidator = 20
        };
    }
}
