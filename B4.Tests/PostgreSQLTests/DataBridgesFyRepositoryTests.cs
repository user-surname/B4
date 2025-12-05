using Xunit;
using Npgsql;
using Dapper;
using B4.Data.PostgreSQL.Repositories;
using B4.Models.Entities.DataEtities;

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

        entity.IdCompany = 9;

        await _repo.UpdateAsync(entity);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal(9, result!.IdCompany);
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
            IdCompany = 100,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdCurrency = 1,
            IdEpigrafe = 200,
            FiscalYear = 100000.00m,
            Percentage = 50.00m,
            Zero = 0.00m,
            ZeroPercentage = 0.00m,
            Absolute = 100000.00m,
            AbsolutePercentage = 100.00m,
            VMixNew = 5000.00m,
            RawMaterial = 2000.00m,
            Scrap = 100.00m,
            Economics = 3000.00m,
            CurrencyMix = 4000.00m,
            Performance = 2500.00m,
            ProtoTool = 1500.00m,
            Others = 800.00m,
            Comments = "Registro de ejemplo para test",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Version = 1,
            Checksum = null,
            IsZero = 1
        };
    }
}
