namespace B4.Tests.MySQLTests.LK;

using B4.Data.MySQL;
using Microsoft.Extensions.Configuration;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using B4.Data.MySQL.Repositories.LkRepositories;
using B4.Models.Entities.LkEntities;

public class PlantDivisionCompanyRepositoryTest : IDisposable
{
    private readonly MySQLDapperContext _context;
    private readonly PlantDivisionCompanyRepository _repository;

    // Lista que almacena los IDs que cada test inserta en la BD
    private readonly List<int> _insertedIds = new();

    public PlantDivisionCompanyRepositoryTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new MySQLDapperContext(config);
        _repository = new PlantDivisionCompanyRepository(_context);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = _context.CreateConnection();
        conn.Execute(
            "DELETE FROM LK_PLANT_DIVISION_COMPANY WHERE idDivisionCompany IN @ids",
            new { ids = _insertedIds });

        _insertedIds.Clear();
    }

    // -------------------------------------------------------------------
    // TESTS
    // -------------------------------------------------------------------

    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var entity = new LkPlantDivisionCompany
        {
            IdDivisionCompany = 99999,
            DivisionCompany = "ADD COMPANY"
        };

        _insertedIds.Add(entity.IdDivisionCompany);

        await _repository.AddAsync(entity);

        using var conn = _context.CreateConnection();
        var result = conn.QuerySingleOrDefault<LkPlantDivisionCompany>(
            "SELECT * FROM LK_PLANT_DIVISION_COMPANY WHERE idDivisionCompany = 99999");

        Assert.NotNull(result);
        Assert.Equal("ADD COMPANY", result.DivisionCompany);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        using var conn = _context.CreateConnection();
        conn.Execute(@"
            INSERT INTO LK_PLANT_DIVISION_COMPANY (idDivisionCompany, DivisionCompany)
            VALUES (99999, 'BYID COMPANY');
        ");
        _insertedIds.Add(99999);

        var result = await _repository.GetByIdAsync(99999);

        Assert.NotNull(result);
        Assert.Equal("BYID COMPANY", result.DivisionCompany);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        using var conn = _context.CreateConnection();
        conn.Execute(@"
            INSERT INTO LK_PLANT_DIVISION_COMPANY VALUES 
            (99999, 'A COMPANY'),
            (99998, 'B COMPANY');
        ");
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
            INSERT INTO LK_PLANT_DIVISION_COMPANY VALUES 
            (99999, 'ORIGINAL COMPANY');
        ");
        _insertedIds.Add(99999);

        var updated = new LkPlantDivisionCompany
        {
            IdDivisionCompany = 99999,
            DivisionCompany = "UPDATED COMPANY"
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<LkPlantDivisionCompany>(
            "SELECT * FROM LK_PLANT_DIVISION_COMPANY WHERE idDivisionCompany = 99999");

        Assert.Equal("UPDATED COMPANY", result.DivisionCompany);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        using var conn = _context.CreateConnection();
        conn.Execute(@"
            INSERT INTO LK_PLANT_DIVISION_COMPANY VALUES 
            (99999, 'DEL COMPANY');
        ");
        _insertedIds.Add(99999);

        await _repository.DeleteAsync(99999);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<LkPlantDivisionCompany>(
            "SELECT * FROM LK_PLANT_DIVISION_COMPANY WHERE idDivisionCompany = 99999");

        Assert.Null(result);
    }
}

