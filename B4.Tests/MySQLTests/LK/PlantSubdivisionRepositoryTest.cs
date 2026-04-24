using B4.Data.DataFactory.Configuration;
using B4.Data.DataFactory.Connections;
using B4.Data.DataFactory.Providers;
using Microsoft.Extensions.Options;
﻿namespace B4.Tests.MySQLTests.LK;

using B4.Data.DataFactory;
using Microsoft.Extensions.Configuration;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using B4.Data.DataFactory.Repositories;
using B4.Models.Entities.LkEntities;

public class PlantSubdivisionRepositoryTest : IDisposable
{
    private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
    private readonly PlantSubdivisionRepository _repository;

    // Lista que almacena los IDs que cada test inserta en la BD
    private readonly List<int> _insertedIds = new();

    public PlantSubdivisionRepositoryTest()
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
        _repository = new PlantSubdivisionRepository(_context, _queryProvider);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = _context.CreateConnection();
        conn.Execute(
            "DELETE FROM LK_PLANT_SUBDIVISION WHERE idSubdivision IN @ids",
            new { ids = _insertedIds });
        _insertedIds.Clear();
    }

    // -------------------------------------------------------------------
    // TESTS
    // -------------------------------------------------------------------

    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var subdivision = new LkPlantSubdivision
        {
            IdSubdivision = 99999,
            Subdivision = "TEST_ADD"
        };

        _insertedIds.Add(subdivision.IdSubdivision);

        await _repository.AddAsync(subdivision);

        using var conn = _context.CreateConnection();
        var result = conn.QuerySingleOrDefault<LkPlantSubdivision>(
            "SELECT * FROM LK_PLANT_SUBDIVISION WHERE idSubdivision = 99999");

        Assert.NotNull(result);
        Assert.Equal("TEST_ADD", result.Subdivision);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_SUBDIVISION (idSubdivision, Subdivision)
                VALUES (99999, 'BYID');
            ");
        }

        _insertedIds.Add(99999);

        var result = await _repository.GetByIdAsync(99999);

        Assert.NotNull(result);
        Assert.Equal("BYID", result.Subdivision);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_SUBDIVISION (idSubdivision, Subdivision) VALUES
                (99999, 'A'),
                (99998, 'B');
            ");
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
                INSERT INTO LK_PLANT_SUBDIVISION (idSubdivision, Subdivision)
                VALUES (99999, 'ORIGINAL');
            ");
        }

        _insertedIds.Add(99999);

        var updated = new LkPlantSubdivision
        {
            IdSubdivision = 99999,
            Subdivision = "UPDATED"
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<LkPlantSubdivision>(
            "SELECT * FROM LK_PLANT_SUBDIVISION WHERE idSubdivision = 99999");

        Assert.Equal("UPDATED", result.Subdivision);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_SUBDIVISION (idSubdivision, Subdivision)
                VALUES (99999, 'DEL');
            ");
        }

        _insertedIds.Add(99999);

        await _repository.DeleteAsync(99999);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<LkPlantSubdivision>(
            "SELECT * FROM LK_PLANT_SUBDIVISION WHERE idSubdivision = 99999");

        Assert.Null(result);
    }
}

