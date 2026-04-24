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
    public class PlantTreeRepositoryTests : IDisposable
    {
        private readonly PlantTreeRepository _repo;
        private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
        private readonly List<int> _ids = new();

        public PlantTreeRepositoryTests()
        {
            var dataFactoryOptions = new DataFactoryOptions
            {
                Provider = "PostgreSQL",
                PostgreSqlConnectionString = TestConfig.Conn
            };

            var options = Options.Create(dataFactoryOptions);

            _context = new DbConnectionFactory(options);
            _queryProvider = new DataQueryProvider(options);
            _repo = new PlantTreeRepository(_context, _queryProvider);
        }

        public void Dispose()
        {
            using var conn = new NpgsqlConnection(TestConfig.Conn);
            conn.Execute("DELETE FROM b4.lk_plant_tree WHERE idtree = ANY(@Ids)",
                new { Ids = _ids.ToArray() });
        }

        [Fact]
        public async Task Test_Connection()
        {
            using var conn = new NpgsqlConnection(TestConfig.Conn);
            await conn.OpenAsync();
            Assert.Equal(System.Data.ConnectionState.Open, conn.State);
        }

        [Fact]
        public async Task Insert_And_GetById_Should_Work()
        {
            var e = CreateSample();

            await _repo.AddAsync(e);
            _ids.Add(e.IdTree);

            var result = await _repo.GetByIdAsync(e.IdTree);

            Assert.NotNull(result);
            Assert.Equal(e.IdDivision, result!.IdDivision);
            Assert.Equal(e.IdSubdivision, result.IdSubdivision);
        }

        [Fact]
        public async Task Update_Should_Work()
        {
            var e = CreateSample();
            await _repo.AddAsync(e);
            _ids.Add(e.IdTree);

            e.IdCountry = 99999;

            await _repo.UpdateAsync(e);

            var result = await _repo.GetByIdAsync(e.IdTree);

            Assert.Equal(99999, result!.IdCountry);
        }

        [Fact]
        public async Task Delete_Should_Work()
        {
            var e = CreateSample();
            await _repo.AddAsync(e);
            _ids.Add(e.IdTree);

            await _repo.DeleteAsync(e.IdTree);

            Assert.Null(await _repo.GetByIdAsync(e.IdTree));
        }

        private LkPlantTree CreateSample()
        {
            return new LkPlantTree
            {
                IdTree = new Random().Next(50000, 99999),
                IdDivision = 10,
                IdDivisionCompany = 20,
                IdSubdivision = 30,
                IdCountry = 40
            };
        }
    }
}