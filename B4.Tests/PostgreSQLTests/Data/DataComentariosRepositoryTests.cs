using Xunit;
using Dapper;
using B4.Data.DataFactory;
using B4.Data.DataFactory.Repositories;
using B4.Models.Entities.DataEntities;

using B4.Data.DataFactory.Configuration;
using B4.Data.DataFactory.Connections;
using B4.Data.DataFactory.Providers;
using Microsoft.Extensions.Options;
namespace B4.Tests.PostgreSQLTests.Data
{
    public class DataComentariosRepositoryTests : IDisposable
    {
        private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
        private readonly DataComentariosRepository _repo;
        private readonly List<int> _ids = new();

        public DataComentariosRepositoryTests()
        {
            var dataFactoryOptions = new DataFactoryOptions
            {
                Provider = "PostgreSQL",
                PostgreSqlConnectionString = TestConfig.Conn
            };

            var options = Options.Create(dataFactoryOptions);

            _context = new DbConnectionFactory(options);
            _queryProvider = new DataQueryProvider(options);
            _repo = new DataComentariosRepository(_context, _queryProvider);
        }

        public void Dispose()
        {
            if (_ids.Count == 0)
                return;

            using var conn = _context.CreateConnection();
            conn.Execute(
                "DELETE FROM b4.data_comentarios WHERE id = ANY(@Ids)",
                new { Ids = _ids.ToArray() }
            );
        }

        // TEST 2: insert + get
        [Fact]
        public async Task Insert_And_Get_Should_Work()
        {
            var e = SampleEntity();

            await _repo.AddAsync(e);
            _ids.Add(e.Id);

            var r = await _repo.GetByIdAsync(e.Id);

            Assert.NotNull(r);
            Assert.Equal(e.Etiqueta, r!.Etiqueta);
            Assert.Equal(e.Comentario, r.Comentario);
        }

        // TEST 3: delete
        [Fact]
        public async Task Delete_Should_Work()
        {
            var e = SampleEntity();

            await _repo.AddAsync(e);
            _ids.Add(e.Id);

            await _repo.DeleteAsync(e.Id);

            Assert.Null(await _repo.GetByIdAsync(e.Id));
        }

        private DataComentarios SampleEntity()
        {
            return new DataComentarios
            {
                IdAPICarga = 1,
                GuidCarga = Guid.NewGuid(),
                FechaUltModif = DateTime.UtcNow,
                IdCompany = 1,
                Ejercicio = 2025,
                IdCiclo = 1,
                IdFase = 1,
                IdEpigrafe = 100,
                Etiqueta = "TEST_LABEL",
                Comentario = "comentario test"
            };
        }
    }
}
