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

public class PlantCountryRepositoryTest : IDisposable
{
    private readonly IDbConnectionFactory _context;
    private readonly IDataQueryProvider _queryProvider;
    private readonly PlantCountryRepository _repository;
    private readonly List<int> _insertedIds = new();

    public PlantCountryRepositoryTest()
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
        _repository = new PlantCountryRepository(_context, _queryProvider);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0) return;

        using var conn = _context.CreateConnection();
        conn.Execute(
            "DELETE FROM LK_PLANT_COUNTRY WHERE idCountry IN @ids",
            new { ids = _insertedIds });

        _insertedIds.Clear();
    }

    // -------------------------------------------------
    // TESTS
    // -------------------------------------------------

    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var country = new LkPlantCountry
        {
            IdCountry = 99999,
            Country = "Testland"
        };

        _insertedIds.Add(country.IdCountry);

        await _repository.AddAsync(country);

        using var conn = _context.CreateConnection();
        var result = conn.QuerySingleOrDefault<LkPlantCountry>(
            "SELECT * FROM LK_PLANT_COUNTRY WHERE idCountry = 99999");

        Assert.NotNull(result);
        Assert.Equal("Testland", result.Country);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_COUNTRY (idCountry, Country)
                VALUES (99999, 'ByIdLand');");
        }

        _insertedIds.Add(99999);

        var result = await _repository.GetByIdAsync(99999);

        Assert.NotNull(result);
        Assert.Equal("ByIdLand", result.Country);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_COUNTRY (idCountry, Country) VALUES
                (99999, 'Aland'),
                (99998, 'Bland');");
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
                INSERT INTO LK_PLANT_COUNTRY (idCountry, Country)
                VALUES (99999, 'OriginalLand');");
        }

        _insertedIds.Add(99999);

        var updated = new LkPlantCountry
        {
            IdCountry = 99999,
            Country = "UpdatedLand"
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<LkPlantCountry>(
            "SELECT * FROM LK_PLANT_COUNTRY WHERE idCountry = 99999");

        Assert.Equal("UpdatedLand", result.Country);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_COUNTRY (idCountry, Country)
                VALUES (99999, 'ToDeleteLand');");
        }

        _insertedIds.Add(99999);

        await _repository.DeleteAsync(99999);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<LkPlantCountry>(
            "SELECT * FROM LK_PLANT_COUNTRY WHERE idCountry = 99999");

        Assert.Null(result);
    }
}

