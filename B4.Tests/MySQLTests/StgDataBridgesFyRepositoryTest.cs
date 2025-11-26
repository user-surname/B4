namespace B4.Tests.MySQLTests;

using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class StgDataBridgesFyRepositoryTest : IDisposable
{
    private readonly DapperContext _context;
    private readonly StgDataBridgesFyRepository _repository;
    private readonly List<int> _insertedIds = new();

    public StgDataBridgesFyRepositoryTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new DapperContext(config);
        _repository = new StgDataBridgesFyRepository(_context);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0) return;

        using var conn = _context.CreateConnection();
        conn.Execute("DELETE FROM STG_DATA_BridgesFY WHERE Id IN @ids", new { ids = _insertedIds });
        _insertedIds.Clear();
    }

    // -------------------------------------------------
    // TEST: ADD
    // -------------------------------------------------
    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var entity = new StgDataBridgesFy
        {
            IdAPICarga = 1,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.Now,
            IdCompany = 1,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdCurrency = 1,
            IdEpigrafe = 1,
            FiscalYear = 100,
            Percentage = 10,
            Zero = 0,
            ZeroPercentage = 0,
            Absolute = 50,
            AbsolutePercentage = 5,
            VMixNew = 2,
            RawMaterial = 3,
            Scrap = 1,
            Economics = 4,
            CurrencyMix = 5,
            Performance = 6,
            ProtoTool = 7,
            Others = 8,
            Comments = "Test add"
        };

        await _repository.AddAsync(entity);

        int newId;
        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>("SELECT Id FROM STG_DATA_BridgesFY ORDER BY Id DESC LIMIT 1;");
        }

        _insertedIds.Add(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<StgDataBridgesFy>("SELECT * FROM STG_DATA_BridgesFY WHERE Id = @id", new { id = newId });

        Assert.NotNull(result);
        Assert.Equal(100, result.FiscalYear);
    }

    // -------------------------------------------------
    // TEST: GET BY ID
    // -------------------------------------------------
    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        int newId;
        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>(@"
                INSERT INTO STG_DATA_BridgesFY 
                (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe, FiscalYear, Percentage, Zero, ZeroPercentage, Absolute, AbsolutePercentage, VMixNew, RawMaterial, Scrap, Economics, CurrencyMix, Performance, ProtoTool, Others, Comments)
                VALUES
                (1, '550e8400-e29b-11d4-a716-446655440001', NOW(), 1, 2025, 1, 1, 1, 1, 200, 20, 0, 0, 100, 10, 2, 3, 1, 4, 5, 6, 7, 8, 'Test byId');
                SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        var result = await _repository.GetByIdAsync(newId);

        Assert.NotNull(result);
        Assert.Equal(200, result.FiscalYear);
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
            id1 = conn.ExecuteScalar<int>(@"
                INSERT INTO STG_DATA_BridgesFY (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe, FiscalYear, Percentage, Zero, ZeroPercentage, Absolute, AbsolutePercentage, VMixNew, RawMaterial, Scrap, Economics, CurrencyMix, Performance, ProtoTool, Others, Comments)
                VALUES (1, UUID(), NOW(), 1, 2025, 1,1,1,1,300,30,0,0,150,15,2,3,1,4,5,6,7,8,'Test A'); SELECT LAST_INSERT_ID();
            ");

            id2 = conn.ExecuteScalar<int>(@"
                INSERT INTO STG_DATA_BridgesFY (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe, FiscalYear, Percentage, Zero, ZeroPercentage, Absolute, AbsolutePercentage, VMixNew, RawMaterial, Scrap, Economics, CurrencyMix, Performance, ProtoTool, Others, Comments)
                VALUES (2, UUID(), NOW(), 1, 2025, 1,1,1,1,400,40,0,0,200,20,2,3,1,4,5,6,7,8,'Test B'); SELECT LAST_INSERT_ID();
            ");
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
        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>(@"
                INSERT INTO STG_DATA_BridgesFY (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe, FiscalYear, Percentage, Zero, ZeroPercentage, Absolute, AbsolutePercentage, VMixNew, RawMaterial, Scrap, Economics, CurrencyMix, Performance, ProtoTool, Others, Comments)
                VALUES (1, UUID(), NOW(), 1, 2025, 1,1,1,1,500,50,0,0,250,25,2,3,1,4,5,6,7,8,'Test update'); SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        var updated = new StgDataBridgesFy
        {
            Id = newId,
            IdAPICarga = 2,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.Now,
            IdCompany = 1,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdCurrency = 1,
            IdEpigrafe = 1,
            FiscalYear = 600,
            Percentage = 60,
            Zero = 0,
            ZeroPercentage = 0,
            Absolute = 300,
            AbsolutePercentage = 30,
            VMixNew = 3,
            RawMaterial = 4,
            Scrap = 2,
            Economics = 5,
            CurrencyMix = 6,
            Performance = 7,
            ProtoTool = 8,
            Others = 9,
            Comments = "Updated test"
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<StgDataBridgesFy>("SELECT * FROM STG_DATA_BridgesFY WHERE Id = @id", new { id = newId });

        Assert.Equal(600, result.FiscalYear);
    }

    // -------------------------------------------------
    // TEST: DELETE
    // -------------------------------------------------
    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        int newId;
        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>(@"
                INSERT INTO STG_DATA_BridgesFY (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe, FiscalYear, Percentage, Zero, ZeroPercentage, Absolute, AbsolutePercentage, VMixNew, RawMaterial, Scrap, Economics, CurrencyMix, Performance, ProtoTool, Others, Comments)
                VALUES (1, UUID(), NOW(), 1, 2025, 1,1,1,1,700,70,0,0,350,35,3,4,2,5,6,7,8,9,'Test delete'); SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        await _repository.DeleteAsync(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<StgDataBridgesFy>("SELECT * FROM STG_DATA_BridgesFY WHERE Id = @id", new { id = newId });

        Assert.Null(result);
    }
}

