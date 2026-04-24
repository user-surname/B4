using Xunit;
using Dapper;
using B4.Data.DataFactory;
using B4.Data.DataFactory.Repositories;
using B4.Models.Entities.LkEntities;

using B4.Data.DataFactory.Configuration;
using B4.Data.DataFactory.Connections;
using B4.Data.DataFactory.Providers;
using Microsoft.Extensions.Options;
namespace B4.Tests.PostgreSQLTests.LK
{
    public class PlantillasBotonesPasosTiposRepositoryTests : IDisposable
    {
        private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
        private readonly PlantillasBotonesPasosTiposRepository _repo;
        private readonly List<int> _ids = new();

        public PlantillasBotonesPasosTiposRepositoryTests()
        {
            var dataFactoryOptions = new DataFactoryOptions
            {
                Provider = "PostgreSQL",
                PostgreSqlConnectionString = TestConfig.Conn
            };

            var options = Options.Create(dataFactoryOptions);

            _context = new DbConnectionFactory(options);
            _queryProvider = new DataQueryProvider(options);
            _repo = new PlantillasBotonesPasosTiposRepository(_context, _queryProvider);
        }

        public void Dispose()
        {
            using var conn = _context.CreateConnection();
            conn.Execute(
                "DELETE FROM b4.lk_plantillas_botones_pasos_tipos WHERE idpasotipo = ANY(@Ids)",
                new { Ids = _ids.ToArray() });
        }

        [Fact]
        public async Task Insert_And_GetById_Should_Work()
        {
            var e = CreateSample();
            await _repo.AddAsync(e);
            _ids.Add(e.IdPasoTipo);

            var result = await _repo.GetByIdAsync(e.IdPasoTipo);

            Assert.NotNull(result);
            Assert.Equal(e.Pasotipo, result!.Pasotipo);
            Assert.Equal(e.Descripcion, result.Descripcion);
        }

        [Fact]
        public async Task Update_Should_Work()
        {
            var e = CreateSample();
            await _repo.AddAsync(e);
            _ids.Add(e.IdPasoTipo);

            e.Descripcion = "UPDATED";

            await _repo.UpdateAsync(e);

            var result = await _repo.GetByIdAsync(e.IdPasoTipo);

            Assert.Equal("UPDATED", result!.Descripcion);
        }

        [Fact]
        public async Task Delete_Should_Work()
        {
            var e = CreateSample();
            await _repo.AddAsync(e);
            _ids.Add(e.IdPasoTipo);

            await _repo.DeleteAsync(e.IdPasoTipo);

            Assert.Null(await _repo.GetByIdAsync(e.IdPasoTipo));
        }

        private LkPlantillasBotonesPasosTipos CreateSample()
        {
            return new LkPlantillasBotonesPasosTipos
            {
                IdPasoTipo = new Random().Next(10000, 99999),
                Pasotipo = "PasoTest",
                Descripcion = "Descripcion Test"
            };
        }
    }
}