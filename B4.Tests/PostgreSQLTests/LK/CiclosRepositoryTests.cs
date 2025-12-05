using Xunit;
using Npgsql;
using Dapper;
using B4.Data.PostgreSQL;
using B4.Data.PostgreSQL.Repositories;
using B4.Models.Entities.LkEntities;

public class CiclosRepositoryTests : IDisposable
{
    private readonly CiclosRepository _repo;
    private readonly List<int> _insertedIds = new();

    public CiclosRepositoryTests()
    {
        _repo = new CiclosRepository(new DapperContext(TestConfig.Configuration));
    }

    public void Dispose()
    {
        if (_insertedIds.Count > 0)
        {
            using var conn = new NpgsqlConnection(TestConfig.Conn);
            conn.Execute("DELETE FROM b4.lk_ciclos WHERE id = ANY(@Ids)", 
                new { Ids = _insertedIds.ToArray() });
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
    public async Task Insert_And_GetById_Should_Work()
    {
        var entity = CreateSample();

        await _repo.AddAsync(entity);

        // PostgreSQL SERIAL autoincrement doesn't update entity.Id → fetch max(id)
        using var conn = new NpgsqlConnection(TestConfig.Conn);
        var id = await conn.ExecuteScalarAsync<int>("SELECT MAX(id) FROM b4.lk_ciclos");
        _insertedIds.Add(id);

        var result = await _repo.GetByIdAsync(id);

        Assert.NotNull(result);
        Assert.Equal(entity.Ciclo, result!.Ciclo);
        Assert.Equal(entity.Descripcion, result.Descripcion);
    }

    [Fact]
    public async Task Update_Should_Modify_Entity()
    {
        var entity = CreateSample();

        await _repo.AddAsync(entity);
        using var conn = new NpgsqlConnection(TestConfig.Conn);
        var id = await conn.ExecuteScalarAsync<int>("SELECT MAX(id) FROM b4.lk_ciclos");
        _insertedIds.Add(id);

        var updated = new LkCiclos
        {
            Id = id,
            IdCiclo = entity.IdCiclo,
            Ciclo = "NuevoCiclo",
            Descripcion = "DescripcionActualizada"
        };

        await _repo.UpdateAsync(updated);

        var result = await _repo.GetByIdAsync(id);

        Assert.NotNull(result);
        Assert.Equal("NuevoCiclo", result!.Ciclo);
        Assert.Equal("DescripcionActualizada", result.Descripcion);
    }

    [Fact]
    public async Task Delete_Should_Remove_Entity()
    {
        var entity = CreateSample();

        await _repo.AddAsync(entity);
        using var conn = new NpgsqlConnection(TestConfig.Conn);
        var id = await conn.ExecuteScalarAsync<int>("SELECT MAX(id) FROM b4.lk_ciclos");
        _insertedIds.Add(id);

        await _repo.DeleteAsync(id);

        var result = await _repo.GetByIdAsync(id);
        Assert.Null(result);
    }

    private LkCiclos CreateSample()
    {
        return new LkCiclos
        {
            IdCiclo = 99,
            Ciclo = "TestCiclo",
            Descripcion = "Descripcion de prueba"
        };
    }
}
