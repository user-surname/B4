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
    public class PlantDivisionRepositoryTests : IDisposable
    {
        private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
        private readonly PlantDivisionRepository _repo;
        private readonly List<int> _ids = new();

        public PlantDivisionRepositoryTests()
        {
            var dataFactoryOptions = new DataFactoryOptions
            {
                Provider = "PostgreSQL",
                PostgreSqlConnectionString = TestConfig.Conn
            };

            var options = Options.Create(dataFactoryOptions);

            _context = new DbConnectionFactory(options);
            _queryProvider = new DataQueryProvider(options);
            _repo = new PlantDivisionRepository(_context, _queryProvider);
        }

        public void Dispose()
        {
            using var conn = _context.CreateConnection();
            conn.Execute("DELETE FROM b4.lk_plant_division WHERE iddivision = ANY(@Ids)",
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
            _ids.Add(entity.IdDivision);

            var result = await _repo.GetByIdAsync(entity.IdDivision);

            Assert.NotNull(result);
            Assert.Equal(entity.Division, result!.Division);
        }

        [Fact]
        public async Task Update_Should_Work()
        {
            var entity = CreateSample();
            await _repo.AddAsync(entity);
            _ids.Add(entity.IdDivision);

            entity.Division = "UpdatedDivision";

            await _repo.UpdateAsync(entity);

            var result = await _repo.GetByIdAsync(entity.IdDivision);

            Assert.Equal("UpdatedDivision", result!.Division);
        }

        [Fact]
        public async Task Delete_Should_Work()
        {
            var entity = CreateSample();
            await _repo.AddAsync(entity);
            _ids.Add(entity.IdDivision);

            await _repo.DeleteAsync(entity.IdDivision);

            var result = await _repo.GetByIdAsync(entity.IdDivision);
            Assert.Null(result);
        }

        private LkPlantDivision CreateSample()
        {
            return new LkPlantDivision
            {
                IdDivision = new Random().Next(10000, 99999),
                Division = "DivisionTest"
            };
        }
    }
}