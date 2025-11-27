using Xunit;
using Npgsql;
using Dapper;
using B4.Models.Entities;
using B4.Data.PostgreSQL.Repositories;

public class DataBridgesFyRepositoryTests : IDisposable
{
    private readonly DataBridgesFyRepository _repo;
    private readonly List<int> _ids = new();

    public DataBridgesFyRepositoryTests()
    {
        _repo = new DataBridgesFyRepository(TestConfig.Conn);
    }

    public void Dispose()
    {
        if (_ids.Count > 0)
        {
            using var conn = new NpgsqlConnection(TestConfig.Conn);
            conn.Execute(
                "DELETE FROM b4.data_bridges_fy WHERE id = ANY(@Ids)",
                new { Ids = _ids.ToArray() });
        }
    }

    [Fact]
    public async Task Test_Connection()
    {
        using var conn = new NpgsqlConnection(TestConfig.Conn);
        await conn.OpenAsync();

        Assert.Equal(System.Data.ConnectionState.Open, conn.State);
    }

    [Fact]
    public async Task Insert_And_Get_Should_Work()
    {
        var e = Sample();
        await _repo.AddAsync(e);

        _ids.Add(e.Id);

        var r = await _repo.GetByIdAsync(e.Id);

        Assert.NotNull(r);
        Assert.Equal(e.Actuals, r!.Actuals);
    }

    [Fact]
    public async Task Update_Should_Work()
    {
        var e = Sample();
        await _repo.AddAsync(e);

        _ids.Add(e.Id);

        e.ExchangeRate = 5m;

        await _repo.UpdateAsync(e);

        var r = await _repo.GetByIdAsync(e.Id);

        Assert.Equal(5m, r!.ExchangeRate);
    }

    [Fact]
    public async Task Delete_Should_Work()
    {
        var e = Sample();
        await _repo.AddAsync(e);

        _ids.Add(e.Id);

        await _repo.DeleteAsync(e.Id);

        Assert.Null(await _repo.GetByIdAsync(e.Id));
    }

    private DataBridgesFy Sample()
    {
        return new DataBridgesFy
        {
            Id = new Random().Next(500000, 999999),
            IdAPICarga = 1,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.UtcNow,
            IdCompany = 100,
            Ejercicio = 2024,
            IdCiclo = 1,
            IdFase = 1,
            IdCurrency = 1,
            IdEpigrafe = 1,
            Actuals = 50m,
            Budget = 60m,
            Variance = -10m,
            ExchangeRate = 1.1m,
            Volume = 100m,
            Comments = "test"
        };
    }
}
