namespace B4.Tests.MySQLTests.LK;

using B4.Data.MySQL;
using Microsoft.Extensions.Configuration;
using Dapper;
using B4.Data.MySQL.Repositories.LkRepositories;
using B4.Models.Entities.LkEntities;

public class CiclosRepositoryTest : IDisposable
{
    private readonly MySQLDapperContext _context;
    private readonly CiclosRepository _repository;

    // IDs generados automáticamente por AUTO_INCREMENT
    private readonly List<int> _insertedIds = new();

    public CiclosRepositoryTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new MySQLDapperContext(config);
        _repository = new CiclosRepository(_context);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = _context.CreateConnection();
        conn.Execute("DELETE FROM LK_CICLOS WHERE id IN @ids", new { ids = _insertedIds });
        _insertedIds.Clear();
    }

    // -------------------------------------------------
    // TEST: ADD
    // -------------------------------------------------
    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var ciclo = new LkCiclos
        {
            IdCiclo = 99999,
            Ciclo = "CICLO ADD",
            Descripcion = "DESC ADD"
        };

        await _repository.AddAsync(ciclo);

        int newId;
        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>(@"
            SELECT id FROM LK_CICLOS 
            ORDER BY id DESC LIMIT 1;");
        }

        _insertedIds.Add(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<LkCiclos>("SELECT * FROM LK_CICLOS WHERE id = @id", new { id = newId });

        Assert.NotNull(result);
        Assert.Equal("CICLO ADD", result.Ciclo);
    }

    // -------------------------------------------------
    // TEST: GET BY ID
    // -------------------------------------------------
    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        int newId;

        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>(@"
                 INSERT INTO LK_CICLOS (idCiclo, Ciclo, Descripcion)
                 VALUES (99999, 'CICLO BYID', 'DESC BYID');
                 SELECT LAST_INSERT_ID();
              ");
        }

        _insertedIds.Add(newId);

        var result = await _repository.GetByIdAsync(newId);

        Assert.NotNull(result);
        Assert.Equal("CICLO BYID", result.Ciclo);
    }

    // -------------------------------------------------
    // TEST: GET ALL
    // -------------------------------------------------
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        int id1, id2;

        using (var conn = _context.CreateConnection())
        {
            id1 = conn.ExecuteScalar<int>(@"
            INSERT INTO LK_CICLOS (idCiclo, Ciclo, Descripcion)
            VALUES (99999, 'CICLO A', 'DESC A');
            SELECT LAST_INSERT_ID();
        ");

            id2 = conn.ExecuteScalar<int>(@"
            INSERT INTO LK_CICLOS (idCiclo, Ciclo, Descripcion)
            VALUES (99998, 'CICLO B', 'DESC B');
            SELECT LAST_INSERT_ID();
        ");
        }

        _insertedIds.AddRange(new[] { id1, id2 });

        var result = await _repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.True(result.Count() >= 2);
    }

    // -------------------------------------------------
    // TEST: UPDATE
    // -------------------------------------------------
    [Fact]
    public async Task UpdateAsync_ShouldModifyRecord()
    {
        int newId;

        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>(@"
        INSERT INTO LK_CICLOS (idCiclo, Ciclo, Descripcion)
        VALUES (99999, 'CICLO ORIG', 'DESC ORIG');
        SELECT LAST_INSERT_ID();
    ");
        }

        _insertedIds.Add(newId);

        var updated = new LkCiclos
        {
            Id = newId,
            IdCiclo = 99999,
            Ciclo = "CICLO UPDATED",
            Descripcion = "DESC UPDATED"
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<LkCiclos>("SELECT * FROM LK_CICLOS WHERE id = @id", new { id = newId });

        Assert.Equal("CICLO UPDATED", result.Ciclo);
        Assert.Equal("DESC UPDATED", result.Descripcion);
    }

    // -------------------------------------------------
    // TEST: DELETE
    // -------------------------------------------------
    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        int newId;

        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>(@"
        INSERT INTO LK_CICLOS (idCiclo, Ciclo, Descripcion)
        VALUES (99999, 'CICLO DEL', 'DESC DEL');
        SELECT LAST_INSERT_ID();
    ");
        }

        _insertedIds.Add(newId);

        await _repository.DeleteAsync(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<LkCiclos>(
            "SELECT * FROM LK_CICLOS WHERE id = @id",
            new { id = newId });

        Assert.Null(result);
    }
}
