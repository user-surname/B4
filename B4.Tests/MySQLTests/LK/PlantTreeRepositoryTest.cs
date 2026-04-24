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

public class PlantTreeRepositoryTest : IDisposable
{
    private readonly IDbConnectionFactory _context;
    private readonly IDataQueryProvider _queryProvider;
    private readonly LkPlantTreeRepository _repository;
    private readonly List<int> _insertedIds = new();

    public PlantTreeRepositoryTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = config.GetConnectionString("MySQLConnectionB4Data")
            ?? throw new InvalidOperationException("Connection string 'MySQLConnectionB4Data' not found.");

        var dataFactoryOptions = new DataFactoryOptions
        {
            Provider = "MySQL",
            MySqlConnectionString = connectionString
        };

        var options = Options.Create(dataFactoryOptions);

        _context = new DbConnectionFactory(options);
        _queryProvider = new DataQueryProvider(options);
        _repository = new LkPlantTreeRepository(_context, _queryProvider);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0) return;

        using var conn = _context.CreateConnection();
        conn.Execute(
            "DELETE FROM LK_PLANT_TREE WHERE idTree IN @ids",
            new { ids = _insertedIds });

        _insertedIds.Clear();
    }

    // -------------------------------------------------
    // TESTS
    // -------------------------------------------------

    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var tree = new LkPlantTree
        {
            IdTree = 99999,
            IdDivision = 1,
            IdDivisionCompany = 2,
            IdSubdivision = 3,
            IdCountry = 4
        };

        _insertedIds.Add(tree.IdTree);

        await _repository.AddAsync(tree);

        using var conn = _context.CreateConnection();
        var result = conn.QuerySingleOrDefault<LkPlantTree>(
            "SELECT * FROM LK_PLANT_TREE WHERE idTree = 99999");

        Assert.NotNull(result);
        Assert.Equal(2, result.IdDivisionCompany);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_TREE (idTree, idDivision, idDivisionCompany, idSubdivision, idCountry)
                VALUES (99999, 1, 2, 3, 4);");
        }

        _insertedIds.Add(99999);

        var result = await _repository.GetByIdAsync(99999);

        Assert.NotNull(result);
        Assert.Equal(3, result.IdSubdivision);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_TREE (idTree, idDivision, idDivisionCompany, idSubdivision, idCountry) VALUES 
                (99999, 1, 2, 3, 4),
                (99998, 5, 6, 7, 8);");
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
                INSERT INTO LK_PLANT_TREE (idTree, idDivision, idDivisionCompany, idSubdivision, idCountry) VALUES 
                (99999, 1, 2, 3, 4);");
        }

        _insertedIds.Add(99999);

        var updated = new LkPlantTree
        {
            IdTree = 99999,
            IdDivision = 10,
            IdDivisionCompany = 20,
            IdSubdivision = 30,
            IdCountry = 40
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<LkPlantTree>(
            "SELECT * FROM LK_PLANT_TREE WHERE idTree = 99999");

        Assert.Equal(20, result.IdDivisionCompany);
        Assert.Equal(40, result.IdCountry);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_TREE (idTree, idDivision, idDivisionCompany, idSubdivision, idCountry) VALUES 
                (99999, 1, 2, 3, 4);");
        }

        _insertedIds.Add(99999);

        await _repository.DeleteAsync(99999);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<LkPlantTree>(
            "SELECT * FROM LK_PLANT_TREE WHERE idTree = 99999");

        Assert.Null(result);
    }
}

