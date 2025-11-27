namespace B4.Tests.MySQLTests;

using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DataComentariosRepositoryTest : IDisposable
{
    private readonly MySQLDapperContext _context;
    private readonly DataComentariosRepository _repository;

    private readonly List<int> _insertedIds = new();

    public DataComentariosRepositoryTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new MySQLDapperContext(config);
        _repository = new DataComentariosRepository(_context);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0) return;

        using var conn = _context.CreateConnection();
        conn.Execute("DELETE FROM DATA_Comentarios WHERE Id IN @ids", new { ids = _insertedIds });
        _insertedIds.Clear();
    }

    // -------------------------------------------------
    // TEST: ADD
    // -------------------------------------------------
    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var comentario = new DataComentarios
        {
            IdAPICarga = 1,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.Now,
            IdCompany = 1,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdEpigrafe = 1,
            Etiqueta = "TEST ADD",
            Comentario = "Comentario de prueba"
        };

        await _repository.AddAsync(comentario);

        int newId;
        using (var conn = _context.CreateConnection())
        {
            newId = conn.ExecuteScalar<int>("SELECT Id FROM DATA_Comentarios ORDER BY Id DESC LIMIT 1;");
        }

        _insertedIds.Add(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<DataComentarios>("SELECT * FROM DATA_Comentarios WHERE Id = @id", new { id = newId });

        Assert.NotNull(result);
        Assert.Equal("TEST ADD", result.Etiqueta);
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
                INSERT INTO DATA_Comentarios 
                (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdEpigrafe, Etiqueta, Comentario)
                VALUES (1, '00000000-0000-0000-0000-000000000001', NOW(), 1, 2025, 1, 1, 1, 'TEST BYID', 'Comentario by ID');
                SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        var result = await _repository.GetByIdAsync(newId);

        Assert.NotNull(result);
        Assert.Equal("TEST BYID", result.Etiqueta);
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
                INSERT INTO DATA_Comentarios 
                (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdEpigrafe, Etiqueta, Comentario)
                VALUES (1, '00000000-0000-0000-0000-000000000002', NOW(), 1, 2025, 1, 1, 1, 'TEST A', 'Comentario A');
                SELECT LAST_INSERT_ID();
            ");

            id2 = conn.ExecuteScalar<int>(@"
                INSERT INTO DATA_Comentarios 
                (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdEpigrafe, Etiqueta, Comentario)
                VALUES (1, '00000000-0000-0000-0000-000000000003', NOW(), 1, 2025, 1, 1, 1, 'TEST B', 'Comentario B');
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
                INSERT INTO DATA_Comentarios 
                (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdEpigrafe, Etiqueta, Comentario)
                VALUES (1, '00000000-0000-0000-0000-000000000004', NOW(), 1, 2025, 1, 1, 1, 'TEST ORIG', 'Comentario original');
                SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        var updated = new DataComentarios
        {
            Id = newId,
            IdAPICarga = 1,
            GuidCarga = Guid.NewGuid(),
            FechaUltModif = DateTime.Now,
            IdCompany = 1,
            Ejercicio = 2025,
            IdCiclo = 1,
            IdFase = 1,
            IdEpigrafe = 1,
            Etiqueta = "TEST UPDATED",
            Comentario = "Comentario actualizado"
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<DataComentarios>("SELECT * FROM DATA_Comentarios WHERE Id = @id", new { id = newId });

        Assert.Equal("TEST UPDATED", result.Etiqueta);
        Assert.Equal("Comentario actualizado", result.Comentario);
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
                INSERT INTO DATA_Comentarios 
                (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdEpigrafe, Etiqueta, Comentario)
                VALUES (1, '00000000-0000-0000-0000-000000000005', NOW(), 1, 2025, 1, 1, 1, 'TEST DEL', 'Comentario a eliminar');
                SELECT LAST_INSERT_ID();
            ");
        }

        _insertedIds.Add(newId);

        await _repository.DeleteAsync(newId);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<DataComentarios>("SELECT * FROM DATA_Comentarios WHERE Id = @id", new { id = newId });

        Assert.Null(result);
    }
}

