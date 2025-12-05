namespace B4.Tests.MySQLTests.LK;

using B4.Data.MySQL;
using Microsoft.Extensions.Configuration;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using B4.Data.MySQL.Repositories.LkRepositories;
using B4.Models.Entities.LkEntities;

public class PlantillasBotonesPasosTiposRepositoryTest : IDisposable
{
    private readonly MySQLDapperContext _context;
    private readonly PlantillasBotonesPasosTiposRepository _repository;

    private readonly List<int> _insertedIds = new();

    public PlantillasBotonesPasosTiposRepositoryTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new MySQLDapperContext(config);
        _repository = new PlantillasBotonesPasosTiposRepository(_context);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0) return;

        using var conn = _context.CreateConnection();
        conn.Execute(
            "DELETE FROM LK_PLANTILLAS_BOTONES_PASOS_TIPOS WHERE idPasoTipo IN @ids",
            new { ids = _insertedIds });

        _insertedIds.Clear();
    }

    // -------------------------------------------------------------------
    // TESTS
    // -------------------------------------------------------------------

    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var entity = new LkPlantillasBotonesPasosTipos
        {
            IdPasoTipo = 99999,
            Pasotipo = "TIPO1",
            Descripcion = "Descripción de prueba"
        };

        _insertedIds.Add(entity.IdPasoTipo);

        await _repository.AddAsync(entity);

        using var conn = _context.CreateConnection();
        var result = conn.QuerySingleOrDefault<LkPlantillasBotonesPasosTipos>(
            "SELECT * FROM LK_PLANTILLAS_BOTONES_PASOS_TIPOS WHERE idPasoTipo = 99999");

        Assert.NotNull(result);
        Assert.Equal("TIPO1", result.Pasotipo);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANTILLAS_BOTONES_PASOS_TIPOS (idPasoTipo, Pasotipo, descripcion)
                VALUES (99999, 'TIPO2', 'Prueba GetById');
            ");
        }

        _insertedIds.Add(99999);

        var result = await _repository.GetByIdAsync(99999);

        Assert.NotNull(result);
        Assert.Equal("TIPO2", result.Pasotipo);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANTILLAS_BOTONES_PASOS_TIPOS VALUES
                (99999, 'TIPO3', 'Desc 3'),
                (99998, 'TIPO4', 'Desc 4');
            ");
        }

        _insertedIds.AddRange(new[] { 99999, 99998 });

        var result = await _repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.True(result.Count() >= 2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANTILLAS_BOTONES_PASOS_TIPOS VALUES
                (99999, 'ORIGINAL', 'Desc original');
            ");
        }

        _insertedIds.Add(99999);

        var updated = new LkPlantillasBotonesPasosTipos
        {
            IdPasoTipo = 99999,
            Pasotipo = "UPDATED",
            Descripcion = "Desc actualizada"
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<LkPlantillasBotonesPasosTipos>(
            "SELECT * FROM LK_PLANTILLAS_BOTONES_PASOS_TIPOS WHERE idPasoTipo = 99999");

        Assert.Equal("UPDATED", result.Pasotipo);
        Assert.Equal("Desc actualizada", result.Descripcion);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANTILLAS_BOTONES_PASOS_TIPOS VALUES
                (99999, 'DEL', 'Desc DEL');
            ");
        }

        _insertedIds.Add(99999);

        await _repository.DeleteAsync(99999);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<LkPlantillasBotonesPasosTipos>(
            "SELECT * FROM LK_PLANTILLAS_BOTONES_PASOS_TIPOS WHERE idPasoTipo = 99999");

        Assert.Null(result);
    }
}

