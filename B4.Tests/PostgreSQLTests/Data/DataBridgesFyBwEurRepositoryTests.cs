using Xunit;
using Npgsql;
using Dapper;
using B4.Data.DataFactory;
using B4.Data.DataFactory.Repositories;
using B4.Models.Entities.DataEntities;

namespace B4.Tests.PostgreSQLTests.Data
{
    public class DataBridgesFyBwEurRepositoryTests : IDisposable
    {
        private readonly DataBridgesFyBwEurRepository _repo;
        private readonly PostgreSQLDapperContext _context;
        private readonly List<int> _ids = new();

        public DataBridgesFyBwEurRepositoryTests()
        {
            _context = new PostgreSQLDapperContext(TestConfig.Configuration);
            _repo = new DataBridgesFyBwEurRepository(_context);
        }

        public void Dispose()
        {
            if (_ids.Count == 0)
                return;

            using var conn = new NpgsqlConnection(TestConfig.Conn);
            conn.Execute(
                "DELETE FROM b4.data_bridges_fy_bw_eur WHERE id = ANY(@Ids)",
                new { Ids = _ids.ToArray() }
            );
        }

        [Fact]
        public async Task Test_Connection()
        {
            using var conn = new NpgsqlConnection(TestConfig.Conn);
            await conn.OpenAsync();

            Assert.Equal(System.Data.ConnectionState.Open, conn.State);
        }

        [Fact]
        public async Task Insert_And_Get_Should_Work()
        {
            var e = Sample();

            await _repo.AddAsync(e);
            _ids.Add(e.Id);

            var r = await _repo.GetByIdAsync(e.Id);

            Assert.NotNull(r);
            Assert.Equal(e.CurrencyMix, r!.CurrencyMix);
        }

        [Fact]
        public async Task Update_Should_Modify()
        {
            var e = Sample();

            await _repo.AddAsync(e);
            _ids.Add(e.Id);

            e.ExchangeRate = 10m;
            await _repo.UpdateAsync(e);

            var r = await _repo.GetByIdAsync(e.Id);

            Assert.NotNull(r);
            Assert.Equal(10m, r!.ExchangeRate);
        }

        [Fact]
        public async Task Delete_Should_Remove()
        {
            var e = Sample();

            await _repo.AddAsync(e);
            _ids.Add(e.Id);

            await _repo.DeleteAsync(e.Id);

            var r = await _repo.GetByIdAsync(e.Id);
            Assert.Null(r);
        }

        private DataBridgesFyBwEur Sample()
        {
            return new DataBridgesFyBwEur
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

                Actuals = 10m,
                Budget = 12m,
                Variance = -2m,
                Volume = 20m,
                CurrencyMix = 1m,
                ExchangeRate = 1.2m,
                Comments = "test eur"
            };
        }
    }
}
