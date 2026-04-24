namespace B4.Tests.MySQLTests;

using B4.Data.DataFactory;
using B4.Data.DataFactory.Repositories;
using B4.Models.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class ControlRepositoryTests : IDisposable
{
    private readonly MySQLDapperContext _context;
    private readonly ControlRepository _repository;

    // Lista que almacena los IDs insertados en cada test para borrarlos después
    private readonly List<int> _insertedIds = new();

    public ControlRepositoryTests()
    {
        // Cargar configuración de appsettings.json
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _context = new MySQLDapperContext(config);
        _repository = new ControlRepository(_context);
    }

    // Dispose se ejecuta después de cada test
    // Borra únicamente los registros insertados durante ese test
    public void Dispose()
    {
        if (_insertedIds.Count == 0)
            return;

        using var conn = _context.CreateConnection();
        conn.Execute(
            "DELETE FROM CONTROL WHERE idControl IN @ids",
            new { ids = _insertedIds });

        _insertedIds.Clear();
    }

    // ---------------------------------------------------------
    // TEST: AddAsync
    // ---------------------------------------------------------
    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        // Arrange: crear un registro Control a insertar
        var control = new Control
        {
            IdControl = 99999,
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

        // Registrar ID para limpieza posterior
        _insertedIds.Add(control.IdControl);

        // Act: insertar el registro usando el repositorio
        await _repository.AddAsync(control);

        // Assert: comprobar que el registro existe en la base de datos
        using var conn = _context.CreateConnection();
        var result = conn.QuerySingleOrDefault<Control>(
            "SELECT * FROM CONTROL WHERE idControl = 99999");

        Assert.NotNull(result);
        Assert.Equal(2024, result.Anyo);
        Assert.Equal("Test Insert", result.AddInfo);
    }

    // ---------------------------------------------------------
    // TEST: GetByIdAsync
    // ---------------------------------------------------------
    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        // Arrange: insertar un registro manualmente para probar GetByIdAsync
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO CONTROL 
                (idControl, Anyo, idCiclo, idFaseControl, Inicio, Final, Activo, AddInfo, SendAutEmail, BWReportsMandatory)
                VALUES (99999, 2025, 6, 2, NOW(), NOW(), true, 'Test GetById', false, false);
            ");
        }

        _insertedIds.Add(99999);

        // Act: recuperar el registro por ID
        var result = await _repository.GetByIdAsync(99999);

        // Assert: validar que el registro existe y es correcto
        Assert.NotNull(result);
        Assert.Equal(2025, result.Anyo);
        Assert.Equal("Test GetById", result.AddInfo);
    }

    // ---------------------------------------------------------
    // TEST: GetAllAsync
    // ---------------------------------------------------------
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        // Arrange: insertar varios registros para comprobar GetAllAsync
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO CONTROL (idControl, Anyo, idCiclo, idFaseControl, Inicio, Final)
                VALUES
                (99999, 2020, 1, 1, NOW(), NOW()),
                (99998, 2021, 2, 2, NOW(), NOW());
            ");
        }

        _insertedIds.AddRange(new[] { 99999, 99998 });

        // Act: recuperar todos los registros
        var result = await _repository.GetAllAsync();

        // Assert: validar que hay registros
        Assert.NotNull(result);
        Assert.True(result.Count() >= 2);
    }

    // ---------------------------------------------------------
    // TEST: UpdateAsync
    // ---------------------------------------------------------
    [Fact]
    public async Task UpdateAsync_ShouldModifyRecord()
    {
        // Arrange: insertar registro inicial
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO CONTROL 
                (idControl, Anyo, idCiclo, idFaseControl, Inicio, Final, AddInfo)
                VALUES (99999, 2022, 3, 3, NOW(), NOW(), 'Original');
            ");
        }

        _insertedIds.Add(99999);

        // Modificar el registro insertado
        var updated = new Control
        {
            IdControl = 99999,
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

        // Act: actualizar el registro usando el repositorio
        await _repository.UpdateAsync(updated);

        // Assert: comprobar que los cambios se aplicaron correctamente
        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<Control>(
            "SELECT * FROM CONTROL WHERE idControl = 99999");

        Assert.Equal("Updated", result.AddInfo);
        Assert.Equal(2023, result.Anyo);
    }

    // ---------------------------------------------------------
    // TEST: DeleteAsync
    // ---------------------------------------------------------
    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        // Arrange: insertar registro que se eliminará
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO CONTROL 
                (idControl, Anyo, idCiclo, idFaseControl, Inicio, Final)
                VALUES (99999, 2024, 7, 1, NOW(), NOW());
            ");
        }

        // Registrar ID por si algo falla y queda suelto
        _insertedIds.Add(99999);

        // Act: borrar el registro usando el repositorio
        await _repository.DeleteAsync(99999);

        // Assert: comprobar que ya no existe
        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<Control>(
            "SELECT * FROM CONTROL WHERE idControl = 99999");

        Assert.Null(result);
    }
}
