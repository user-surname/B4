namespace B4.Tests.MySQLTests;

using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class DataBridgesFyBwEurRepositoryTest : IDisposable
{
    private readonly MySQLDapperContext _context;
    private readonly DataBridgesFyBwEurRepository _repository;

    private readonly List<int> _insertedIds = new();

    public DataBridgesFyBwEurRepositoryTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new MySQLDapperContext(config);
        _repository = new DataBridgesFyBwEurRepository(_context);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0) return;

        using var conn = _context.CreateConnection();
        conn.Execute("DELETE FROM DATA_BRIDGESFY_BW_EUR WHERE Id IN @ids", new { ids = _insertedIds });
        _insertedIds.Clear();
    }

    // -------------------------------------------------
    // TEST: ADD
    // -------------------------------------------------
    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var record = new DataBridgesFyBwEur
        {
            IdAPICarga = 1,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.UtcNow,
            IdCompany = 1,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdCurrency = 1,
            IdEpigrafe = 1,
            Actuals = 100,
            PctActuals = 50,
            Budget = 200,
            PctBudget = 75,
            Variance = 10,
            Volume = 5,
            InventoryChange = 2,
            Mix = 1,
            New = 3,
            Economics = 4,
            QuickSavings = 0,
            CurrencyMix = 1,
            ExchangeRate = 1.1m,
            RawMaterial = 10,
            Scrap = 5,
            IndustrialPerformance = 2,
            ProtoTooling = 3,
            Other = 0,
            Check = 1,
            Comments = "Test ADD",
            IdCarga = null,
            IdCargaSTGBW = null,
            IdHoja = null
        };

        await _repository.AddAsync(record);

        int newId;
        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>("SELECT Id FROM DATA_BRIDGESFY_BW_EUR ORDER BY Id DESC LIMIT 1;");
        }

        _insertedIds.Add(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<DataBridgesFyBwEur>("SELECT * FROM DATA_BRIDGESFY_BW_EUR WHERE Id = @id", new { id = newId });

        Assert.NotNull(result);
        Assert.Equal(record.Actuals, result.Actuals);
    }

    // -------------------------------------------------
    // TEST: GET BY ID
    // -------------------------------------------------
    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        int newId;
        Guid guid = Guid.NewGuid();

        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>(@"
                INSERT INTO DATA_BRIDGESFY_BW_EUR 
                (idAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                 Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, `New`, Economics,
                 QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap, IndustrialPerformance, ProtoTooling, Other,
                 `Check`, Comments)
                VALUES
                (1, @guid, NOW(), 1, 2025, 1,1,1,1,100,50,200,75,10,5,2,1,3,4,0,1,1.1,10,5,2,3,0,1,'Test BYID');
                SELECT LAST_INSERT_ID();", new { guid });
        }

        _insertedIds.Add(newId);

        var result = await _repository.GetByIdAsync(newId);

        Assert.NotNull(result);
        Assert.Equal(100, result.Actuals);
    }

    // -------------------------------------------------
    // TEST: GET ALL
    // -------------------------------------------------
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        int id1, id2;

        using (var conn = _context.CreateConnection())
        {
            id1 = conn.ExecuteScalar<int>("INSERT INTO DATA_BRIDGESFY_BW_EUR (idAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe, Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, `New`, Economics, QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap, IndustrialPerformance, ProtoTooling, Other, `Check`, Comments) VALUES (1, UUID(), NOW(),1,2025,1,1,1,1,100,50,200,75,10,5,2,1,3,4,0,1,1.1,10,5,2,3,0,1,'Test A'); SELECT LAST_INSERT_ID();");
            id2 = conn.ExecuteScalar<int>("INSERT INTO DATA_BRIDGESFY_BW_EUR (idAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe, Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, `New`, Economics, QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap, IndustrialPerformance, ProtoTooling, Other, `Check`, Comments) VALUES (1, UUID(), NOW(),1,2025,1,1,1,1,200,60,300,80,20,10,5,2,4,5,1,2,1.2,20,10,4,5,1,2,'Test B'); SELECT LAST_INSERT_ID();");
        }

        _insertedIds.AddRange(new[] { id1, id2 });

        var result = await _repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.True(result.Count() >= 2);
    }

    // -------------------------------------------------
    // TEST: UPDATE
    // -------------------------------------------------
    [Fact]
    public async Task UpdateAsync_ShouldModifyRecord()
    {
        int newId;
        Guid guid = Guid.NewGuid();

        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>(@"
                INSERT INTO DATA_BRIDGESFY_BW_EUR 
                (idAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                 Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, `New`, Economics,
                 QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap, IndustrialPerformance, ProtoTooling, Other,
                 `Check`, Comments)
                VALUES
                (1, @guid, NOW(),1,2025,1,1,1,1,100,50,200,75,10,5,2,1,3,4,0,1,1.1,10,5,2,3,0,1,'Test ORIG');
                SELECT LAST_INSERT_ID();", new { guid });
        }

        _insertedIds.Add(newId);

        var updated = new DataBridgesFyBwEur
        {
            Id = newId,
            IdAPICarga = 1,
            GuidCarga = guid,
            FechaUltModif = DateTime.UtcNow,
            IdCompany = 1,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdCurrency = 1,
            IdEpigrafe = 1,
            Actuals = 999,
            PctActuals = 99,
            Budget = 888,
            PctBudget = 88,
            Variance = 77,
            Volume = 66,
            InventoryChange = 55,
            Mix = 44,
            New = 33,
            Economics = 22,
            QuickSavings = 11,
            CurrencyMix = 10,
            ExchangeRate = 1.23m,
            RawMaterial = 9,
            Scrap = 8,
            IndustrialPerformance = 7,
            ProtoTooling = 6,
            Other = 5,
            Check = 4,
            Comments = "Test UPDATED"
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<DataBridgesFyBwEur>("SELECT * FROM DATA_BRIDGESFY_BW_EUR WHERE Id = @id", new { id = newId });

        Assert.Equal(999, result.Actuals);
        Assert.Equal("Test UPDATED", result.Comments);
    }

    // -------------------------------------------------
    // TEST: DELETE
    // -------------------------------------------------
    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        int newId;
        Guid guid = Guid.NewGuid();

        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>(@"
                INSERT INTO DATA_BRIDGESFY_BW_EUR 
                (idAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                 Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, `New`, Economics,
                 QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap, IndustrialPerformance, ProtoTooling, Other,
                 `Check`, Comments)
                VALUES
                (1, @guid, NOW(),1,2025,1,1,1,1,100,50,200,75,10,5,2,1,3,4,0,1,1.1,10,5,2,3,0,1,'Test DEL');
                SELECT LAST_INSERT_ID();", new { guid });
        }

        _insertedIds.Add(newId);

        await _repository.DeleteAsync(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<DataBridgesFyBwEur>("SELECT * FROM DATA_BRIDGESFY_BW_EUR WHERE Id = @id", new { id = newId });

        Assert.Null(result);
    }
}
