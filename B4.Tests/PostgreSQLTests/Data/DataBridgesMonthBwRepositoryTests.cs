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
    public class DataBridgesMonthBwRepositoryTests : IDisposable
    {
        private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
        private readonly DataBridgesMonthBwRepository _repo;
        private readonly List<int> _ids = new();

        public DataBridgesMonthBwRepositoryTests()
        {
            var dataFactoryOptions = new DataFactoryOptions
            {
                Provider = "PostgreSQL",
                PostgreSqlConnectionString = TestConfig.Conn
            };

            var options = Options.Create(dataFactoryOptions);

            _context = new DbConnectionFactory(options);
            _queryProvider = new DataQueryProvider(options);
            _repo = new DataBridgesMonthBwRepository(_context, _queryProvider);
        }

        public void Dispose()
        {
            if (_ids.Count == 0)
                return;

            using var conn = _context.CreateConnection();

            conn.Execute(
                "DELETE FROM b4.data_bridges_month_bw WHERE id = ANY(@Ids)",
                new { Ids = _ids.ToArray() }
            );
        }

        // TEST 1: conexión
        [Fact]
        public async Task Test_Connection()
        {
            using var conn = _context.CreateConnection();
            await conn.OpenAsync();

            Assert.Equal(System.Data.ConnectionState.Open, conn.State);
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
            Assert.Equal(e.Volume, r!.Volume);
            Assert.Equal(e.InventoryChange, r.InventoryChange);
            Assert.Equal(e.CurrencyMix, r.CurrencyMix);
        }

        // TEST 3: update
        [Fact]
        public async Task Update_Should_Work()
        {
            var e = SampleEntity();

            await _repo.AddAsync(e);
            _ids.Add(e.Id);

            e.ExchangeRate = 9.99m;
            await _repo.UpdateAsync(e);

            var r = await _repo.GetByIdAsync(e.Id);

            Assert.NotNull(r);
            Assert.Equal(9.99m, r!.ExchangeRate);
        }

        // TEST 4: delete
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

        private DataBridgesMonthBw SampleEntity()
        {
            return new DataBridgesMonthBw
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

                Volume = 200m,
                InventoryChange = 15m,
                Mix = 3m,
                New = 5m,
                Economics = 20m,
                QuickSavings = 2m,
                CurrencyMix = 1.5m,
                ExchangeRate = 1.1m,
                RawMaterial = 12m,
                Scrap = 1m,
                IndustrialPerformance = 7m,
                ProtoTooling = 2m,
                Other = 1m,
                Check = 0.5m,
                Comments = "test month"
            };
        }
    }
}
