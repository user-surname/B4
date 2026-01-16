using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Npgsql;
using Dapper;

namespace B4.Tests.Api
{
    public class DataActualsControllerTests
        : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly HttpClient _client;
        private readonly List<int> _idsToCleanup = new();

        public DataActualsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        // --------------------------------------------------
        // TEST 1: GET -> OK + wrapper
        // --------------------------------------------------
        [Fact]
        public async Task Get_Actuals_By_Planta_Ejercicio_Should_Return_Wrapped_Response()
        {
            // Arrange: insertar un registro para que NO sea NotFound
            InsertActualsRow(planta: 50, ejercicio: 2025, epigrafe: 1);

            // Act  ✅ ruta correcta (v1)
            var response = await _client.GetAsync("/api/v1/DataActuals/50/2025/1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();

            // wrapper mínimo
            Assert.Contains("\"coderror\"", json);
            Assert.Contains("\"action\"", json);
            Assert.Contains("\"exectimems\"", json);
            Assert.Contains("\"data\"", json);

            // y que sea coderror 0
            Assert.Contains("\"coderror\":0", json);
        }

        // --------------------------------------------------
        // TEST 2: GET inexistente -> wrapper con error (NotFound)
        // --------------------------------------------------
        [Fact]
        public async Task Get_Actuals_NotFound_Should_Return_Wrapped_Response()
        {
            var response = await _client.GetAsync("/api/v1/DataActuals/9999/1900/9999");

            // Puede devolver 404 si el controller hace NotFound,
            // o 500 si hay excepción en repo/controller y lo captura el middleware.
            Assert.True(
                response.StatusCode == HttpStatusCode.NotFound ||
                response.StatusCode == HttpStatusCode.InternalServerError,
                $"Status inesperado: {(int)response.StatusCode} {response.StatusCode}"
            );

            var json = await response.Content.ReadAsStringAsync();

            // wrapper mínimo
            Assert.Contains("\"coderror\"", json);
            Assert.Contains("\"action\"", json);
            Assert.Contains("\"exectimems\"", json);
            Assert.Contains("\"data\"", json);

            // coderror NO debe ser 0
            Assert.DoesNotContain("\"coderror\":0", json);
        }

        // --------------------------------------------------
        // Helper: inserta fila real en PostgreSQL
        // (si usas MySQL en tests, dímelo y te lo adapto)
        // --------------------------------------------------
        private void InsertActualsRow(int planta, int ejercicio, int epigrafe)
        {
            using var conn = new NpgsqlConnection(TestConfig.Conn);

            var sql = @"
                INSERT INTO b4.data_actuals
                (idapicarga, guidcarga, fechaultmodif,
                 idcompany, ejercicio, idciclo, idfase,
                 idcurrency, idepigrafe,
                 mes00, mes01, mes02, mes03, mes04, mes05, mes06,
                 mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                VALUES
                (1, @Guid, NOW(),
                 @Planta, @Ejercicio, 0, 0,
                 0, @Epigrafe,
                 0, 1, 2, 3, 4, 5, 6,
                 7, 8, 9, 10, 11, 12, 13)
                RETURNING id;";

            var newId = conn.ExecuteScalar<int>(sql, new
            {
                Guid = Guid.NewGuid(),
                Planta = planta,
                Ejercicio = ejercicio,
                Epigrafe = epigrafe
            });

            _idsToCleanup.Add(newId);
        }

        // --------------------------------------------------
        // Limpieza
        // --------------------------------------------------
        public void Dispose()
        {
            if (_idsToCleanup.Count == 0)
                return;

            using var conn = new NpgsqlConnection(TestConfig.Conn);
            conn.Execute(
                "DELETE FROM b4.data_actuals WHERE id = ANY(@Ids)",
                new { Ids = _idsToCleanup.ToArray() }
            );
        }
    }
}
