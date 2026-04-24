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
    public class DataActualsRepositoryTests : IDisposable
    {
        private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
        private readonly DataActualsRepository _repo;
        private readonly List<int> _ids = new();

        public DataActualsRepositoryTests()
        {
            var dataFactoryOptions = new DataFactoryOptions
            {
                Provider = "PostgreSQL",
                PostgreSqlConnectionString = TestConfig.Conn
            };

            var options = Options.Create(dataFactoryOptions);

            _context = new DbConnectionFactory(options);
            _queryProvider = new DataQueryProvider(options);
            _repo = new DataActualsRepository(_context, _queryProvider);
        }

        public void Dispose()
        {
            if (_ids.Count == 0)
                return;

            using var conn = _context.CreateConnection();
            conn.Execute(
                "DELETE FROM b4.data_actuals WHERE id = ANY(@Ids)",
                new { Ids = _ids.ToArray() }
            );
        }

        // TEST 1: Conexión
        [Fact]
        public async Task Test_Connection()
        {
            using var conn = _context.CreateConnection();
            await conn.OpenAsync();

            Assert.Equal(System.Data.ConnectionState.Open, conn.State);
        }

        // TEST 2: Insert + Get
        [Fact]
        public async Task Insert_And_GetById_Should_Work()
        {
            var entity = CreateSampleEntity();

            await _repo.AddAsync(entity);
            _ids.Add(entity.Id);

            var result = await _repo.GetByIdAsync(entity.Id);

            Assert.NotNull(result);
            Assert.Equal(entity.Ejercicio, result!.Ejercicio);
            Assert.Equal(entity.Mes01, result.Mes01);
        }

        // TEST 3: Update
        [Fact]
        public async Task Update_Should_Modify_Entity()
        {
            var entity = CreateSampleEntity();

            await _repo.AddAsync(entity);
            _ids.Add(entity.Id);

            entity.Mes02 = 777m;

            await _repo.UpdateAsync(entity);

            var result = await _repo.GetByIdAsync(entity.Id);

            Assert.NotNull(result);
            Assert.Equal(777m, result!.Mes02);
        }

        // TEST 4: Delete
        [Fact]
        public async Task Delete_Should_Remove_Entity()
        {
            var entity = CreateSampleEntity();

            await _repo.AddAsync(entity);
            _ids.Add(entity.Id);

            await _repo.DeleteAsync(entity.Id);

            var result = await _repo.GetByIdAsync(entity.Id);

            Assert.Null(result);
        }

        // ----------------------------------------------
        // Helper: generar una entidad válida
        // ----------------------------------------------
        private DataActuals CreateSampleEntity()
        {
            return new DataActuals
            {
                IdAPICarga = 1,
                GuidCarga = Guid.NewGuid(),
                FechaUltModif = DateTime.UtcNow,
                IdCompany = 50,
                Ejercicio = 2025,
                IdCiclo = 1,
                IdFase = 1,
                IdCurrency = 1,
                IdEpigrafe = 1,

                Mes00 = 10m,
                Mes01 = 20m,
                Mes02 = 30m,
                Mes03 = 40m,
                Mes04 = 50m,
                Mes05 = 60m,
                Mes06 = 70m,
                Mes07 = 80m,
                Mes08 = 90m,
                Mes09 = 100m,
                Mes10 = 110m,
                Mes11 = 120m,
                Mes12 = 130m,
                Mes13 = 140m
            };
        }
    }
}
