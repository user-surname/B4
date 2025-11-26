namespace B4.Tests.MySQLTests;

using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;

public class DataForecastRepositoryTest : IDisposable
{
    private readonly DapperContext _context;
    private readonly DataForecastRepository _repository;

    // IDs generados automáticamente por AUTO_INCREMENT
    private readonly List<int> _insertedIds = new();

    public DataForecastRepositoryTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new DapperContext(config);
        _repository = new DataForecastRepository(_context);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = _context.CreateConnection();
        conn.Execute("DELETE FROM DATA_Forecast WHERE id IN @ids", new { ids = _insertedIds });
        _insertedIds.Clear();
    }

    // -------------------------------------------------
    // TEST: ADD
    // -------------------------------------------------
    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var forecast = new DataForecast
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
            Mes01 = 100,
            Mes02 = 100,
            Mes03 = 100,
            Mes04 = 100,
            Mes05 = 100,
            Mes06 = 100,
            Mes07 = 100,
            Mes08 = 100,
            Mes09 = 100,
            Mes10 = 100,
            Mes11 = 100,
            Mes12 = 100,
            Mes13 = 100
        };

        await _repository.AddAsync(forecast);

        int newId;
        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>("SELECT id FROM DATA_Forecast ORDER BY id DESC LIMIT 1;");
        }

        _insertedIds.Add(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<DataForecast>("SELECT * FROM DATA_Forecast WHERE id = @id", new { id = newId });

        Assert.NotNull(result);
        Assert.Equal(100, result.Mes00);
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
                INSERT INTO DATA_Forecast 
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                 200,200,200,200,200,200,200,200,200,200,200,200,200,200);
                SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        var result = await _repository.GetByIdAsync(newId);

        Assert.NotNull(result);
        Assert.Equal(200, result.Mes00);
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
                INSERT INTO DATA_Forecast 
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                 101,101,101,101,101,101,101,101,101,101,101,101,101,101);
                SELECT LAST_INSERT_ID();
            ");

            id2 = conn.ExecuteScalar<int>(@"
                INSERT INTO DATA_Forecast 
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                 102,102,102,102,102,102,102,102,102,102,102,102,102,102);
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
                INSERT INTO DATA_Forecast 
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                 300,300,300,300,300,300,300,300,300,300,300,300,300,300);
                SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        var updated = new DataForecast
        {
            Id = newId,
            IdAPICarga = 1,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.Now,
            IdCompany = 1,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdCurrency = 1,
            IdEpigrafe = 1,
            Mes00 = 999
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<DataForecast>("SELECT * FROM DATA_Forecast WHERE id = @id", new { id = newId });

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
                INSERT INTO DATA_Forecast 
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                 400,400,400,400,400,400,400,400,400,400,400,400,400,400);
                SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        await _repository.DeleteAsync(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<DataForecast>("SELECT * FROM DATA_Forecast WHERE id = @id", new { id = newId });

        Assert.Null(result);
    }
}
