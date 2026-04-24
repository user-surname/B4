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

public class PlantCurrencyRepositoryTest : IDisposable
{
    private readonly IDbConnectionFactory _context;
    private readonly IDataQueryProvider _queryProvider;
    private readonly PlantCurrencyRepository _repository;
    private readonly List<int> _insertedIds = new();

    public PlantCurrencyRepositoryTest()
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
        _repository = new PlantCurrencyRepository(_context, _queryProvider);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0) return;

        using var conn = _context.CreateConnection();
        conn.Execute(
            "DELETE FROM LK_PLANT_CURRENCY WHERE idCurrency IN @ids",
            new { ids = _insertedIds });

        _insertedIds.Clear();
    }

    // -------------------------------------------------
    // TESTS
    // -------------------------------------------------

    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var currency = new LkPlantCurrency
        {
            IdCurrency = 99999,
            Currency = "TestCoin",
            CurrencyAlias = "TC"
        };

        _insertedIds.Add(currency.IdCurrency);

        await _repository.AddAsync(currency);

        using var conn = _context.CreateConnection();
        var result = conn.QuerySingleOrDefault<LkPlantCurrency>(
            "SELECT * FROM LK_PLANT_CURRENCY WHERE idCurrency = 99999");

        Assert.NotNull(result);
        Assert.Equal("TestCoin", result.Currency);
        Assert.Equal("TC", result.CurrencyAlias);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_CURRENCY (idCurrency, Currency, CurrencyAlias)
                VALUES (99999, 'ByIdCoin', 'BIC');");
        }

        _insertedIds.Add(99999);

        var result = await _repository.GetByIdAsync(99999);

        Assert.NotNull(result);
        Assert.Equal("ByIdCoin", result.Currency);
        Assert.Equal("BIC", result.CurrencyAlias);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_CURRENCY (idCurrency, Currency, CurrencyAlias) VALUES
                (99999, 'CoinA', 'CA'),
                (99998, 'CoinB', 'CB');");
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
                INSERT INTO LK_PLANT_CURRENCY (idCurrency, Currency, CurrencyAlias)
                VALUES (99999, 'OriginalCoin', 'OC');");
        }

        _insertedIds.Add(99999);

        var updated = new LkPlantCurrency
        {
            IdCurrency = 99999,
            Currency = "UpdatedCoin",
            CurrencyAlias = "UC"
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<LkPlantCurrency>(
            "SELECT * FROM LK_PLANT_CURRENCY WHERE idCurrency = 99999");

        Assert.Equal("UpdatedCoin", result.Currency);
        Assert.Equal("UC", result.CurrencyAlias);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_CURRENCY (idCurrency, Currency, CurrencyAlias)
                VALUES (99999, 'ToDeleteCoin', 'TDC');");
        }

        _insertedIds.Add(99999);

        await _repository.DeleteAsync(99999);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<LkPlantCurrency>(
            "SELECT * FROM LK_PLANT_CURRENCY WHERE idCurrency = 99999");

        Assert.Null(result);
    }
}

