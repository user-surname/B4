namespace B4.Tests.MySQLTests.LK;

using B4.Data.MySQL;
using Microsoft.Extensions.Configuration;
using Dapper;
using B4.Data.MySQL.Repositories.LkRepositories;
using B4.Models.Entities.LkEntities;

public class PlantCompanyRepositoryTest : IDisposable
{
    private readonly MySQLDapperContext _context;
    private readonly PlantCompanyRepository _repository;
    private readonly List<int> _insertedIds = new();

    public PlantCompanyRepositoryTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new MySQLDapperContext(config);
        _repository = new PlantCompanyRepository(_context);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = _context.CreateConnection();
        conn.Execute(
            "DELETE FROM LK_PLANT_COMPANY WHERE IdCompany IN @ids",
            new { ids = _insertedIds });

        _insertedIds.Clear();
    }

    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var company = new LkPlantCompany
        {
            IdCompany = 99999,
            CompanyCode = "TEST",
            ManagementCompany = "MANAGEMENT",
            IdCurrency = 1,
            Company = "C1",
            Active = true,
            IdDivision = 10,
            IdDivisionCompany = 20,
            IdSubdivision = 30,
            IdCountry = 40,
            Location = "LOC",
            Obs = "OBS",
            RegionalValidatorPwd = "PWD",
            Region = "REGION",
            RegionalValidator = "VALIDATOR",
            RegionalValidatorEmail = "email@test.com",
            ContributorPwd = "CPWD",
            ManagementCompanyBackup = "BACKUP",
            RegionBackup = "RB",
            CountryBackup = "CB",
            IdRegValidator = 1
        };

        _insertedIds.Add(company.IdCompany);

        await _repository.AddAsync(company);

        using var conn = _context.CreateConnection();
        var result = conn.QuerySingleOrDefault<LkPlantCompany>(
            "SELECT * FROM LK_PLANT_COMPANY WHERE IdCompany = 99999");

        Assert.NotNull(result);
        Assert.Equal("TEST", result.CompanyCode);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        using var conn = _context.CreateConnection();
        conn.Execute(@"
            INSERT INTO LK_PLANT_COMPANY 
            (IdCompany, CompanyCode, ManagementCompany, IdCurrency, Company, Active, 
             IdDivision, IdDivisionCompany, IdSubdivision, IdCountry, Location)
            VALUES (99999, 'BYID', 'MAN', 1, 'C1', TRUE, 10, 20, 30, 40, 'LOC');");

        _insertedIds.Add(99999);

        var result = await _repository.GetByIdAsync(99999);

        Assert.NotNull(result);
        Assert.Equal("BYID", result.CompanyCode);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        using var conn = _context.CreateConnection();
        conn.Execute(@"
            INSERT INTO LK_PLANT_COMPANY (IdCompany, CompanyCode, ManagementCompany, IdCurrency, Company, Active, IdDivision, IdDivisionCompany, IdSubdivision, IdCountry, Location)
            VALUES 
            (99999, 'A', 'MAN1', 1, 'C1', TRUE, 10, 20, 30, 40, 'LOC1'),
            (99998, 'B', 'MAN2', 1, 'C2', TRUE, 11, 21, 31, 41, 'LOC2');");

        _insertedIds.AddRange(new[] { 99999, 99998 });

        var result = await _repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.True(result.Count() >= 2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyRecord()
    {
        using var conn = _context.CreateConnection();
        conn.Execute(@"
            INSERT INTO LK_PLANT_COMPANY (IdCompany, CompanyCode, ManagementCompany, IdCurrency, Company, Active, IdDivision, IdDivisionCompany, IdSubdivision, IdCountry, Location)
            VALUES (99999, 'ORIGINAL', 'MAN', 1, 'C1', TRUE, 10, 20, 30, 40, 'LOC');");

        _insertedIds.Add(99999);

        var updated = new LkPlantCompany
        {
            IdCompany = 99999,
            CompanyCode = "UPDATED",
            ManagementCompany = "MAN2",
            IdCurrency = 2,
            Company = "C2",
            Active = false,
            IdDivision = 11,
            IdDivisionCompany = 21,
            IdSubdivision = 31,
            IdCountry = 41,
            Location = "LOC2",
            Obs = null,
            RegionalValidatorPwd = null,
            Region = null,
            RegionalValidator = null,
            RegionalValidatorEmail = null,
            ContributorPwd = null,
            ManagementCompanyBackup = null,
            RegionBackup = null,
            CountryBackup = null,
            IdRegValidator = null
        };

        await _repository.UpdateAsync(updated);

        var result = conn.QuerySingle<LkPlantCompany>(
            "SELECT * FROM LK_PLANT_COMPANY WHERE IdCompany = 99999");

        Assert.Equal("UPDATED", result.CompanyCode);
        Assert.False(result.Active);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        using var conn = _context.CreateConnection();
        conn.Execute(@"
            INSERT INTO LK_PLANT_COMPANY (IdCompany, CompanyCode, ManagementCompany, IdCurrency, Company, Active, IdDivision, IdDivisionCompany, IdSubdivision, IdCountry, Location)
            VALUES (99999, 'DEL', 'MAN', 1, 'C1', TRUE, 10, 20, 30, 40, 'LOC');");

        _insertedIds.Add(99999);

        await _repository.DeleteAsync(99999);

        var result = conn.QuerySingleOrDefault<LkPlantCompany>(
            "SELECT * FROM LK_PLANT_COMPANY WHERE IdCompany = 99999");

        Assert.Null(result);
    }
}

