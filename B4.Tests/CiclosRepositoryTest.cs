/*
namespace B4.Tests;

using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;

public class CiclosRepositoryTest : IDisposable
{
    private readonly DapperContext _context;
    private readonly CiclosRepository _repository;

    // IDs insertados durante cada test → eliminados en Dispose()
    private readonly List<int> _insertedIds = new();

    public CiclosRepositoryTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new DapperContext(config);
        _repository = new CiclosRepository(_context);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = _context.CreateConnection();

        conn.Execute(
            "DELETE FROM LK_CICLOS WHERE IdCiclo IN @ids",
            new { ids = _insertedIds });

        _insertedIds.Clear();
    }

    // -------------------------------------------------------------------
    // TESTS
    // -------------------------------------------------------------------

    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var ciclo = new LkCiclos();
        {
            Id = 99999,
            IdCiclo = 10,
            IdHoja = 20,
            NombreCiclo = "CICLO_ADD",
            Descripcion = "Descripcion ADD"
        };

        _insertedIds.Add(ciclo.IdCiclo);

        await _repository.AddAsync(ciclo);

        using var conn = _context.CreateConnection();
        var result = conn.QuerySingleOrDefault<LkCiclo>(
            "SELECT * FROM LK_CICLOS WHERE IdCiclo = 99999");

        Assert.NotNull(result);
        Assert.Equal("CICLO_ADD", result.NombreCiclo);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_CICLOS 
                (IdCiclo, IdPlantilla, IdHoja, NombreCiclo, Descripcion)
                VALUES (99999, 11, 21, 'CICLO_BYID', 'DES BYID');
            ");
        }

        _insertedIds.Add(99999);

        var result = await _repository.GetByIdAsync(99999);

        Assert.NotNull(result);
        Assert.Equal("CICLO_BYID", result.NombreCiclo);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_CICLOS 
                VALUES 
                (99999, 12, 22, 'A', 'DESC A'),
                (99998, 13, 23, 'B', 'DESC B');
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
                INSERT INTO LK_CICLOS 
                VALUES (99999, 14, 24, 'ORIGINAL', 'DESC ORIGINAL');
            ");
        }

        _insertedIds.Add(99999);

        var updated = new LkCiclos
        {
            IdCiclo = 99999,
            IdPlantilla = 14,
            IdHoja = 24,
            NombreCiclo = "UPDATED",
            Descripcion = "DESC UPDATED"
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<LkCiclos>(
            "SELECT * FROM LK_CICLOS WHERE IdCiclo = 99999");

        Assert.Equal("UPDATED", result.NombreCiclo);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_CICLOS 
                VALUES (99999, 15, 25, 'DEL', 'DESC DEL');
            ");
        }

        _insertedIds.Add(99999);

        await _repository.DeleteAsync(99999);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<LkCiclo>(
            "SELECT * FROM LK_CICLOS WHERE IdCiclo = 99999");

        Assert.Null(result);
    }
}

*/