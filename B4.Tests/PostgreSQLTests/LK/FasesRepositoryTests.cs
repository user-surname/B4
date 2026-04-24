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
    public class FasesRepositoryTests : IDisposable
    {
        private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
        private readonly FasesRepository _repo;
        private readonly List<int> _ids = new();

        public FasesRepositoryTests()
        {
            var dataFactoryOptions = new DataFactoryOptions
            {
                Provider = "PostgreSQL",
                PostgreSqlConnectionString = TestConfig.Conn
            };

            var options = Options.Create(dataFactoryOptions);

            _context = new DbConnectionFactory(options);
            _queryProvider = new DataQueryProvider(options);
            _repo = new FasesRepository(_context, _queryProvider);
        }

        public void Dispose()
        {
            using var conn = _context.CreateConnection();
            conn.Execute("DELETE FROM b4.lk_fases WHERE idfase = ANY(@Ids)",
                new { Ids = _ids.ToArray() });
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

            _ids.Add(entity.IdFase);

            var result = await _repo.GetByIdAsync(entity.IdFase);

            Assert.NotNull(result);
            Assert.Equal(entity.Fase, result!.Fase);
            Assert.Equal(entity.FaseAlias, result.FaseAlias);
        }

        [Fact]
        public async Task Update_Should_Modify_Entity()
        {
            var entity = CreateSample();
            await _repo.AddAsync(entity);
            _ids.Add(entity.IdFase);

            entity.Fase = "FActualizada";

            await _repo.UpdateAsync(entity);
            var result = await _repo.GetByIdAsync(entity.IdFase);

            Assert.Equal("FActualizada", result!.Fase);
        }

        [Fact]
        public async Task Delete_Should_Remove_Entity()
        {
            var entity = CreateSample();
            await _repo.AddAsync(entity);
            _ids.Add(entity.IdFase);

            await _repo.DeleteAsync(entity.IdFase);

            var result = await _repo.GetByIdAsync(entity.IdFase);
            Assert.Null(result);
        }

        private LkFases CreateSample()
        {
            return new LkFases
            {
                IdFase = new Random().Next(2000, 9999),
                Fase = "FaseTest",
                FaseAlias = "FT"
            };
        }
    }
}