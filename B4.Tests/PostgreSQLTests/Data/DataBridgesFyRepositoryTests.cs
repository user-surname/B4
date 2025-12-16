using Xunit;
using Npgsql;
using Dapper;
using B4.Data.PostgreSQL;
using B4.Data.PostgreSQL.Repositories.DataRepositories;
using B4.Models.Entities.DataEntities;

namespace B4.Tests.PostgreSQLTests.Data
{
    public class DataBridgesFyRepositoryTests : IDisposable
    {
        private readonly DataBridgesFyRepository _repo;
        private readonly PostgreSQLDapperContext _context;
        private readonly List<int> _ids = new();

        public DataBridgesFyRepositoryTests()
        {
            _context = new PostgreSQLDapperContext(TestConfig.Configuration);
            _repo = new DataBridgesFyRepository(_context);
        }

        public void Dispose()
        {
            if (_ids.Count == 0)
                return;

            using var conn = new NpgsqlConnection(TestConfig.Conn);
            conn.Execute(
                "DELETE FROM b4.data_bridges_fy WHERE id = ANY(@Ids)",
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
            Assert.Equal(e.FiscalYear, r!.FiscalYear);
            Assert.Equal(e.Economics, r.Economics);
            Assert.Equal(e.CurrencyMix, r.CurrencyMix);
        }

        [Fact]
        public async Task Update_Should_Work()
        {
            var e = Sample();

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
            var e = Sample();

            await _repo.AddAsync(e);
            _ids.Add(e.Id);

            await _repo.DeleteAsync(e.Id);

            Assert.Null(await _repo.GetByIdAsync(e.Id));
        }

        private DataBridgesFy Sample()
        {
            return new DataBridgesFy
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
                Comments = "test record"
            };
        }
    }
}
