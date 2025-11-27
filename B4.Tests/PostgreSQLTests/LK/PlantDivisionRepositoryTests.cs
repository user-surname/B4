using Xunit;
using Npgsql;
using Dapper;
using B4.Models.Entities;
using B4.Data.PostgreSQL;
using B4.Data.PostgreSQL.Repositories;

public class PlantDivisionRepositoryTests : IDisposable
{
    private readonly PlantDivisionRepository _repo;
    private readonly List<int> _ids = new();

    public PlantDivisionRepositoryTests()
    {
        _repo = new PlantDivisionRepository(new DapperContext(TestConfig.Configuration));
    }

    public void Dispose()
    {
        using var conn = new NpgsqlConnection(TestConfig.Conn);
        conn.Execute("DELETE FROM b4.lk_plant_division WHERE iddivision = ANY(@Ids)",
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
        _ids.Add(entity.IdDivision);

        var result = await _repo.GetByIdAsync(entity.IdDivision);

        Assert.NotNull(result);
        Assert.Equal(entity.Division, result!.Division);
    }

    [Fact]
    public async Task Update_Should_Work()
    {
        var entity = CreateSample();
        await _repo.AddAsync(entity);
        _ids.Add(entity.IdDivision);

        entity.Division = "UpdatedDivision";

        await _repo.UpdateAsync(entity);

        var result = await _repo.GetByIdAsync(entity.IdDivision);

        Assert.Equal("UpdatedDivision", result!.Division);
    }

    [Fact]
    public async Task Delete_Should_Work()
    {
        var entity = CreateSample();
        await _repo.AddAsync(entity);
        _ids.Add(entity.IdDivision);

        await _repo.DeleteAsync(entity.IdDivision);

        var result = await _repo.GetByIdAsync(entity.IdDivision);
        Assert.Null(result);
    }

    private LkPlantDivision CreateSample()
    {
        return new LkPlantDivision
        {
            IdDivision = new Random().Next(10000, 99999),
            Division = "DivisionTest"
        };
    }
}
