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
    public class PlantDivisionCompanyRepositoryTests : IDisposable
    {
        private readonly PlantDivisionCompanyRepository _repo;
        private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
        private readonly List<int> _ids = new();

        public PlantDivisionCompanyRepositoryTests()
        {
            var dataFactoryOptions = new DataFactoryOptions
            {
                Provider = "PostgreSQL",
                PostgreSqlConnectionString = TestConfig.Conn
            };

            var options = Options.Create(dataFactoryOptions);

            _context = new DbConnectionFactory(options);
            _queryProvider = new DataQueryProvider(options);
            _repo = new PlantDivisionCompanyRepository(_context, _queryProvider);
        }

        public void Dispose()
        {
            using var conn = new NpgsqlConnection(TestConfig.Conn);
            conn.Execute("DELETE FROM b4.lk_plant_division_company WHERE iddivisioncompany = ANY(@Ids)",
                new { Ids = _ids.ToArray() });
        }

        [Fact]
        public async Task Insert_And_GetById_Should_Work()
        {
            var entity = CreateSample();
            await _repo.AddAsync(entity);
            _ids.Add(entity.IdDivisionCompany);

            var result = await _repo.GetByIdAsync(entity.IdDivisionCompany);

            Assert.NotNull(result);
            Assert.Equal(entity.DivisionCompany, result!.DivisionCompany);
        }

        [Fact]
        public async Task Update_Should_Work()
        {
            var entity = CreateSample();
            await _repo.AddAsync(entity);
            _ids.Add(entity.IdDivisionCompany);

            entity.DivisionCompany = "Updated";

            await _repo.UpdateAsync(entity);

            var result = await _repo.GetByIdAsync(entity.IdDivisionCompany);

            Assert.Equal("Updated", result!.DivisionCompany);
        }

        [Fact]
        public async Task Delete_Should_Work()
        {
            var entity = CreateSample();
            await _repo.AddAsync(entity);
            _ids.Add(entity.IdDivisionCompany);

            await _repo.DeleteAsync(entity.IdDivisionCompany);

            var result = await _repo.GetByIdAsync(entity.IdDivisionCompany);
            Assert.Null(result);
        }

        private LkPlantDivisionCompany CreateSample()
        {
            return new LkPlantDivisionCompany
            {
                IdDivisionCompany = new Random().Next(10000, 99999),
                DivisionCompany = "TestDivComp"
            };
        }
    }
}