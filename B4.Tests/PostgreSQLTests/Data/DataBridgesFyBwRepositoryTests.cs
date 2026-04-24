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
    public class DataBridgesFyBwRepositoryTests : IDisposable
    {
        private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
        private readonly DataBridgesFyBwRepository _repo;
        private readonly List<int> _ids = new();

        public DataBridgesFyBwRepositoryTests()
        {
            var dataFactoryOptions = new DataFactoryOptions
            {
                Provider = "PostgreSQL",
                PostgreSqlConnectionString = TestConfig.Conn
            };

            var options = Options.Create(dataFactoryOptions);

            _context = new DbConnectionFactory(options);
            _queryProvider = new DataQueryProvider(options);
            _repo = new DataBridgesFyBwRepository(_context, _queryProvider);
        }

        public void Dispose()
        {
            if (_ids.Count == 0)
                return;

            using var conn = _context.CreateConnection();
            conn.Execute(
                "DELETE FROM b4.data_bridges_fy_bw WHERE id = ANY(@Ids)",
                new { Ids = _ids.ToArray() }
            );
        }

        [Fact]
        public async Task Insert_And_Get_Should_Work()
        {
            var e = SampleEntity();

            await _repo.AddAsync(e);
            _ids.Add(e.Id);

            var r = await _repo.GetByIdAsync(e.Id);

            Assert.NotNull(r);
            Assert.Equal(e.FiscalYear, r!.FiscalYear);
            Assert.Equal(e.Economics, r.Economics);
            Assert.Equal(e.CurrencyMix, r.CurrencyMix);
        }

        [Fact]
        public async Task Update_Should_Work()
        {
            var e = SampleEntity();

            await _repo.AddAsync(e);
            _ids.Add(e.Id);

            e.Performance = 999m;
            await _repo.UpdateAsync(e);

            var r = await _repo.GetByIdAsync(e.Id);

            Assert.NotNull(r);
            Assert.Equal(999m, r!.Performance);
        }

        [Fact]
        public async Task Delete_Should_Work()
        {
            var e = SampleEntity();

            await _repo.AddAsync(e);
            _ids.Add(e.Id);

            await _repo.DeleteAsync(e.Id);

            var r = await _repo.GetByIdAsync(e.Id);
            Assert.Null(r);
        }

        private DataBridgesFyBw SampleEntity()
        {
            return new DataBridgesFyBw
            {
                IdAPICarga = 1,
                GuidCarga = Guid.NewGuid(),
                FechaUltModif = DateTime.UtcNow,
                IdCompany = 10,
                Ejercicio = 2025,
                IdCiclo = 1,
                IdFase = 1,
                IdCurrency = 1,
                IdEpigrafe = 1,

                FiscalYear = 100m,
                Percentage = 10m,
                Zero = 5m,
                ZeroPercentage = 5m,
                Absolute = 20m,
                AbsolutePercentage = 20m,
                VMixNew = 1m,
                RawMaterial = 2m,
                Scrap = 1m,
                Economics = 3m,
                CurrencyMix = 0.5m,
                Performance = 7m,
                ProtoTool = 2m,
                Others = 1m,

                Comments = "test record",
                IdCarga = null,
                IdCargaSTGBW = null,
                IdHoja = null
            };
        }
    }
}