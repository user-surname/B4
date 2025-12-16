using Xunit;
using Npgsql;
using Dapper;
using B4.Data.PostgreSQL;
using B4.Data.PostgreSQL.Repositories.DataRepositories;
using B4.Models.Entities.DataEntities;

public class DataBridgesFyRepositoryTests : IDisposable
{
    private readonly DataBridgesFyRepository _repo;
    private readonly PostgreSQLDapperContext _context;
    private readonly List<int> _insertedIds = new();

    public DataBridgesFyRepositoryTests()
    {
        _context = new PostgreSQLDapperContext(TestConfig.Configuration);
        _repo = new DataBridgesFyRepository(_context);
    }

    // 🔥 Se ejecuta después de cada test
    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = new NpgsqlConnection(TestConfig.Conn);
        conn.Execute(
            "DELETE FROM b4.data_bridges_fy WHERE id = ANY(@Ids)",
            new { Ids = _insertedIds.ToArray() }
        );

        _insertedIds.Clear();
    }

    // -------------------------------------------------------------
    // TEST 1: Comprobar conexión básica
    // -------------------------------------------------------------
    [Fact]
    public async Task Test_Connection()
    {
        using var conn = new NpgsqlConnection(TestConfig.Conn);
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
        Assert.Equal(entity.FiscalYear, result.FiscalYear);
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

        entity.Performance = 9999m;

        await _repo.UpdateAsync(entity);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal(9999m, result!.Performance);
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

            FiscalYear = 100000m,
            Percentage = 50m,
            Zero = 0m,
            ZeroPercentage = 0m,
            Absolute = 100000m,
            AbsolutePercentage = 100m,
            VMixNew = 5000m,
            RawMaterial = 2000m,
            Scrap = 100m,
            Economics = 3000m,
            CurrencyMix = 4000m,
            Performance = 2500m,
            ProtoTool = 1500m,
            Others = 800m,
            Comments = "Registro de ejemplo para test",

            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Version = 1,
            Checksum = 1,
            IsZero = 1
        };
    }
}
