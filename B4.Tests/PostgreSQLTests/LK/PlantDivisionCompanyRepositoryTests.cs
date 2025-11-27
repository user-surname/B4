using Xunit;
using Npgsql;
using Dapper;
using B4.Models.Entities;
using B4.Data.PostgreSQL;
using B4.Data.PostgreSQL.Repositories;

public class PlantDivisionCompanyRepositoryTests : IDisposable
{
    private readonly PlantDivisionCompanyRepository _repo;
    private readonly List<int> _ids = new();

    public PlantDivisionCompanyRepositoryTests()
    {
        _repo = new PlantDivisionCompanyRepository(new DapperContext(TestConfig.Configuration));
    }

    public void Dispose()
    {
        using var conn = new NpgsqlConnection(TestConfig.Conn);
        conn.Execute("DELETE FROM b4.lk_plant_division_company WHERE iddivisioncompany = ANY(@Ids)",
            new { Ids = _ids.ToArray() });
    }

    [Fact]
    public async Task Insert_And_GetById_Should_Work()
    {
        var entity = CreateSample();
        await _repo.AddAsync(entity);
        _ids.Add(entity.IdDivisionCompany);

        var result = await _repo.GetByIdAsync(entity.IdDivisionCompany);

        Assert.NotNull(result);
        Assert.Equal(entity.DivisionCompany, result!.DivisionCompany);
    }

    [Fact]
    public async Task Update_Should_Work()
    {
        var entity = CreateSample();
        await _repo.AddAsync(entity);
        _ids.Add(entity.IdDivisionCompany);

        entity.DivisionCompany = "Updated";

        await _repo.UpdateAsync(entity);

        var result = await _repo.GetByIdAsync(entity.IdDivisionCompany);

        Assert.Equal("Updated", result!.DivisionCompany);
    }

    [Fact]
    public async Task Delete_Should_Work()
    {
        var entity = CreateSample();
        await _repo.AddAsync(entity);
        _ids.Add(entity.IdDivisionCompany);

        await _repo.DeleteAsync(entity.IdDivisionCompany);

        var result = await _repo.GetByIdAsync(entity.IdDivisionCompany);
        Assert.Null(result);
    }

    private LkPlantDivisionCompany CreateSample()
    {
        return new LkPlantDivisionCompany
        {
            IdDivisionCompany = new Random().Next(10000, 99999),
            DivisionCompany = "TestDivComp"
        };
    }
}
