using Xunit;
using Npgsql;
using Dapper;
using B4.Models.Entities;
using B4.Data.PostgreSQL.Repositories;

public class DataBridgesFyRepositoryTests : IDisposable
{
    private const string Conn =
        "Host=pg-3105f5b7-elpuig-23d9.b.aivencloud.com;Port=21134;Database=defaultdb;Username=avnadmin;Password=AVNS_D8EOFxPDlpUH6LhFFLw;Ssl Mode=Require;";

    private readonly DataBridgesFyRepository _repo;
    private readonly List<int> _insertedIds = new();

    public DataBridgesFyRepositoryTests()
    {
        _repo = new DataBridgesFyRepository(Conn);
    }

    // 🔥 Se ejecuta después de cada test
    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = new NpgsqlConnection(Conn);
        conn.Execute("DELETE FROM b4.data_bridges_fy WHERE id = ANY(@Ids)",
            new { Ids = _insertedIds.ToArray() });

        _insertedIds.Clear();
    }

    // -------------------------------------------------------------
    // TEST 1: Comprobar conexión básica
    // -------------------------------------------------------------
    [Fact]
    public async Task Test_Connection()
    {
        using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        Assert.Equal(System.Data.ConnectionState.Open, conn.State);
    }

    // -------------------------------------------------------------
    // TEST 2: Insert + GetById
    // -------------------------------------------------------------
    [Fact]
    public async Task Insert_And_GetById_Should_Work()
    {
        var entity = CreateSampleEntity();

        await _repo.AddAsync(entity);
        _insertedIds.Add(entity.Id);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal(entity.IdCompany, result!.IdCompany);
        Assert.Equal(entity.Actuals, result.Actuals);
    }

    // -------------------------------------------------------------
    // TEST 3: Update
    // -------------------------------------------------------------
    [Fact]
    public async Task Update_Should_Modify_Entity()
    {
        var entity = CreateSampleEntity();
        await _repo.AddAsync(entity);
        _insertedIds.Add(entity.Id);

        entity.PctActuals = 999.99m;

        await _repo.UpdateAsync(entity);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal(999.99m, result!.PctActuals);
    }

    // -------------------------------------------------------------
    // TEST 4: Delete
    // -------------------------------------------------------------
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


    // -------------------------------------------------------------
    // Helper: Crea una entidad válida para pruebas
    // -------------------------------------------------------------
    private DataBridgesFy CreateSampleEntity()
    {
        return new DataBridgesFy
        {
            IdAPICarga = 1,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.UtcNow,
            IdCompany = 10,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdCurrency = 1,
            IdEpigrafe = 1,

            Actuals = 100m,
            PctActuals = 10m,
            Budget = 200m,
            PctBudget = 20m,
            Variance = -100m,

            Volume = 50m,
            InventoryChange = 5m,
            Mix = 10m,
            New = 30m,
            Economics = 15m,
            QuickSavings = 3m,
            CurrencyMix = 4m,
            ExchangeRate = 1.10m,
            RawMaterial = 20m,
            Scrap = 2m,
            IndustrialPerformance = 12m,
            ProtoTooling = 7m,
            Other = 8m,
            Check = 1m,
            Comments = "Test comment",

            IdCarga = null,
            IdCargaSTGBW = null,
            IdHoja = null
        };
    }
}
