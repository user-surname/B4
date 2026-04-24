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
    public class DataForecastRepositoryTests : IDisposable
    {
        private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
        private readonly DataForecastRepository _repo;
        private readonly List<int> _ids = new();

        public DataForecastRepositoryTests()
        {
            var dataFactoryOptions = new DataFactoryOptions
            {
                Provider = "PostgreSQL",
                PostgreSqlConnectionString = TestConfig.Conn
            };

            var options = Options.Create(dataFactoryOptions);

            _context = new DbConnectionFactory(options);
            _queryProvider = new DataQueryProvider(options);
            _repo = new DataForecastRepository(_context, _queryProvider);
        }

        public void Dispose()
        {
            if (_ids.Count == 0)
                return;

            using var conn = _context.CreateConnection();
            conn.Execute(
                "DELETE FROM b4.data_forecast WHERE id = ANY(@Ids)",
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
            Assert.Equal(e.Mes01, r!.Mes01);
            Assert.Equal(e.Mes05, r.Mes05);
        }

        // TEST 3: update
        [Fact]
        public async Task Update_Should_Work()
        {
            var e = SampleEntity();

            await _repo.AddAsync(e);
            _ids.Add(e.Id);

            e.Mes02 = 777m;

            await _repo.UpdateAsync(e);

            var r = await _repo.GetByIdAsync(e.Id);

            Assert.NotNull(r);
            Assert.Equal(777m, r!.Mes02);
        }

        // TEST 4: delete
        [Fact]
        public async Task Delete_Should_Work()
        {
            var e = SampleEntity();

            await _repo.AddAsync(e);
            _ids.Add(e.Id);

            await _repo.DeleteAsync(e.Id);

            Assert.Null(await _repo.GetByIdAsync(e.Id));
        }

        // ------------------------------------
        // Helper
        // ------------------------------------
        private DataForecast SampleEntity()
        {
            return new DataForecast
            {
                IdAPICarga = 1,
                GuidCarga = Guid.NewGuid(),
                FechaUltModif = DateTime.UtcNow,
                IdCompany = 1,
                Ejercicio = 2025,
                IdCiclo = 1,
                IdFase = 1,
                IdCurrency = 1,
                IdEpigrafe = 1,

                Mes00 = 1m,
                Mes01 = 10m,
                Mes02 = 20m,
                Mes03 = 30m,
                Mes04 = 40m,
                Mes05 = 50m,
                Mes06 = 60m,
                Mes07 = 70m,
                Mes08 = 80m,
                Mes09 = 90m,
                Mes10 = 100m,
                Mes11 = 110m,
                Mes12 = 120m,
                Mes13 = 130m
            };
        }
    }
}
