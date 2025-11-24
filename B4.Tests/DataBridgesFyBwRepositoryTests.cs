using Xunit;
using Npgsql;
using Dapper;
using B4.Models.Entities;
using B4.Data.PostgreSQL.Repositories;

public class DataBridgesFyBwRepositoryTests : IDisposable
{
    private const string Conn =
        "Host=pg-3105f5b7-elpuig-23d9.b.aivencloud.com;Port=21134;Database=defaultdb;Username=avnadmin;Password=AVNS_D8EOFxPDlpUH6LhFFLw;Ssl Mode=Require;";

    private readonly DataBridgesFyBwRepository _repo;
    private readonly List<int> _insertedIds = new();

    public DataBridgesFyBwRepositoryTests()
    {
        _repo = new DataBridgesFyBwRepository(Conn);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = new NpgsqlConnection(Conn);
        conn.Execute("DELETE FROM b4.data_bridges_fy_bw WHERE id = ANY(@Ids)",
            new { Ids = _insertedIds.ToArray() });

        _insertedIds.Clear();
    }

    // -------------------------------------------------------------
    // TEST 1 - Conexión
    // -------------------------------------------------------------
    [Fact]
    public async Task Test_Connection()
    {
        using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        Assert.Equal(System.Data.ConnectionState.Open, conn.State);
    }

    // -------------------------------------------------------------
    // TEST 2 - Insert + GetById
    // -------------------------------------------------------------
    [Fact]
    public async Task Insert_And_GetById_Should_Work()
    {
        var entity = CreateSampleEntity();

        await _repo.AddAsync(entity);
        _insertedIds.Add(entity.Id);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal(entity.FiscalYear, result!.FiscalYear);
        Assert.Equal(entity.Performance, result.Performance);
    }

    // -------------------------------------------------------------
    // TEST 3 - Update
    // -------------------------------------------------------------
    [Fact]
    public async Task Update_Should_Modify_Entity()
    {
        var entity = CreateSampleEntity();
        await _repo.AddAsync(entity);
        _insertedIds.Add(entity.Id);

        entity.CurrencyMix = 777m;

        await _repo.UpdateAsync(entity);
        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal(777m, result!.CurrencyMix);
    }

    // -------------------------------------------------------------
    // TEST 4 - Delete
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
    // Helper: entidad válida
    // -------------------------------------------------------------
    private DataBridgesFyBw CreateSampleEntity()
    {
        return new DataBridgesFyBw
        {
            IdAPICarga = 10,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.UtcNow,
            IdCompany = 50,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdCurrency = 1,
            IdEpigrafe = 1,

            FiscalYear = 100m,
            Percentage = 10m,
            Zero = 5m,
            ZeroPercentage = 2m,
            Absolute = 200m,
            AbsolutePercentage = 20m,
            VMixNew = 50m,
            RawMaterial = 30m,
            Scrap = 5m,
            Economics = 12m,
            CurrencyMix = 8m,
            Performance = 15m,
            ProtoTool = 6m,
            Others = 7m,
            Comments = "Test Data FYBW",

            IdCarga = null,
            IdCargaSTGBW = null,
            IdHoja = null
        };
    }
}
