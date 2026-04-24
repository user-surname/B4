using Xunit;
using Npgsql;
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
    public class PlantSubdivisionRepositoryTests : IDisposable
    {
        private readonly PlantSubdivisionRepository _repo;
        private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
        private readonly List<int> _ids = new();

        public PlantSubdivisionRepositoryTests()
        {
            var dataFactoryOptions = new DataFactoryOptions
            {
                Provider = "PostgreSQL",
                PostgreSqlConnectionString = TestConfig.Conn
            };

            var options = Options.Create(dataFactoryOptions);

            _context = new DbConnectionFactory(options);
            _queryProvider = new DataQueryProvider(options);
            _repo = new PlantSubdivisionRepository(_context, _queryProvider);
        }

        public void Dispose()
        {
            using var conn = new NpgsqlConnection(TestConfig.Conn);
            conn.Execute("DELETE FROM b4.lk_plant_subdivision WHERE idsubdivision = ANY(@Ids)",
                new { Ids = _ids.ToArray() });
        }

        [Fact]
        public async Task Insert_And_GetById_Should_Work()
        {
            var entity = CreateSample();
            await _repo.AddAsync(entity);
            _ids.Add(entity.IdSubdivision);

            var result = await _repo.GetByIdAsync(entity.IdSubdivision);

            Assert.NotNull(result);
            Assert.Equal(entity.Subdivision, result!.Subdivision);
        }

        [Fact]
        public async Task Update_Should_Work()
        {
            var entity = CreateSample();
            await _repo.AddAsync(entity);
            _ids.Add(entity.IdSubdivision);

            entity.Subdivision = "UpdatedSubd";

            await _repo.UpdateAsync(entity);

            var result = await _repo.GetByIdAsync(entity.IdSubdivision);

            Assert.Equal("UpdatedSubd", result!.Subdivision);
        }

        [Fact]
        public async Task Delete_Should_Work()
        {
            var entity = CreateSample();
            await _repo.AddAsync(entity);
            _ids.Add(entity.IdSubdivision);

            await _repo.DeleteAsync(entity.IdSubdivision);

            var result = await _repo.GetByIdAsync(entity.IdSubdivision);
            Assert.Null(result);
        }

        private LkPlantSubdivision CreateSample()
        {
            return new LkPlantSubdivision
            {
                IdSubdivision = new Random().Next(20000, 99999),
                Subdivision = "SubdivisionTest"
            };
        }
    }
}