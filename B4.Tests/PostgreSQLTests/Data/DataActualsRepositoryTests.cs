using Xunit;
using Npgsql;
using Dapper;
using B4.Data.PostgreSQL.Repositories;
using B4.Models.Entities.DataEtities;

namespace B4.Tests.PostgreSQLTests.Data
{
public class DataActualsRepositoryTests : IDisposable
{
    private readonly DataActualsRepository _repo;
    private readonly List<int> _insertedIds = new();

    public DataActualsRepositoryTests()
    {
        _repo = new DataActualsRepository(TestConfig.Conn);
    }

    // Limpieza automática
    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = new NpgsqlConnection(TestConfig.Conn);

        conn.Execute(
            "DELETE FROM b4.data_actuals WHERE id = ANY(@Ids)",
            new { Ids = _insertedIds.ToArray() }
        );

        _insertedIds.Clear();
    }

    // TEST 1: Conexión
    [Fact]
    public async Task Test_Connection()
    {
        using var conn = new NpgsqlConnection(TestConfig.Conn);
        await conn.OpenAsync();

        Assert.Equal(System.Data.ConnectionState.Open, conn.State);
    }

    // TEST 2: Insert + Get
    [Fact]
    public async Task Insert_And_GetById_Should_Work()
    {
        var entity = CreateSampleEntity();

        await _repo.AddAsync(entity);
        _insertedIds.Add(entity.Id);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal(entity.Ejercicio, result!.Ejercicio);
        Assert.Equal(entity.Mes01, result.Mes01);
    }

    // TEST 3: Update
    [Fact]
    public async Task Update_Should_Modify_Entity()
    {
        var entity = CreateSampleEntity();

        await _repo.AddAsync(entity);
        _insertedIds.Add(entity.Id);

        entity.Mes02 = 777m;

        await _repo.UpdateAsync(entity);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal(777m, result!.Mes02);
    }

    // TEST 4: Delete
    [Fact]
    public async Task Delete_Should_Remove_Entity()
    {
        var entity = CreateSampleEntity();

        await _repo.AddAsync(entity);
        _insertedIds.Add(entity.Id);

        await _repo.DeleteAsync(entity.Id);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.Null(result);
    }

    // ----------------------------------------------
    //  Helper: generar una entidad válida
    // ----------------------------------------------
    private DataActuals CreateSampleEntity()
    {
        return new DataActuals
        {
            IdAPICarga = 1,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.UtcNow,
            IdCompany = 50,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdCurrency = 1,
            IdEpigrafe = 1,

            Mes00 = 10m,
            Mes01 = 20m,
            Mes02 = 30m,
            Mes03 = 40m,
            Mes04 = 50m,
            Mes05 = 60m,
            Mes06 = 70m,
            Mes07 = 80m,
            Mes08 = 90m,
            Mes09 = 100m,
            Mes10 = 110m,
            Mes11 = 120m,
            Mes12 = 130m,
            Mes13 = 140m
        };
    }
}
}
