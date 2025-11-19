namespace B4.Tests;

using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;

public class FasesRepositoryTest : IDisposable
{
    private readonly DapperContext _context;
    private readonly FasesRepository _repository;

    // Lista de IDs insertados durante cada test para limpiarlos después
    private readonly List<int> _insertedIds = new();

    public FasesRepositoryTest()
    {
        // Cargar configuración desde appsettings.json
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new DapperContext(config);
        _repository = new FasesRepository(_context);
    }

    // Limpieza después de cada test
    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = _context.CreateConnection();

        conn.Execute(
            "DELETE FROM LK_FASES WHERE idFase IN @ids",
            new { ids = _insertedIds });

        _insertedIds.Clear();
    }

    // -------------------------------------------------------------------
    // TESTS
    // -------------------------------------------------------------------

    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var fase = new LkFases
        {
            IdFase = 99999,
            Fase = "FASE ADD",
            FaseAlias = "ADD"
        };

        _insertedIds.Add(fase.IdFase);

        await _repository.AddAsync(fase);

        using var conn = _context.CreateConnection();
        var result = conn.QuerySingleOrDefault<LkFases>(
            "SELECT * FROM LK_FASES WHERE idFase = 99999");

        Assert.NotNull(result);
        Assert.Equal("FASE ADD", result.Fase);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_FASES (idFase, Fase, FaseAlias)
                VALUES (99999, 'FASE BYID', 'BYID');
            ");
        }

        _insertedIds.Add(99999);

        var result = await _repository.GetByIdAsync(99999);

        Assert.NotNull(result);
        Assert.Equal("FASE BYID", result.Fase);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_FASES VALUES
                (99999, 'FASE A', 'A'),
                (99998, 'FASE B', 'B');
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
                INSERT INTO LK_FASES VALUES
                (99999, 'FASE ORIGINAL', 'ORIG');
            ");
        }

        _insertedIds.Add(99999);

        var updated = new LkFases
        {
            IdFase = 99999,
            Fase = "FASE UPDATED",
            FaseAlias = "UPD"
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<LkFases>(
            "SELECT * FROM LK_FASES WHERE idFase = 99999");

        Assert.Equal("FASE UPDATED", result.Fase);
        Assert.Equal("UPD", result.FaseAlias);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_FASES VALUES
                (99999, 'FASE DELETE', 'DEL');
            ");
        }

        _insertedIds.Add(99999);

        await _repository.DeleteAsync(99999);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<LkFases>(
            "SELECT * FROM LK_FASES WHERE idFase = 99999");

        Assert.Null(result);
    }
}

