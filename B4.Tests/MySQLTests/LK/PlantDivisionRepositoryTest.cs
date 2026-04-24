using B4.Data.DataFactory.Configuration;
using B4.Data.DataFactory.Connections;
using B4.Data.DataFactory.Providers;
using Microsoft.Extensions.Options;
﻿namespace B4.Tests.MySQLTests.LK;

using B4.Data.DataFactory;
using Microsoft.Extensions.Configuration;
using Dapper;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using B4.Data.DataFactory.Repositories;
using B4.Models.Entities.LkEntities;

public class PlantDivisionRepositoryTest : IDisposable
{
    private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
    private readonly PlantDivisionRepository _repository;
    private readonly List<int> _insertedIds = new();

    public PlantDivisionRepositoryTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = config.GetConnectionString("MySQLConnectionB4Data")
            ?? throw new InvalidOperationException("Connection string 'MySQLConnectionB4Data' not found.");

        var dataFactoryOptions = new DataFactoryOptions
        {
            Provider = "MySQL",
            ConnectionString = connectionString
        };

        var options = Options.Create(dataFactoryOptions);

        _context = new DbConnectionFactory(options);
        _queryProvider = new DataQueryProvider(options);
        _repository = new PlantDivisionRepository(_context, _queryProvider);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0) return;

        using var conn = _context.CreateConnection();
        conn.Execute(
            "DELETE FROM LK_PLANT_DIVISION WHERE idDivision IN @ids",
            new { ids = _insertedIds });

        _insertedIds.Clear();
    }

    // -------------------------------------------------
    // TESTS
    // -------------------------------------------------

    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var division = new LkPlantDivision
        {
            IdDivision = 99999,
            Division = "TestDivision"
        };

        _insertedIds.Add(division.IdDivision);

        await _repository.AddAsync(division);

        using var conn = _context.CreateConnection();
        var result = conn.QuerySingleOrDefault<LkPlantDivision>(
            "SELECT * FROM LK_PLANT_DIVISION WHERE idDivision = 99999");

        Assert.NotNull(result);
        Assert.Equal("TestDivision", result.Division);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_DIVISION (idDivision, Division)
                VALUES (99999, 'ByIdDivision');");
        }

        _insertedIds.Add(99999);

        var result = await _repository.GetByIdAsync(99999);

        Assert.NotNull(result);
        Assert.Equal("ByIdDivision", result.Division);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_DIVISION (idDivision, Division) VALUES
                (99999, 'DivisionA'),
                (99998, 'DivisionB');");
        }

        _insertedIds.AddRange(new[] { 99999, 99998 });

        var result = await _repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.True(result.Count() >= 2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_DIVISION (idDivision, Division)
                VALUES (99999, 'OriginalDivision');");
        }

        _insertedIds.Add(99999);

        var updated = new LkPlantDivision
        {
            IdDivision = 99999,
            Division = "UpdatedDivision"
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<LkPlantDivision>(
            "SELECT * FROM LK_PLANT_DIVISION WHERE idDivision = 99999");

        Assert.Equal("UpdatedDivision", result.Division);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_DIVISION (idDivision, Division)
                VALUES (99999, 'ToDeleteDivision');");
        }

        _insertedIds.Add(99999);

        await _repository.DeleteAsync(99999);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<LkPlantDivision>(
            "SELECT * FROM LK_PLANT_DIVISION WHERE idDivision = 99999");

        Assert.Null(result);
    }
}

