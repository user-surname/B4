using Xunit;
using Npgsql;
using Dapper;
using B4.Models.Entities;
using B4.Data.PostgreSQL.Repositories;

public class DataForecastRepositoryTests : IDisposable
{
    private const string Conn =
        "Host=pg-3105f5b7-elpuig-23d9.b.aivencloud.com;Port=21134;Database=defaultdb;Username=avnadmin;Password=AVNS_D8EOFxPDlpUH6LhFFLw;Ssl Mode=Require;";

    private readonly DataForecastRepository _repo;
    private readonly List<int> _insertedIds = new();

    public DataForecastRepositoryTests()
    {
        _repo = new DataForecastRepository(Conn);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = new NpgsqlConnection(Conn);
        conn.Execute("DELETE FROM b4.data_forecast WHERE id = ANY(@Ids)",
            new { Ids = _insertedIds.ToArray() });

        _insertedIds.Clear();
    }

    [Fact]
    public async Task Test_Connection()
    {
        using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        Assert.Equal(System.Data.ConnectionState.Open, conn.State);
    }

    [Fact]
    public async Task Insert_And_GetById_Should_Work()
    {
        var entity = CreateEntity();

        await _repo.AddAsync(entity);
        _insertedIds.Add(entity.Id);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal(entity.IdCompany, result!.IdCompany);
    }

    [Fact]
    public async Task Update_Should_Modify_Entity()
    {
        var entity = CreateEntity();
        await _repo.AddAsync(entity);
        _insertedIds.Add(entity.Id);

        entity.Mes03 = 777m;

        await _repo.UpdateAsync(entity);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal(777m, result!.Mes03);
    }

    [Fact]
    public async Task Delete_Should_Remove_Entity()
    {
        var entity = CreateEntity();
        await _repo.AddAsync(entity);
        _insertedIds.Add(entity.Id);

        await _repo.DeleteAsync(entity.Id);

        var result = await _repo.GetByIdAsync(entity.Id);
        Assert.Null(result);
    }

    // Helper: entidad base
    private DataForecast CreateEntity()
    {
        return new DataForecast
        {
            IdAPICarga = 99,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.UtcNow,
            IdCompany = 500,
            Ejercicio = 2026,
            IdCiclo = 3,
            IdFase = 2,
            IdCurrency = 1,
            IdEpigrafe = 1,
            Mes00 = 1m, Mes01 = 2m, Mes02 = 3m, Mes03 = 4m, Mes04 = 5m,
            Mes05 = 6m, Mes06 = 7m, Mes07 = 8m, Mes08 = 9m,
            Mes09 = 10m, Mes10 = 11m, Mes11 = 12m, Mes12 = 13m, Mes13 = 14m
        };
    }
}
