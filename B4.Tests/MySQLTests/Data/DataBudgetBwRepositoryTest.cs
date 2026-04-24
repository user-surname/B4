using B4.Data.DataFactory.Configuration;
using B4.Data.DataFactory.Connections;
using B4.Data.DataFactory.Providers;
using Microsoft.Extensions.Options;
﻿namespace B4.Tests.MySQLTests.Data;

using B4.Data.DataFactory;
using Microsoft.Extensions.Configuration;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using B4.Data.DataFactory.Repositories;
using B4.Models.Entities.DataEntities;

public class DataBudgetBwRepositoryTest : IDisposable
{
    private readonly IDbConnectionFactory _context;
    private readonly IDataQueryProvider _queryProvider;
    private readonly DataBudgetBwRepository _repository;
    private readonly List<int> _insertedIds = new();

    public DataBudgetBwRepositoryTest()
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
        _repository = new DataBudgetBwRepository(_context, _queryProvider);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = _context.CreateConnection();
        conn.Execute("DELETE FROM DATA_Budget_BW WHERE id IN @ids", new { ids = _insertedIds });
        _insertedIds.Clear();
    }

    // -------------------------------------------------
    // TEST: ADD
    // -------------------------------------------------
    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var budget = new DataBudgetBw
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
            Mes00 = 100,
            Mes01 = 200,
            Mes02 = 300,
            Mes03 = 400,
            Mes04 = 500,
            Mes05 = 600,
            Mes06 = 700,
            Mes07 = 800,
            Mes08 = 900,
            Mes09 = 1000,
            Mes10 = 1100,
            Mes11 = 1200,
            Mes12 = 1300,
            Mes13 = 1400
        };

        await _repository.AddAsync(budget);

        int newId;
        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>("SELECT id FROM DATA_Budget_BW ORDER BY id DESC LIMIT 1;");
        }

        _insertedIds.Add(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<DataBudgetBw>("SELECT * FROM DATA_Budget_BW WHERE id = @id", new { id = newId });

        Assert.NotNull(result);
        Assert.Equal(budget.Mes00, result.Mes00);
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
                INSERT INTO DATA_Budget_BW
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (1, '00000000-0000-0000-0000-000000000000', NOW(), 1, 2025, 1, 1, 1, 1,
                 10,11,12,13,14,15,16,17,18,19,20,21,22,23);
                SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        var result = await _repository.GetByIdAsync(newId);

        Assert.NotNull(result);
        Assert.Equal(10, result.Mes00);
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
                INSERT INTO DATA_Budget_BW
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (1, '00000000-0000-0000-0000-000000000001', NOW(), 1, 2025, 1, 1, 1, 1,
                 10,11,12,13,14,15,16,17,18,19,20,21,22,23);
                SELECT LAST_INSERT_ID();
            ");

            id2 = conn.ExecuteScalar<int>(@"
                INSERT INTO DATA_Budget_BW
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (1, '00000000-0000-0000-0000-000000000002', NOW(), 1, 2025, 1, 1, 1, 1,
                 20,21,22,23,24,25,26,27,28,29,30,31,32,33);
                SELECT LAST_INSERT_ID();
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
                INSERT INTO DATA_Budget_BW
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (1, '00000000-0000-0000-0000-000000000003', NOW(), 1, 2025, 1, 1, 1, 1,
                 10,11,12,13,14,15,16,17,18,19,20,21,22,23);
                SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        var updated = new DataBudgetBw
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
            Mes00 = 999,
            Mes01 = 11,
            Mes02 = 12,
            Mes03 = 13,
            Mes04 = 14,
            Mes05 = 15,
            Mes06 = 16,
            Mes07 = 17,
            Mes08 = 18,
            Mes09 = 19,
            Mes10 = 20,
            Mes11 = 21,
            Mes12 = 22,
            Mes13 = 23
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<DataBudgetBw>("SELECT * FROM DATA_Budget_BW WHERE id = @id", new { id = newId });

        Assert.Equal(999, result.Mes00);
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
                INSERT INTO DATA_Budget_BW
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (1, '00000000-0000-0000-0000-000000000004', NOW(), 1, 2025, 1, 1, 1, 1,
                 10,11,12,13,14,15,16,17,18,19,20,21,22,23);
                SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        await _repository.DeleteAsync(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<DataBudgetBw>("SELECT * FROM DATA_Budget_BW WHERE id = @id", new { id = newId });

        Assert.Null(result);
    }
}

