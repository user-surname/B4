using Xunit;
using Npgsql;
using Dapper;
using B4.Models.Entities;
using B4.Data.PostgreSQL.Repositories;

public class DataBridgesMonthBwRepositoryTests : IDisposable
{
    private const string Conn =
        "Host=pg-3105f5b7-elpuig-23d9.b.aivencloud.com;Port=21134;Database=defaultdb;Username=avnadmin;Password=AVNS_D8EOFxPDlpUH6LhFFLw;Ssl Mode=Require;";

    private readonly DataBridgesMonthBwRepository _repo;
    private readonly List<int> _insertedIds = new();

    public DataBridgesMonthBwRepositoryTests()
    {
        _repo = new DataBridgesMonthBwRepository(Conn);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = new NpgsqlConnection(Conn);
        conn.Execute("DELETE FROM b4.data_bridges_month_bw WHERE id = ANY(@Ids)",
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
        var entity = CreateSampleEntity();

        await _repo.AddAsync(entity);
        _insertedIds.Add(entity.Id);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal(entity.Volume, result!.Volume);
        Assert.Equal(entity.Economics, result.Economics);
    }

    [Fact]
    public async Task Update_Should_Modify_Entity()
    {
        var entity = CreateSampleEntity();
        await _repo.AddAsync(entity);
        _insertedIds.Add(entity.Id);

        entity.QuickSavings = 999m;

        await _repo.UpdateAsync(entity);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal(999m, result!.QuickSavings);
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

    private DataBridgesMonthBw CreateSampleEntity()
    {
        return new DataBridgesMonthBw
        {
            IdAPICarga = 1,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.UtcNow,
            IdCompany = 300,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdCurrency = 1,
            IdEpigrafe = 1,

            Volume = 10m,
            InventoryChange = 2m,
            Mix = 3m,
            New = 4m,
            Economics = 5m,
            QuickSavings = 6m,
            CurrencyMix = 7m,
            ExchangeRate = 1.2m,
            RawMaterial = 8m,
            Scrap = 1m,
            IndustrialPerformance = 9m,
            ProtoTooling = 2m,
            Other = 3m,
            Check = 0.5m,
            Comments = "Test month bridge",

            IdCarga = null,
            IdCargaSTGBW = null,
            IdHoja = null
        };
    }
}
