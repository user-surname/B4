using Xunit;
using Npgsql;
using Dapper;
using B4.Data.PostgreSQL;
using B4.Data.PostgreSQL.Repositories.DataRepositories;
using B4.Models.Entities.DataEntities;

namespace B4.Tests.PostgreSQLTests.Data
{
    public class DataTipoCambioRepositoryTests : IDisposable
    {
        private readonly DataTipoCambioRepository _repo;
        private readonly PostgreSQLDapperContext _context;
        private readonly List<int> _insertedIds = new();

        public DataTipoCambioRepositoryTests()
        {
            _context = new PostgreSQLDapperContext(TestConfig.Configuration);
            _repo = new DataTipoCambioRepository(_context);
        }

        public void Dispose()
        {
            if (_insertedIds.Count == 0)
                return;

            using var conn = new NpgsqlConnection(TestConfig.Conn);
            conn.Execute(
                "DELETE FROM b4.data_tipo_cambio WHERE id = ANY(@Ids)",
                new { Ids = _insertedIds.ToArray() }
            );
        }

        // TEST 1: conexión
        [Fact]
        public async Task Test_Connection()
        {
            using var conn = new NpgsqlConnection(TestConfig.Conn);
            await conn.OpenAsync();

            Assert.Equal(System.Data.ConnectionState.Open, conn.State);
        }

        // TEST 2: insert + get
        [Fact]
        public async Task Insert_And_Get_Should_Work()
        {
            var entity = Sample();

            await _repo.AddAsync(entity);
            _insertedIds.Add(entity.Id);

            var result = await _repo.GetByIdAsync(entity.Id);

            Assert.NotNull(result);
            Assert.Equal(entity.P, result!.P);
        }

        // TEST 3: update
        [Fact]
        public async Task Update_Should_Work()
        {
            var entity = Sample();

            await _repo.AddAsync(entity);
            _insertedIds.Add(entity.Id);

            entity.FC = 555.55m;

            await _repo.UpdateAsync(entity);

            var result = await _repo.GetByIdAsync(entity.Id);

            Assert.NotNull(result);
            Assert.Equal(555.55m, result!.FC);
        }

        // TEST 4: delete
        [Fact]
        public async Task Delete_Should_Work()
        {
            var entity = Sample();

            await _repo.AddAsync(entity);
            _insertedIds.Add(entity.Id);

            await _repo.DeleteAsync(entity.Id);

            var result = await _repo.GetByIdAsync(entity.Id);
            Assert.Null(result);
        }

        // ------------------------------------
        // Helper
        // ------------------------------------
        private DataTipoCambio Sample()
        {
            return new DataTipoCambio
            {
                IdAPICarga = 1,
                GuidCarga = Guid.NewGuid(),
                FechaUltModif = DateTime.UtcNow,
                Ejercicio = 2024,
                IdCurrency = 1,
                CalendarDay = DateTime.UtcNow,
                Mes = 1,
                P = 10.5m,
                FC = 11.5m,
                FB = 12.5m,
                IdCarga = null,
                IdCargaSTGBW = null,
                IdHoja = null
            };
        }
    }
}
