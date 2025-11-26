namespace B4.Tests.MySQLTests;

using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;

public class DataActualsBwRepositoryTest : IDisposable
{
    private readonly MySQLDapperContext _context;
    private readonly DataActualsBwRepository _repository;

    private readonly List<int> _insertedIds = new();

    public DataActualsBwRepositoryTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new MySQLDapperContext(config);
        _repository = new DataActualsBwRepository(_context);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = _context.CreateConnection();
        conn.Execute("DELETE FROM DATA_Actuals_BW WHERE id IN @ids", new { ids = _insertedIds });

        _insertedIds.Clear();
    }

    // -------------------------------------------------
    // TEST: ADD
    // -------------------------------------------------
    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var entity = new DataActualsBw
        {
            IdAPICarga = 1,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.Now,
            IdCompany = 10,
            Ejercicio = 2025,
            IdCiclo = 2,
            IdFase = 3,
            IdCurrency = 1,
            IdEpigrafe = 100,
            Mes00 = 1,
            Mes01 = 2,
            Mes02 = 3,
            Mes03 = 4,
            Mes04 = 5,
            Mes05 = 6,
            Mes06 = 7,
            Mes07 = 8,
            Mes08 = 9,
            Mes09 = 10,
            Mes10 = 11,
            Mes11 = 12,
            Mes12 = 13,
            Mes13 = 14,
            IdCarga = null,
            IdCargaSTGBW = null,
            IdHoja = null
        };

        await _repository.AddAsync(entity);

        int newId;

        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>(@"
                SELECT id FROM DATA_Actuals_BW
                ORDER BY id DESC LIMIT 1;
            ");
        }

        _insertedIds.Add(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<DataActualsBw>(
            "SELECT * FROM DATA_Actuals_BW WHERE id = @id", new { id = newId });

        Assert.NotNull(result);
        Assert.Equal(2025, result.Ejercicio);
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
                INSERT INTO DATA_Actuals_BW
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio,
                 idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07,
                 mes08, mes09, mes10, mes11, mes12, mes13,
                 idCarga, idCargaSTGBW, idHoja)
                VALUES
                (1, UUID(), NOW(), 10, 2030,
                 2, 3, 1, 200,
                 1,2,3,4,5,6,7,8,9,10,11,12,13,14,
                 NULL, NULL, NULL);

                SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        var result = await _repository.GetByIdAsync(newId);

        Assert.NotNull(result);
        Assert.Equal(2030, result.Ejercicio);
        Assert.Equal(200, result.IdEpigrafe);
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
                INSERT INTO DATA_Actuals_BW
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio,
                 idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07,
                 mes08, mes09, mes10, mes11, mes12, mes13,
                 idCarga, idCargaSTGBW, idHoja)
                VALUES
                (1, UUID(), NOW(), 20, 2040,
                 2, 3, 1, 300,
                 1,2,3,4,5,6,7,8,9,10,11,12,13,14,
                 NULL, NULL, NULL);

                SELECT LAST_INSERT_ID();
            ");

            id2 = conn.ExecuteScalar<int>(@"
                INSERT INTO DATA_Actuals_BW
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio,
                 idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07,
                 mes08, mes09, mes10, mes11, mes12, mes13,
                 idCarga, idCargaSTGBW, idHoja)
                VALUES
                (1, UUID(), NOW(), 30, 2050,
                 2, 3, 1, 400,
                 1,2,3,4,5,6,7,8,9,10,11,12,13,14,
                 NULL, NULL, NULL);

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
                INSERT INTO DATA_Actuals_BW
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio,
                 idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07,
                 mes08, mes09, mes10, mes11, mes12, mes13,
                 idCarga, idCargaSTGBW, idHoja)
                VALUES
                (1, UUID(), NOW(), 40, 2022,
                 2, 3, 1, 500,
                 1,2,3,4,5,6,7,8,9,10,11,12,13,14,
                 NULL, NULL, NULL);

                SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        var updated = new DataActualsBw
        {
            Id = newId,
            IdAPICarga = 9,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.Now,
            IdCompany = 99,
            Ejercicio = 2099,
            IdCiclo = 9,
            IdFase = 9,
            IdCurrency = 9,
            IdEpigrafe = 999,
            Mes00 = 9,
            Mes01 = 9,
            Mes02 = 9,
            Mes03 = 9,
            Mes04 = 9,
            Mes05 = 9,
            Mes06 = 9,
            Mes07 = 9,
            Mes08 = 9,
            Mes09 = 9,
            Mes10 = 9,
            Mes11 = 9,
            Mes12 = 9,
            Mes13 = 9,
            IdCarga = null,
            IdCargaSTGBW = null,
            IdHoja = null
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<DataActualsBw>(
            "SELECT * FROM DATA_Actuals_BW WHERE id = @id", new { id = newId });

        Assert.Equal(2099, result.Ejercicio);
        Assert.Equal(999, result.IdEpigrafe);
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
                INSERT INTO DATA_Actuals_BW
                (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio,
                 idCiclo, idFase, idCurrency, idEpigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07,
                 mes08, mes09, mes10, mes11, mes12, mes13,
                 idCarga, idCargaSTGBW, idHoja)
                VALUES
                (1, UUID(), NOW(), 50, 2020,
                 2, 3, 1, 600,
                 1,2,3,4,5,6,7,8,9,10,11,12,13,14,
                 NULL, NULL, NULL);

                SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        await _repository.DeleteAsync(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<DataActualsBw>(
            "SELECT * FROM DATA_Actuals_BW WHERE id = @id",
            new { id = newId });

        Assert.Null(result);
    }
}

