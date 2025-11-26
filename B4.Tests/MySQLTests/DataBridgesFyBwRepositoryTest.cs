namespace B4.Tests.MySQLTests;

using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;
using System;

public class DataBridgesFyBwRepositoryTest : IDisposable
{
    private readonly DapperContext _context;
    private readonly DataBridgesFyBwRepository _repository;

    private readonly List<int> _insertedIds = new();

    public DataBridgesFyBwRepositoryTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new DapperContext(config);
        _repository = new DataBridgesFyBwRepository(_context);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = _context.CreateConnection();
        conn.Execute("DELETE FROM DATA_BridgesFY_BW WHERE Id IN @ids", new { ids = _insertedIds });
        _insertedIds.Clear();
    }

    // -------------------------------------------------
    // TEST: ADD
    // -------------------------------------------------
    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var entity = new DataBridgesFyBw
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
            FiscalYear = 100,
            Percentage = 10,
            Zero = 0,
            ZeroPercentage = 0,
            Absolute = 50,
            AbsolutePercentage = 5,
            VMixNew = 1,
            RawMaterial = 2,
            Scrap = 3,
            Economics = 4,
            CurrencyMix = 5,
            Performance = 6,
            ProtoTool = 7,
            Others = 8,
            Comments = "Test",
            IdCarga = 1,
            IdCargaSTGBW = 2,
            IdHoja = 1
        };

        await _repository.AddAsync(entity);

        int newId;
        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>("SELECT Id FROM DATA_BridgesFY_BW ORDER BY Id DESC LIMIT 1;");
        }

        _insertedIds.Add(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<DataBridgesFyBw>("SELECT * FROM DATA_BridgesFY_BW WHERE Id = @id", new { id = newId });

        Assert.NotNull(result);
        Assert.Equal(entity.IdCompany, result.IdCompany);
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
                INSERT INTO DATA_BridgesFY_BW
                (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                 FiscalYear, Percentage, Zero, ZeroPercentage, Absolute, AbsolutePercentage, VMixNew, RawMaterial, Scrap,
                 Economics, CurrencyMix, Performance, ProtoTool, Others, Comments, idCarga, idCargaSTGBW, idHoja)
                VALUES
                (1, @Guid, NOW(), 1, 2025, 1, 1, 1, 1, 100, 10, 0, 0, 50, 5, 1, 2, 3, 4, 5, 6, 7, 8, 'BYID', 1, 2, 1);
                SELECT LAST_INSERT_ID();
            ", new { Guid = guid });
        }

        _insertedIds.Add(newId);

        var result = await _repository.GetByIdAsync(newId);

        Assert.NotNull(result);
        Assert.Equal("BYID", result.Comments);
    }

    // -------------------------------------------------
    // TEST: GET ALL
    // -------------------------------------------------
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        int id1, id2;
        Guid guid1 = Guid.NewGuid();
        Guid guid2 = Guid.NewGuid();

        using (var conn = _context.CreateConnection())
        {
            id1 = conn.ExecuteScalar<int>(@"
                INSERT INTO DATA_BridgesFY_BW
                (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                 FiscalYear, Percentage, Zero, ZeroPercentage, Absolute, AbsolutePercentage, VMixNew, RawMaterial, Scrap,
                 Economics, CurrencyMix, Performance, ProtoTool, Others, Comments, idCarga, idCargaSTGBW, idHoja)
                VALUES
                (1, @Guid1, NOW(), 1, 2025, 1, 1, 1, 1, 100, 10, 0, 0, 50, 5, 1, 2, 3, 4, 5, 6, 7, 8, 'A', 1, 2, 1);
                SELECT LAST_INSERT_ID();
            ", new { Guid1 = guid1 });

            id2 = conn.ExecuteScalar<int>(@"
                INSERT INTO DATA_BridgesFY_BW
                (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                 FiscalYear, Percentage, Zero, ZeroPercentage, Absolute, AbsolutePercentage, VMixNew, RawMaterial, Scrap,
                 Economics, CurrencyMix, Performance, ProtoTool, Others, Comments, idCarga, idCargaSTGBW, idHoja)
                VALUES
                (1, @Guid2, NOW(), 1, 2025, 1, 1, 1, 1, 100, 10, 0, 0, 50, 5, 1, 2, 3, 4, 5, 6, 7, 8, 'B', 1, 2, 1);
                SELECT LAST_INSERT_ID();
            ", new { Guid2 = guid2 });
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
                INSERT INTO DATA_BridgesFY_BW
                (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                 FiscalYear, Percentage, Zero, ZeroPercentage, Absolute, AbsolutePercentage, VMixNew, RawMaterial, Scrap,
                 Economics, CurrencyMix, Performance, ProtoTool, Others, Comments, idCarga, idCargaSTGBW, idHoja)
                VALUES
                (1, @Guid, NOW(), 1, 2025, 1, 1, 1, 1, 100, 10, 0, 0, 50, 5, 1, 2, 3, 4, 5, 6, 7, 8, 'ORIG', 1, 2, 1);
                SELECT LAST_INSERT_ID();
            ", new { Guid = guid });
        }

        _insertedIds.Add(newId);

        var updated = new DataBridgesFyBw
        {
            Id = newId,
            IdAPICarga = 2,
            GuidCarga = guid,
            FechaUltModif = DateTime.UtcNow,
            IdCompany = 1,
            Ejercicio = 2026,
            IdCiclo = 2,
            IdFase = 2,
            IdCurrency = 2,
            IdEpigrafe = 2,
            FiscalYear = 200,
            Percentage = 20,
            Zero = 0,
            ZeroPercentage = 0,
            Absolute = 100,
            AbsolutePercentage = 10,
            VMixNew = 2,
            RawMaterial = 3,
            Scrap = 4,
            Economics = 5,
            CurrencyMix = 6,
            Performance = 7,
            ProtoTool = 8,
            Others = 9,
            Comments = "UPDATED",
            IdCarga = 2,
            IdCargaSTGBW = 3,
            IdHoja = 2
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<DataBridgesFyBw>("SELECT * FROM DATA_BridgesFY_BW WHERE Id = @id", new { id = newId });

        Assert.Equal("UPDATED", result.Comments);
        Assert.Equal(200, result.FiscalYear);
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
                INSERT INTO DATA_BridgesFY_BW
                (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                 FiscalYear, Percentage, Zero, ZeroPercentage, Absolute, AbsolutePercentage, VMixNew, RawMaterial, Scrap,
                 Economics, CurrencyMix, Performance, ProtoTool, Others, Comments, idCarga, idCargaSTGBW, idHoja)
                VALUES
                (1, @Guid, NOW(), 1, 2025, 1, 1, 1, 1, 100, 10, 0, 0, 50, 5, 1, 2, 3, 4, 5, 6, 7, 8, 'DEL', 1, 2, 1);
                SELECT LAST_INSERT_ID();
            ", new { Guid = guid });
        }

        _insertedIds.Add(newId);

        await _repository.DeleteAsync(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<DataBridgesFyBw>("SELECT * FROM DATA_BridgesFY_BW WHERE Id = @id", new { id = newId });

        Assert.Null(result);
    }
}
