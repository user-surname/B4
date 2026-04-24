using Xunit;
using Npgsql;
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
    public class DataForecastBwRepositoryTests : IDisposable
    {
        private readonly DataForecastBwRepository _repo;
        private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
        private readonly List<int> _insertedIds = new();

        public DataForecastBwRepositoryTests()
        {
            var dataFactoryOptions = new DataFactoryOptions
            {
                Provider = "PostgreSQL",
                ConnectionString = TestConfig.Conn
            };

            var options = Options.Create(dataFactoryOptions);

            _context = new DbConnectionFactory(options);
            _queryProvider = new DataQueryProvider(options);
            _repo = new DataForecastBwRepository(_context, _queryProvider);
        }

        public void Dispose()
        {
            if (_insertedIds.Count == 0)
                return;

            using var conn = new NpgsqlConnection(TestConfig.Conn);
            conn.Execute(
                "DELETE FROM b4.data_forecast_bw WHERE id = ANY(@Ids)",
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
            Assert.Equal(entity.Mes01, result!.Mes01);
        }

        // TEST 3: update
        [Fact]
        public async Task Update_Should_Work()
        {
            var entity = Sample();

            await _repo.AddAsync(entity);
            _insertedIds.Add(entity.Id);

            entity.Mes02 = 777m;

            await _repo.UpdateAsync(entity);

            var result = await _repo.GetByIdAsync(entity.Id);

            Assert.NotNull(result);
            Assert.Equal(777m, result!.Mes02);
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

        private DataForecastBw Sample()
        {
            return new DataForecastBw
            {
                IdAPICarga = 1,
                GuidCarga = Guid.NewGuid(),
                FechaUltModif = DateTime.UtcNow,
                IdCompany = 20,
                Ejercicio = 2024,
                IdCiclo = 1,
                IdFase = 1,
                IdCurrency = 1,
                IdEpigrafe = 1,

                Mes00 = 10m,
                Mes01 = 11m,
                Mes02 = 12m,
                Mes03 = 13m,
                Mes04 = 14m,
                Mes05 = 15m,
                Mes06 = 16m,
                Mes07 = 17m,
                Mes08 = 18m,
                Mes09 = 19m,
                Mes10 = 20m,
                Mes11 = 21m,
                Mes12 = 22m,
                Mes13 = 23m
            };
        }
    }
}
