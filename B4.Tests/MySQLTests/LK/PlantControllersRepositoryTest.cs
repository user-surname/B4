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

public class PlantControllersRepositoryTest : IDisposable
{
    private readonly MySQLDapperContext _context;
    private readonly PlantControllersRepository _repository;

    private readonly List<int> _insertedIds = new();

    public PlantControllersRepositoryTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new MySQLDapperContext(config);
        _repository = new PlantControllersRepository(_context);
    }

    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = _context.CreateConnection();
        conn.Execute(
            "DELETE FROM LK_PLANT_CONTROLLERS WHERE idCompanyController IN @ids",
            new { ids = _insertedIds });

        _insertedIds.Clear();
    }

    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var controller = new LkPlantControllers
        {
            IdCompanyController = 99999,
            IdCompany = 1,
            Controller = "Controller Test",
            Email = "test@example.com"
        };

        _insertedIds.Add(controller.IdCompanyController);

        await _repository.AddAsync(controller);

        using var conn = _context.CreateConnection();
        var result = conn.QuerySingleOrDefault<LkPlantControllers>(
            "SELECT * FROM LK_PLANT_CONTROLLERS WHERE idCompanyController = 99999");

        Assert.NotNull(result);
        Assert.Equal("Controller Test", result.Controller);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_CONTROLLERS
                (idCompanyController, IdCompany, Controller, Email)
                VALUES (99999, 1, 'ById Controller', 'byid@example.com');");
        }

        _insertedIds.Add(99999);

        var result = await _repository.GetByIdAsync(99999);

        Assert.NotNull(result);
        Assert.Equal("ById Controller", result.Controller);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_CONTROLLERS VALUES
                (99999, 1, 'A', 'a@example.com'),
                (99998, 2, 'B', 'b@example.com');");
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
                INSERT INTO LK_PLANT_CONTROLLERS
                VALUES (99999, 1, 'Original', 'original@example.com');");
        }

        _insertedIds.Add(99999);

        var updated = new LkPlantControllers
        {
            IdCompanyController = 99999,
            IdCompany = 2,
            Controller = "Updated",
            Email = "updated@example.com"
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<LkPlantControllers>(
            "SELECT * FROM LK_PLANT_CONTROLLERS WHERE idCompanyController = 99999");

        Assert.Equal("Updated", result.Controller);
        Assert.Equal(2, result.IdCompany);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_PLANT_CONTROLLERS
                VALUES (99999, 1, 'Delete', 'delete@example.com');");
        }

        _insertedIds.Add(99999);

        await _repository.DeleteAsync(99999);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<LkPlantControllers>(
            "SELECT * FROM LK_PLANT_CONTROLLERS WHERE idCompanyController = 99999");

        Assert.Null(result);
    }
}

