using B4.Data.DataFactory.Configuration;
using B4.Data.DataFactory.Connections;
using B4.Data.DataFactory.Providers;
using B4.Data.DataFactory.Repositories;
using B4.Models.Entities.LkEntities;
using Dapper;
using Microsoft.Extensions.Options;
using Xunit;

namespace B4.Tests.PostgreSQLTests.LK
{
    public class CiclosRepositoryTests : IDisposable
    {
        private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
        private readonly LkCiclosRepository _repo;
        private readonly List<int> _insertedIds = new();

        public CiclosRepositoryTests()
        {
            var dataFactoryOptions = new DataFactoryOptions
            {
                Provider = "PostgreSQL",
                PostgreSqlConnectionString = TestConfig.Conn
            };

            var options = Options.Create(dataFactoryOptions);
            _context = new DbConnectionFactory(options);
            _queryProvider = new DataQueryProvider(options);
            _repo = new LkCiclosRepository(_context, _queryProvider);
        }

        public void Dispose()
        {
            if (_insertedIds.Count == 0)
                return;

            using var conn = _context.CreateConnection();
            conn.Execute("DELETE FROM b4.lk_ciclos WHERE id = ANY(@Ids)",
                new { Ids = _insertedIds.ToArray() });

            _insertedIds.Clear();
        }

        [Fact]
        public async Task Test_Connection()
        {
            using var conn = _context.CreateConnection();
            await conn.OpenAsync();
            Assert.Equal(System.Data.ConnectionState.Open, conn.State);
        }

        [Fact]
        public async Task Insert_And_GetById_Should_Work()
        {
            var entity = CreateSample();

            await _repo.AddAsync(entity);

            // PostgreSQL SERIAL autoincrement doesn't update entity.Id → fetch max(id)
            using var conn = _context.CreateConnection();
            var id = await conn.ExecuteScalarAsync<int>("SELECT MAX(id) FROM b4.lk_ciclos");
            _insertedIds.Add(id);

            var result = await _repo.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(entity.Ciclo, result!.Ciclo);
            Assert.Equal(entity.Descripcion, result.Descripcion);
        }

        [Fact]
        public async Task Update_Should_Modify_Entity()
        {
            var entity = CreateSample();

            await _repo.AddAsync(entity);
            using var conn = _context.CreateConnection();
            var id = await conn.ExecuteScalarAsync<int>("SELECT MAX(id) FROM b4.lk_ciclos");
            _insertedIds.Add(id);

            var updated = new LkCiclos
            {
                Id = id,
                IdCiclo = entity.IdCiclo,
                Ciclo = "NuevoCiclo",
                Descripcion = "DescripcionActualizada"
            };

            await _repo.UpdateAsync(updated);

            var result = await _repo.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal("NuevoCiclo", result!.Ciclo);
            Assert.Equal("DescripcionActualizada", result.Descripcion);
        }

        [Fact]
        public async Task Delete_Should_Remove_Entity()
        {
            var entity = CreateSample();

            await _repo.AddAsync(entity);
            using var conn = _context.CreateConnection();
            var id = await conn.ExecuteScalarAsync<int>("SELECT MAX(id) FROM b4.lk_ciclos");
            _insertedIds.Add(id);

            await _repo.DeleteAsync(id);

            var result = await _repo.GetByIdAsync(id);
            Assert.Null(result);
        }

        private LkCiclos CreateSample()
        {
            return new LkCiclos
            {
                IdCiclo = 99,
                Ciclo = "TestCiclo",
                Descripcion = "Descripcion de prueba"
            };
        }
    }
}