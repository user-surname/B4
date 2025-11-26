namespace B4.Tests.MySQLTests;

using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;

public class EpigrafeRepositoryTest : IDisposable
{
    private readonly MySQLDapperContext _context;
    private readonly EpigrafeRepository _repository;

    // Lista que almacena los IDs que cada test inserta en la BD.
    // Luego seran borrados automaticamente en Dispose().
    private readonly List<int> _insertedIds = new();

    public EpigrafeRepositoryTest()
    {
        // Cargar configuracion desde appsettings.json (cadena de conexion de test)
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        // Crear el contexto Dapper (MySQL)
        _context = new MySQLDapperContext(config);

        // Crear instancia del repositorio a testear
        _repository = new EpigrafeRepository(_context);
    }

    // Metodo que se ejecuta DESPUES de cada test.
    // El proposito es limpiar SOLO los registros insertados durante ese test.
    public void Dispose()
    {
        // Si no insertamos nada, no hay que borrar nada
        if (_insertedIds.Count == 0)
            return;

        using var conn = _context.CreateConnection();

        // Borramos unicamente los IDs registrados en este test
        conn.Execute(
            "DELETE FROM LK_EPIGRAFES WHERE idEpigrafe IN @ids",
            new { ids = _insertedIds });

        // Limpieza de la lista para el proximo test
        _insertedIds.Clear();
    }

    // -------------------------------------------------------------------
    // TESTS
    // -------------------------------------------------------------------

    [Fact]
    public async Task AddAsync_ShouldInsertRecord()
    {
        // Arrange: creamos un epigrafe a insertar
        var epigrafe = new LkEpigrafe
        {
            IdEpigrafe = 99999, // ID fijo para pruebas
            IdPlantilla = 10,
            IdHoja = 20,
            PreEpigrafe = "PRE",
            Epigrafe = "ADD",
            EpigrafeFull = "PRE ADD"
        };

        // Registramos el ID para borrarlo al final del test
        _insertedIds.Add(99999);

        // Act: llamamos al metodo del repositorio
        await _repository.AddAsync(epigrafe);

        // Assert: verificamos que el registro existe en la BD
        using var conn = _context.CreateConnection();
        var result = conn.QuerySingleOrDefault<LkEpigrafe>(
            "SELECT * FROM LK_EPIGRAFES WHERE idEpigrafe = 99999");

        Assert.NotNull(result);
        Assert.Equal("ADD", result.Epigrafe);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord()
    {
        // Arrange: insertamos un registro manualmente
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_EPIGRAFES 
                (idEpigrafe, idPlantilla, idHoja, PreEpigrafe, Epigrafe, EpigrafeFull)
                VALUES (99999, 11, 21, 'PRE2', 'BYID', 'PRE2 BYID');
            ");
        }

        _insertedIds.Add(99999);

        // Act: obtener el registro por ID
        var result = await _repository.GetByIdAsync(99999);

        // Assert: validar que el registro existe y es correcto
        Assert.NotNull(result);
        Assert.Equal("BYID", result.Epigrafe);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecords()
    {
        // Arrange: insertamos varios registros para comprobar GetAll
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_EPIGRAFES VALUES 
                (99999, 12, 22, 'P3', 'A', 'P3 A'),
                (99998, 13, 23, 'P4', 'B', 'P4 B');
            ");
        }

        // Registrar IDs para limpieza posterior
        _insertedIds.AddRange(new[] { 99999, 99998 });

        // Act: recuperar todos los registros
        var result = await _repository.GetAllAsync();

        // Assert: comprobar que hay datos
        Assert.NotNull(result);
        Assert.True(result.Count() >= 2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyRecord()
    {
        // Arrange: insertamos un registro inicial
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_EPIGRAFES 
                VALUES (99999, 14, 24, 'P5', 'ORIGINAL', 'P5 ORIGINAL');
            ");
        }

        // Registrar ID para eliminar luego
        _insertedIds.Add(99999);

        // Modify el registro insertado
        var updated = new LkEpigrafe
        {
            IdEpigrafe = 99999,
            IdPlantilla = 14,
            IdHoja = 24,
            PreEpigrafe = "P5",
            Epigrafe = "UPDATED",
            EpigrafeFull = "P5 UPDATED"
        };

        // Act: actualizar el registro
        await _repository.UpdateAsync(updated);

        // Assert: validar que se guardaron los cambios
        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingle<LkEpigrafe>(
            "SELECT * FROM LK_EPIGRAFES WHERE idEpigrafe = 99999");

        Assert.Equal("UPDATED", result.Epigrafe);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecord()
    {
        // Arrange: insertamos un registro que luego ser? eliminado
        using (var conn = _context.CreateConnection())
        {
            conn.Execute(@"
                INSERT INTO LK_EPIGRAFES 
                VALUES (99999, 15, 25, 'P6', 'DEL', 'P6 DEL');
            ");
        }

        // Registrar ID por si algo falla y el registro queda suelto
        _insertedIds.Add(99999);

        await _repository.DeleteAsync(99999);

        // Assert: comprobar que ya no existe en la BD
        using var conn2 = _context.CreateConnection();
        var result = conn2.QuerySingleOrDefault<LkEpigrafe>(
            "SELECT * FROM LK_EPIGRAFES WHERE idEpigrafe = 99999");

        Assert.Null(result);
    }
}