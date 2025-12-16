using Xunit;
using Npgsql;
using Dapper;
using B4.Data.PostgreSQL;
using B4.Data.PostgreSQL.Repositories.DataRepositories;
using B4.Models.Entities.DataEntities;

public class DataBudgetRepositoryTests : IDisposable
{
    private readonly DataBudgetRepository _repo;
    private readonly PostgreSQLDapperContext _context;
    private readonly List<int> _insertedIds = new();

    public DataBudgetRepositoryTests()
    {
        _context = new PostgreSQLDapperContext(TestConfig.Configuration);
        _repo = new DataBudgetRepository(_context);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = new NpgsqlConnection(TestConfig.Conn);
        conn.Execute(
            "DELETE FROM b4.data_budget WHERE id = ANY(@Ids)",
            new { Ids = _insertedIds.ToArray() }
        );

        _insertedIds.Clear();
    }

    // -------------------------------------------------------------
    // TEST 1: Conexión
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
        Assert.Equal(entity.Mes03, result.Mes03);
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

        entity.Mes05 = 999m;

        await _repo.UpdateAsync(entity);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal(999m, result!.Mes05);
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
    // Helper: entidad válida
    // -------------------------------------------------------------
    private DataBudget CreateSampleEntity()
    {
        return new DataBudget
        {
            IdAPICarga = 1,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.UtcNow,
            IdCompany = 200,
            Ejercicio = 2026,
            IdCiclo = 2,
            IdFase = 3,
            IdCurrency = 1,
            IdEpigrafe = 1,

            Mes00 = 5m,
            Mes01 = 10m,
            Mes02 = 15m,
            Mes03 = 20m,
            Mes04 = 25m,
            Mes05 = 30m,
            Mes06 = 35m,
            Mes07 = 40m,
            Mes08 = 45m,
            Mes09 = 50m,
            Mes10 = 55m,
            Mes11 = 60m,
            Mes12 = 65m,
            Mes13 = 70m
        };
    }
}
