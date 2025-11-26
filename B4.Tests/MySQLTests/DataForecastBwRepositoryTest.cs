namespace B4.Tests.MySQLTests;

using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;

public class DataForecastBwRepositoryTest : IDisposable
{
    private readonly MySQLDapperContext _context;
    private readonly DataForecastBwRepository _repository;

    // IDs generados automáticamente por AUTO_INCREMENT
    private readonly List<int> _insertedIds = new();

    public DataForecastBwRepositoryTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new MySQLDapperContext(config);
        _repository = new DataForecastBwRepository(_context);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = _context.CreateConnection();
        conn.Execute("DELETE FROM DATA_Forecast_BW WHERE id IN @ids", new { ids = _insertedIds });
        _insertedIds.Clear();
    }

    // -------------------------------------------------
    // TEST: ADD
    // -------------------------------------------------
    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var forecast = new DataForecastBw
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
            Mes00 = 0,
            Mes01 = 0,
            Mes02 = 0,
            Mes03 = 0,
            Mes04 = 0,
            Mes05 = 0,
            Mes06 = 0,
            Mes07 = 0,
            Mes08 = 0,
            Mes09 = 0,
            Mes10 = 0,
            Mes11 = 0,
            Mes12 = 0,
            Mes13 = 0,
            IdCarga = null,
            IdCargaSTGBW = null,
            IdHoja = null
        };

        await _repository.AddAsync(forecast);

        int newId;
        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>(@"
                SELECT id FROM DATA_Forecast_BW
                ORDER BY id DESC LIMIT 1;");
        }

        _insertedIds.Add(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<DataForecastBw>(
            "SELECT * FROM DATA_Forecast_BW WHERE id = @id", new { id = newId });

        Assert.NotNull(result);
        Assert.Equal(forecast.IdAPICarga, result.IdAPICarga);
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
                INSERT INTO DATA_Forecast_BW 
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (1, '00000000-0000-0000-0000-000000000001', NOW(), 1, 2025, 1, 1, 1, 1,
                 0,0,0,0,0,0,0,0,0,0,0,0,0,0);
                SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        var result = await _repository.GetByIdAsync(newId);

        Assert.NotNull(result);
        Assert.Equal(1, result.IdAPICarga);
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
                INSERT INTO DATA_Forecast_BW 
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (1, '00000000-0000-0000-0000-000000000001', NOW(), 1, 2025, 1, 1, 1, 1,
                 0,0,0,0,0,0,0,0,0,0,0,0,0,0);
                SELECT LAST_INSERT_ID();
            ");

            id2 = conn.ExecuteScalar<int>(@"
                INSERT INTO DATA_Forecast_BW 
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (2, '00000000-0000-0000-0000-000000000002', NOW(), 1, 2025, 1, 1, 1, 1,
                 0,0,0,0,0,0,0,0,0,0,0,0,0,0);
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
                INSERT INTO DATA_Forecast_BW 
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (1, '00000000-0000-0000-0000-000000000003', NOW(), 1, 2025, 1, 1, 1, 1,
                 0,0,0,0,0,0,0,0,0,0,0,0,0,0);
                SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        var updated = new DataForecastBw
        {
            Id = newId,
            IdAPICarga = 99,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.UtcNow,
            IdCompany = 1,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdCurrency = 1,
            IdEpigrafe = 1,
            Mes00 = 1,
            Mes01 = 1,
            Mes02 = 1,
            Mes03 = 1,
            Mes04 = 1,
            Mes05 = 1,
            Mes06 = 1,
            Mes07 = 1,
            Mes08 = 1,
            Mes09 = 1,
            Mes10 = 1,
            Mes11 = 1,
            Mes12 = 1,
            Mes13 = 1,
            IdCarga = null,
            IdCargaSTGBW = null,
            IdHoja = null
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<DataForecastBw>(
            "SELECT * FROM DATA_Forecast_BW WHERE id = @id", new { id = newId });

        Assert.Equal(99, result.IdAPICarga);
        Assert.Equal(1, result.Mes00);
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
                INSERT INTO DATA_Forecast_BW 
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (1, '00000000-0000-0000-0000-000000000004', NOW(), 1, 2025, 1, 1, 1, 1,
                 0,0,0,0,0,0,0,0,0,0,0,0,0,0);
                SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        await _repository.DeleteAsync(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<DataForecastBw>(
            "SELECT * FROM DATA_Forecast_BW WHERE id = @id",
            new { id = newId });

        Assert.Null(result);
    }
}

