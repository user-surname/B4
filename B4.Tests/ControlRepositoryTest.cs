namespace B4.Tests;

using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

public class ControlRepositoryTests
{
    private readonly DapperContext _context;
    private readonly ControlRepository _repository;

    public ControlRepositoryTests()
    {
        // Cargar configuración desde appsettings.json
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new DapperContext(config);
        _repository = new ControlRepository(_context);

        // Limpiar tabla CONTROL ANTES de cada test
        using var conn = _context.CreateConnection();
        conn.Execute("DELETE FROM CONTROL;");
    }

    // ---------------------------------------------------------
    // ADD
    // ---------------------------------------------------------
    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        var control = new Control
        {
            IdControl = 1,
            Anyo = 2024,
            IdCiclo = 5,
            IdFaseControl = 1,
            Inicio = DateTime.Now.AddDays(-10),
            Final = DateTime.Now.AddDays(10),
            Activo = true,
            AddInfo = "Test Insert",
            SendAutEmail = false,
            BWReportsMandatory = false
        };

        await _repository.AddAsync(control);

        using var conn = _context.CreateConnection();
        var result = conn.QuerySingleOrDefault<Control>(
            "SELECT * FROM CONTROL WHERE idControl = 1");

        Assert.NotNull(result);
        Assert.Equal(2024, result.Anyo);
        Assert.Equal("Test Insert", result.AddInfo);
    }

    // ---------------------------------------------------------
    // GET BY ID
    // ---------------------------------------------------------
    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO CONTROL 
                (idControl, Anyo, idCiclo, idFaseControl, Inicio, Final, Activo, AddInfo, SendAutEmail, BWReportsMandatory)
                VALUES (2, 2025, 6, 2, NOW(), NOW(), true, 'Test GetById', false, false);
            ");
        }

        var result = await _repository.GetByIdAsync(2);

        Assert.NotNull(result);
        Assert.Equal(2025, result.Anyo);
        Assert.Equal("Test GetById", result.AddInfo);
    }

    // ---------------------------------------------------------
    // GET ALL
    // ---------------------------------------------------------
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO CONTROL (idControl, Anyo, idCiclo, idFaseControl, Inicio, Final)
                VALUES
                (3, 2020, 1, 1, NOW(), NOW()),
                (4, 2021, 2, 2, NOW(), NOW());
            ");
        }

        var result = await _repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.True(result.Count() >= 2);
    }

    // ---------------------------------------------------------
    // UPDATE
    // ---------------------------------------------------------
    [Fact]
    public async Task UpdateAsync_ShouldModifyRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO CONTROL 
                (idControl, Anyo, idCiclo, idFaseControl, Inicio, Final, AddInfo)
                VALUES (5, 2022, 3, 3, NOW(), NOW(), 'Original');
            ");
        }

        var updated = new Control
        {
            IdControl = 5,
            Anyo = 2023,
            IdCiclo = 4,
            IdFaseControl = 2,
            Inicio = DateTime.Now.AddDays(-5),
            Final = DateTime.Now.AddDays(5),
            Activo = true,
            AddInfo = "Updated",
            SendAutEmail = true,
            BWReportsMandatory = false
        };

        await _repository.UpdateAsync(updated);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<Control>(
            "SELECT * FROM CONTROL WHERE idControl = 5");

        Assert.Equal("Updated", result.AddInfo);
        Assert.Equal(2023, result.Anyo);
    }

    // ---------------------------------------------------------
    // DELETE
    // ---------------------------------------------------------
    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO CONTROL 
                (idControl, Anyo, idCiclo, idFaseControl, Inicio, Final)
                VALUES (6, 2024, 7, 1, NOW(), NOW());
            ");
        }

        await _repository.DeleteAsync(6);

        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<Control>(
            "SELECT * FROM CONTROL WHERE idControl = 6");

        Assert.Null(result);
    }
}


