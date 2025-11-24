using Xunit;
using Npgsql;
using Dapper;
using B4.Data.PostgreSQL.Repositories;
using B4.Models.Entities;

public class DataComentariosRepositoryTests : IDisposable
{
    private const string Conn =
        "Host=pg-3105f5b7-elpuig-23d9.b.aivencloud.com;Port=21134;Database=defaultdb;Username=avnadmin;Password=AVNS_D8EOFxPDlpUH6LhFFLw;Ssl Mode=Require;";

    private readonly DataComentariosRepository _repo;

    // Lista de IDs insertados por los tests
    private readonly List<int> _insertedIds = new();

    public DataComentariosRepositoryTests()
    {
        _repo = new DataComentariosRepository(Conn);
    }

    // Este método se ejecuta DESPUÉS de cada test
    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = new NpgsqlConnection(Conn);

        conn.Execute($"DELETE FROM b4.data_comentarios WHERE id = ANY(@Ids)", 
            new { Ids = _insertedIds.ToArray() });

        _insertedIds.Clear();
    }

    // -------------------------------------
    // TESTS
    // -------------------------------------

    [Fact]
    public async Task Test_Connection()
    {
        using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        Assert.Equal(System.Data.ConnectionState.Open, conn.State);
    }

    [Fact]
    public async Task Insert_And_GetById_Should_Work()
    {
        var entity = new DataComentarios
        {
            IdAPICarga = 1,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.UtcNow,
            IdCompany = 100,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdEpigrafe = 1,
            Etiqueta = "TEST",
            Comentario = "Comentario desde test"
        };

        await _repo.AddAsync(entity);

        // Registramos el ID para borrarlo en Dispose()
        _insertedIds.Add(entity.Id);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal("TEST", result!.Etiqueta);
    }

    [Fact]
    public async Task Update_Should_Modify_Entity()
    {
        var entity = new DataComentarios
        {
            IdAPICarga = 1,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.UtcNow,
            IdCompany = 100,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdEpigrafe = 1,
            Etiqueta = "UPDATE_TEST",
            Comentario = "Viejo"
        };

        await _repo.AddAsync(entity);
        _insertedIds.Add(entity.Id);

        entity.Comentario = "Nuevo";

        await _repo.UpdateAsync(entity);

        var result = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal("Nuevo", result!.Comentario);
    }

    [Fact]
    public async Task Delete_Should_Remove_Entity()
    {
        var entity = new DataComentarios
        {
            IdAPICarga = 1,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.UtcNow,
            IdCompany = 100,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdEpigrafe = 1,
            Etiqueta = "DELETE_TEST",
            Comentario = "Eliminar"
        };

        await _repo.AddAsync(entity);

        // No lo registramos porque lo borraremos manualmente
        await _repo.DeleteAsync(entity.Id);

        var result = await _repo.GetByIdAsync(entity.Id);
        Assert.Null(result);
    }
}
