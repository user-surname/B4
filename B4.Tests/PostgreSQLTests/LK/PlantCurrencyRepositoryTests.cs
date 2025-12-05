using Xunit;
using Npgsql;
using Dapper;
using B4.Data.PostgreSQL;
using B4.Data.PostgreSQL.Repositories;
using B4.Models.Entities.LkEntities;

public class PlantCurrencyRepositoryTests : IDisposable
{
    private readonly PlantCurrencyRepository _repo;
    private readonly List<int> _ids = new();

    public PlantCurrencyRepositoryTests()
    {
        _repo = new PlantCurrencyRepository(new DapperContext(TestConfig.Configuration));
    }

    public void Dispose()
    {
        using var conn = new NpgsqlConnection(TestConfig.Conn);
        conn.Execute("DELETE FROM b4.lk_plant_currency WHERE idcurrency = ANY(@Ids)",
            new { Ids = _ids.ToArray() });
    }

    [Fact]
    public async Task Test_Connection()
    {
        using var conn = new NpgsqlConnection(TestConfig.Conn);
        await conn.OpenAsync();
        Assert.Equal(System.Data.ConnectionState.Open, conn.State);
    }

    [Fact]
    public async Task Insert_And_GetById_Should_Work()
    {
        var entity = CreateSample();
        await _repo.AddAsync(entity);
        _ids.Add(entity.IdCurrency);

        var result = await _repo.GetByIdAsync(entity.IdCurrency);

        Assert.NotNull(result);
        Assert.Equal(entity.Currency, result!.Currency);
        Assert.Equal(entity.CurrencyAlias, result.CurrencyAlias);
    }

    [Fact]
    public async Task Update_Should_Work()
    {
        var entity = CreateSample();
        await _repo.AddAsync(entity);
        _ids.Add(entity.IdCurrency);

        entity.CurrencyAlias = "UPD";

        await _repo.UpdateAsync(entity);

        var result = await _repo.GetByIdAsync(entity.IdCurrency);

        Assert.Equal("UPD", result!.CurrencyAlias);
    }

    [Fact]
    public async Task Delete_Should_Work()
    {
        var entity = CreateSample();
        await _repo.AddAsync(entity);
        _ids.Add(entity.IdCurrency);

        await _repo.DeleteAsync(entity.IdCurrency);

        var result = await _repo.GetByIdAsync(entity.IdCurrency);
        Assert.Null(result);
    }

    private LkPlantCurrency CreateSample()
    {
        return new LkPlantCurrency
        {
            IdCurrency = new Random().Next(30000, 99999),
            Currency = "TestCurrency",
            CurrencyAlias = "TC"
        };
    }
}
