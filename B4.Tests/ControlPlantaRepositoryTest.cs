namespace B4.Tests;

using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;

public class ControlPlantaRepositoryTest : IDisposable
{
    private readonly DapperContext _context;
    private readonly ControlPlantaRepository _repository;

    // Lista para IDs insertados en cada test
    private readonly List<int> _insertedIds = new();

    public ControlPlantaRepositoryTest()
    {
        // Cargar configuración desde appsettings.json
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        // Crear contexto y repositorio
        _context = new DapperContext(config);
        _repository = new ControlPlantaRepository(_context);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = _context.CreateConnection();

        conn.Execute(
            "DELETE FROM CONTROL_PLANTA WHERE idControlPlanta IN @ids",
            new { ids = _insertedIds });

        _insertedIds.Clear();
    }

    // -------------------------------------------------------------------
    // TESTS
    // -------------------------------------------------------------------

    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var entity = new ControlPlanta
        {
            IdControlPlanta = 99999,
            IdControl = 100,
            IdCompany = 200
        };

        _insertedIds.Add(entity.IdControlPlanta);

        await _repository.AddAsync(entity);

        using var conn = _context.CreateConnection();
        var result = conn.QuerySingleOrDefault<ControlPlanta>(
            "SELECT * FROM CONTROL_PLANTA WHERE idControlPlanta = 99999");

        Assert.NotNull(result);
        Assert.Equal(100, result.IdControl);
        Assert.Equal(200, result.IdCompany);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO CONTROL_PLANTA
                (idControlPlanta, idControl, idCompany)
                VALUES (99999, 10, 20);
            ");
        }

        _insertedIds.Add(99999);

        var result = await _repository.GetByIdAsync(99999);

        Assert.NotNull(result);
        Assert.Equal(10, result.IdControl);
        Assert.Equal(20, result.IdCompany);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO CONTROL_PLANTA VALUES
                (99999, 111, 222),
                (99998, 333, 444);
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
                INSERT INTO CONTROL_PLANTA
                VALUES (99999, 50, 60);
            ");
        }

        _insertedIds.Add(99999);

        var updated = new ControlPlanta
        {
            IdControlPlanta = 99999,
            IdControl = 500,
            IdCompany = 600
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<ControlPlanta>(
            "SELECT * FROM CONTROL_PLANTA WHERE idControlPlanta = 99999");

        Assert.Equal(500, result.IdControl);
        Assert.Equal(600, result.IdCompany);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO CONTROL_PLANTA
                VALUES (99999, 77, 88);
            ");
        }

        _insertedIds.Add(99999);

        await _repository.DeleteAsync(99999);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<ControlPlanta>(
            "SELECT * FROM CONTROL_PLANTA WHERE idControlPlanta = 99999");

        Assert.Null(result);
    }
}

