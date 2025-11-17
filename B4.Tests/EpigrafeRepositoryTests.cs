namespace B4.Tests;
using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Xunit;

public class EpigrafeRepositoryTests
{
    private readonly DapperContext _context;
    private readonly EpigrafeRepository _repository;

    public EpigrafeRepositoryTests()
    {
        // Cargamos configuraci?n desde appsettings.Test.json
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new DapperContext(config);
        _repository = new EpigrafeRepository(_context);

        // Limpiar tabla ANTES de cada test
        using var conn = _context.CreateConnection();
        conn.Execute("DELETE FROM LK_EPIGRAFES;");
    }

    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var epigrafe = new LkEpigrafe
        {
            IdEpigrafe = 1,
            IdPlantilla = 10,
            IdHoja = 20,
            PreEpigrafe = "PRE",
            Epigrafe = "ADD",
            EpigrafeFull = "PRE ADD"
        };

        await _repository.AddAsync(epigrafe);

        using var conn = _context.CreateConnection();
        var result = conn.QuerySingleOrDefault<LkEpigrafe>(
            "SELECT * FROM LK_EPIGRAFES WHERE idEpigrafe = 1");

        Assert.NotNull(result);
        Assert.Equal("ADD", result.Epigrafe);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_EPIGRAFES 
                (idEpigrafe, idPlantilla, idHoja, PreEpigrafe, Epigrafe, EpigrafeFull)
                VALUES (2, 11, 21, 'PRE2', 'BYID', 'PRE2 BYID');
            ");
        }

        var result = await _repository.GetByIdAsync(2);

        Assert.NotNull(result);
        Assert.Equal("BYID", result.Epigrafe);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_EPIGRAFES VALUES 
                (3, 12, 22, 'P3', 'A', 'P3 A'),
                (4, 13, 23, 'P4', 'B', 'P4 B');
            ");
        }

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
                INSERT INTO LK_EPIGRAFES 
                VALUES (5, 14, 24, 'P5', 'ORIGINAL', 'P5 ORIGINAL');
            ");
        }

        var updated = new LkEpigrafe
        {
            IdEpigrafe = 5,
            IdPlantilla = 14,
            IdHoja = 24,
            PreEpigrafe = "P5",
            Epigrafe = "UPDATED",
            EpigrafeFull = "P5 UPDATED"
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<LkEpigrafe>(
            "SELECT * FROM LK_EPIGRAFES WHERE idEpigrafe = 5");

        Assert.Equal("UPDATED", result.Epigrafe);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_EPIGRAFES 
                VALUES (6, 15, 25, 'P6', 'DEL', 'P6 DEL');
            ");
        }

        await _repository.DeleteAsync(6);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<LkEpigrafe>(
            "SELECT * FROM LK_EPIGRAFES WHERE idEpigrafe = 6");

        Assert.Null(result);
    }
}


