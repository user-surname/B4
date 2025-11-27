using Xunit;
using Npgsql;
using Dapper;
using B4.Models.Entities;
using B4.Data.PostgreSQL;
using B4.Data.PostgreSQL.Repositories;

public class PlantCountryRepositoryTests : IDisposable
{
    private readonly PlantCountryRepository _repo;
    private readonly List<int> _ids = new();

    public PlantCountryRepositoryTests()
    {
        _repo = new PlantCountryRepository(new DapperContext(TestConfig.Configuration));
    }

    public void Dispose()
    {
        using var conn = new NpgsqlConnection(TestConfig.Conn);
        conn.Execute("DELETE FROM b4.lk_plant_country WHERE idcountry = ANY(@Ids)",
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

        _ids.Add(entity.IdCountry);

        var result = await _repo.GetByIdAsync(entity.IdCountry);

        Assert.NotNull(result);
        Assert.Equal(entity.Country, result!.Country);
    }

    [Fact]
    public async Task Update_Should_Work()
    {
        var entity = CreateSample();
        await _repo.AddAsync(entity);
        _ids.Add(entity.IdCountry);

        entity.Country = "UpdatedCountry";

        await _repo.UpdateAsync(entity);

        var result = await _repo.GetByIdAsync(entity.IdCountry);

        Assert.NotNull(result);
        Assert.Equal("UpdatedCountry", result!.Country);
    }

    [Fact]
    public async Task Delete_Should_Work()
    {
        var entity = CreateSample();
        await _repo.AddAsync(entity);
        _ids.Add(entity.IdCountry);

        await _repo.DeleteAsync(entity.IdCountry);

        var result = await _repo.GetByIdAsync(entity.IdCountry);

        Assert.Null(result);
    }

    private LkPlantCountry CreateSample()
    {
        return new LkPlantCountry
        {
            IdCountry = new Random().Next(20000, 99999),
            Country = "TestCountry"
        };
    }
}
