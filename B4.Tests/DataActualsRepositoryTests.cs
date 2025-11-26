using Xunit;
using Npgsql;
using Dapper;
using B4.Models.Entities;
using B4.Data.PostgreSQL.Repositories;

public class DataActualsRepositoryTests : IDisposable
{
    private const string Conn =
        "Host=pg-3105f5b7-elpuig-23d9.b.aivencloud.com;Port=21134;Database=defaultdb;Username=avnadmin;Password=AVNS_D8EOFxPDlpUH6LhFFLw;Ssl Mode=Require;";

    private readonly DataActualsRepository _repo;
    private readonly List<int> _insertedIds = new();

    public DataActualsRepositoryTests()
    {
        _repo = new DataActualsRepository(Conn);
    }

    // 🧹 Limpieza automática después de cada test
    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = new NpgsqlConnection(Conn);
        conn.Execute("DELETE FROM b4.data_actuals WHERE id = ANY(@Ids)", 
            new { Ids = _insertedIds.ToArray() });

        _insertedIds.Clear();
    }

    // ----------------------------------------------
    // 🔥 TEST 1: Conexión básica
    // ----------------------------------------------
    [Fact]
    public async Task Test_Connection()
    {
        using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        Assert.Equal(System.Data.ConnectionState.Open, conn.State);
    }

    // ----------------------------------------------
    // 🔥 TEST 2: Insert + GetById
    // ----------------------------------------------
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

    // ----------------------------------------------
    // 🔥 TEST 3: Update
    // ----------------------------------------------
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

    // ----------------------------------------------
    // 🔥 TEST 4: Delete
    // ----------------------------------------------
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
    // 🔧 Helper: generar una entidad válida
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
