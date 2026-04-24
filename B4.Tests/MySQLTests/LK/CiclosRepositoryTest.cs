using B4.Data.DataFactory.Configuration;
using B4.Data.DataFactory.Connections;
using B4.Data.DataFactory.Providers;
using B4.Data.DataFactory.Repositories;
using B4.Models.Entities.LkEntities;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace B4.Tests.MySQLTests.LK;

public class CiclosRepositoryTest : IDisposable
{
    private readonly IDbConnectionFactory _context;
    private readonly IDataQueryProvider _queryProvider;
    private readonly LkCiclosRepository _repository;
    private readonly List<int> _insertedIds = new();

    public CiclosRepositoryTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = config.GetConnectionString("MySQLConnectionB4Data")
            ?? throw new InvalidOperationException("Connection string 'MySQLConnectionB4Data' not found.");

        var dataFactoryOptions = new DataFactoryOptions
        {
            Provider = "MySQL",
            MySqlMySqlConnectionString = connectionString
        };

        var options = Options.Create(dataFactoryOptions);
        _context = new DbConnectionFactory(options);
        _queryProvider = new DataQueryProvider(options);
        _repository = new LkCiclosRepository(_context, _queryProvider);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = _context.CreateConnection();
        conn.Execute("DELETE FROM LK_CICLOS WHERE id IN @ids", new { ids = _insertedIds });
        _insertedIds.Clear();
    }

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

        using var conn = _context.CreateConnection();
        var newId = conn.ExecuteScalar<int>("SELECT id FROM LK_CICLOS ORDER BY id DESC LIMIT 1;");
        _insertedIds.Add(newId);

        var result = conn.QuerySingleOrDefault<LkCiclos>(
            "SELECT * FROM LK_CICLOS WHERE id = @id",
            new { id = newId });

        Assert.NotNull(result);
        Assert.Equal("CICLO ADD", result!.Ciclo);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        using var conn = _context.CreateConnection();
        var newId = conn.ExecuteScalar<int>(@"
            INSERT INTO LK_CICLOS (idCiclo, Ciclo, Descripcion)
            VALUES (99999, 'CICLO BYID', 'DESC BYID');
            SELECT LAST_INSERT_ID();
        ");
        _insertedIds.Add(newId);

        var result = await _repository.GetByIdAsync(newId);

        Assert.NotNull(result);
        Assert.Equal("CICLO BYID", result!.Ciclo);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        using var conn = _context.CreateConnection();
        var id1 = conn.ExecuteScalar<int>(@"
            INSERT INTO LK_CICLOS (idCiclo, Ciclo, Descripcion)
            VALUES (99999, 'CICLO A', 'DESC A');
            SELECT LAST_INSERT_ID();
        ");
        var id2 = conn.ExecuteScalar<int>(@"
            INSERT INTO LK_CICLOS (idCiclo, Ciclo, Descripcion)
            VALUES (99998, 'CICLO B', 'DESC B');
            SELECT LAST_INSERT_ID();
        ");
        _insertedIds.AddRange(new[] { id1, id2 });

        var result = await _repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.True(result.Count() >= 2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyRecord()
    {
        using var conn = _context.CreateConnection();
        var newId = conn.ExecuteScalar<int>(@"
            INSERT INTO LK_CICLOS (idCiclo, Ciclo, Descripcion)
            VALUES (99999, 'CICLO ORIG', 'DESC ORIG');
            SELECT LAST_INSERT_ID();
        ");
        _insertedIds.Add(newId);

        var updated = new LkCiclos
        {
            Id = newId,
            IdCiclo = 99999,
            Ciclo = "CICLO UPDATED",
            Descripcion = "DESC UPDATED"
        };

        await _repository.UpdateAsync(updated);

        var result = conn.QuerySingle<LkCiclos>(
            "SELECT * FROM LK_CICLOS WHERE id = @id",
            new { id = newId });

        Assert.Equal("CICLO UPDATED", result.Ciclo);
        Assert.Equal("DESC UPDATED", result.Descripcion);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        using var conn = _context.CreateConnection();
        var newId = conn.ExecuteScalar<int>(@"
            INSERT INTO LK_CICLOS (idCiclo, Ciclo, Descripcion)
            VALUES (99999, 'CICLO DEL', 'DESC DEL');
            SELECT LAST_INSERT_ID();
        ");
        _insertedIds.Add(newId);

        await _repository.DeleteAsync(newId);

        var result = conn.QuerySingleOrDefault<LkCiclos>(
            "SELECT * FROM LK_CICLOS WHERE id = @id",
            new { id = newId });

        Assert.Null(result);
    }
}
