using Xunit;
using Npgsql;
using Dapper;
using B4.Models.Entities;
using B4.Data.PostgreSQL.Repositories;

public class DataBridgesFyBwEurRepositoryTests : IDisposable
{
    private const string Conn =
        "Host=pg-3105f5b7-elpuig-23d9.b.aivencloud.com;Port=21134;Database=defaultdb;Username=avnadmin;Password=AVNS_D8EOFxPDlpUH6LhFFLw;Ssl Mode=Require;";

    private readonly DataBridgesFyBwEurRepository _repo;
    private readonly List<int> _insertedIds = new();

    public DataBridgesFyBwEurRepositoryTests()
    {
        _repo = new DataBridgesFyBwEurRepository(Conn);
    }

    public void Dispose()
    {
        if (_insertedIds.Count > 0)
        {
            using var conn = new NpgsqlConnection(Conn);
            conn.Execute("DELETE FROM b4.data_bridges_fy_bw_eur WHERE id = ANY(@Ids)",
                new { Ids = _insertedIds.ToArray() });
        }
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
        var entity = CreateSampleEntity();

        await _repo.AddAsync(entity);
        _insertedIds.Add(entity.Id);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal(entity.Actuals, result!.Actuals);
        Assert.Equal(entity.CurrencyMix, result.CurrencyMix);
    }

    [Fact]
    public async Task Update_Should_Modify_Entity()
    {
        var entity = CreateSampleEntity();
        await _repo.AddAsync(entity);
        _insertedIds.Add(entity.Id);

        entity.ExchangeRate = 9.99m;

        await _repo.UpdateAsync(entity);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal(9.99m, result!.ExchangeRate);
    }

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

    private DataBridgesFyBwEur CreateSampleEntity()
    {
        return new DataBridgesFyBwEur
        {
            IdAPICarga = 1,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.UtcNow,
            IdCompany = 100,
            Ejercicio = 2025,
            IdCiclo = 2,
            IdFase = 1,
            IdCurrency = 1,
            IdEpigrafe = 1,

            Actuals = 100m,
            PctActuals = 10m,
            Budget = 120m,
            PctBudget = 12m,
            Variance = -20m,
            Volume = 30m,
            InventoryChange = 3m,
            Mix = 5m,
            New = 10m,
            Economics = 7m,
            QuickSavings = 2m,
            CurrencyMix = 1m,
            ExchangeRate = 1.15m,
            RawMaterial = 6m,
            Scrap = 1m,
            IndustrialPerformance = 4m,
            ProtoTooling = 2m,
            Other = 1m,
            Check = 0.5m,
            Comments = "Test EUR bridge",

            IdCarga = null,
            IdCargaSTGBW = null,
            IdHoja = null
        };
    }
}
