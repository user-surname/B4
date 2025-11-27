using Xunit;
using Npgsql;
using Dapper;
using B4.Models.Entities;
using B4.Data.PostgreSQL.Repositories;

public class DataComentariosRepositoryTests : IDisposable
{
    private readonly DataComentariosRepository _repo;
    private readonly List<int> _ids = new();

    public DataComentariosRepositoryTests()
    {
        _repo = new DataComentariosRepository(TestConfig.Conn);
    }

    public void Dispose()
    {
        if (_ids.Count > 0)
        {
            using var conn = new NpgsqlConnection(TestConfig.Conn);
            conn.Execute(
                "DELETE FROM b4.data_comentarios WHERE id = ANY(@Ids)",
                new { Ids = _ids.ToArray() }
            );
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
        var e = SampleEntity();

        await _repo.AddAsync(e);

        _ids.Add(e.Id);

        var r = await _repo.GetByIdAsync(e.Id);

        Assert.NotNull(r);
        Assert.Equal(e.Etiqueta, r!.Etiqueta);
        Assert.Equal(e.Comentario, r.Comentario);
    }

    [Fact]
    public async Task Delete_Should_Work()
    {
        var e = SampleEntity();

        await _repo.AddAsync(e);
        _ids.Add(e.Id);

        await _repo.DeleteAsync(e.Id);

        Assert.Null(await _repo.GetByIdAsync(e.Id));
    }

    private DataComentarios SampleEntity()
    {
        return new DataComentarios
        {
            Id = new Random().Next(1000, 999999),
            IdAPICarga = 1,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.UtcNow,
            IdCompany = 1,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdEpigrafe = 100,   // esta propiedad sí existe
            Etiqueta = "TEST_LABEL",
            Comentario = "comentario test"
        };
    }
}
