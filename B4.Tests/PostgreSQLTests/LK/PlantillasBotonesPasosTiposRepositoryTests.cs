using Xunit;
using Npgsql;
using Dapper;
using B4.Data.PostgreSQL;
using B4.Data.PostgreSQL.Repositories;
using B4.Models.Entities.LkEntities;

public class PlantillasBotonesPasosTiposRepositoryTests : IDisposable
{
    private readonly PlantillasBotonesPasosTiposRepository _repo;
    private readonly List<int> _ids = new();

    public PlantillasBotonesPasosTiposRepositoryTests()
    {
        _repo = new PlantillasBotonesPasosTiposRepository(new PostgreSQLDapperContext(TestConfig.Configuration));
    }

    public void Dispose()
    {
        using var conn = new NpgsqlConnection(TestConfig.Conn);
        conn.Execute(
            "DELETE FROM b4.lk_plantillas_botones_pasos_tipos WHERE idpasotipo = ANY(@Ids)",
            new { Ids = _ids.ToArray() });
    }

    [Fact]
    public async Task Insert_And_GetById_Should_Work()
    {
        var e = CreateSample();
        await _repo.AddAsync(e);
        _ids.Add(e.IdPasoTipo);

        var result = await _repo.GetByIdAsync(e.IdPasoTipo);

        Assert.NotNull(result);
        Assert.Equal(e.Pasotipo, result!.Pasotipo);
        Assert.Equal(e.Descripcion, result.Descripcion);
    }

    [Fact]
    public async Task Update_Should_Work()
    {
        var e = CreateSample();
        await _repo.AddAsync(e);
        _ids.Add(e.IdPasoTipo);

        e.Descripcion = "UPDATED";

        await _repo.UpdateAsync(e);

        var result = await _repo.GetByIdAsync(e.IdPasoTipo);

        Assert.Equal("UPDATED", result!.Descripcion);
    }

    [Fact]
    public async Task Delete_Should_Work()
    {
        var e = CreateSample();
        await _repo.AddAsync(e);
        _ids.Add(e.IdPasoTipo);

        await _repo.DeleteAsync(e.IdPasoTipo);

        Assert.Null(await _repo.GetByIdAsync(e.IdPasoTipo));
    }

    private LkPlantillasBotonesPasosTipos CreateSample()
    {
        return new LkPlantillasBotonesPasosTipos
        {
            IdPasoTipo = new Random().Next(10000, 99999),
            Pasotipo = "PasoTest",
            Descripcion = "Descripcion Test"
        };
    }
}
