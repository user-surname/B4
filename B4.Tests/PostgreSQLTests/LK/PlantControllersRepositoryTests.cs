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
    public class PlantControllersRepositoryTests : IDisposable
    {
        private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
        private readonly PlantControllersRepository _repo;
        private readonly List<int> _ids = new();

        public PlantControllersRepositoryTests()
        {
            var dataFactoryOptions = new DataFactoryOptions
            {
                Provider = "PostgreSQL",
                PostgreSqlConnectionString = TestConfig.Conn
            };

            var options = Options.Create(dataFactoryOptions);

            _context = new DbConnectionFactory(options);
            _queryProvider = new DataQueryProvider(options);
            _repo = new PlantControllersRepository(_context, _queryProvider);
        }

        public void Dispose()
        {
            using var conn = _context.CreateConnection();
            conn.Execute("DELETE FROM b4.lk_plant_controllers WHERE idcompanycontroller = ANY(@Ids)",
                new { Ids = _ids.ToArray() });
        }

        [Fact]
        public async Task Insert_And_GetById_Should_Work()
        {
            var e = CreateSample();
            await _repo.AddAsync(e);
            _ids.Add(e.IdCompanyController);

            var result = await _repo.GetByIdAsync(e.IdCompanyController);

            Assert.NotNull(result);
            Assert.Equal(e.Email, result!.Email);
        }

        [Fact]
        public async Task Update_Should_Work()
        {
            var e = CreateSample();
            await _repo.AddAsync(e);
            _ids.Add(e.IdCompanyController);

            e.Email = "updated@test.com";

            await _repo.UpdateAsync(e);

            var result = await _repo.GetByIdAsync(e.IdCompanyController);

            Assert.Equal("updated@test.com", result!.Email);
        }

        [Fact]
        public async Task Delete_Should_Work()
        {
            var e = CreateSample();
            await _repo.AddAsync(e);
            _ids.Add(e.IdCompanyController);

            await _repo.DeleteAsync(e.IdCompanyController);

            Assert.Null(await _repo.GetByIdAsync(e.IdCompanyController));
        }

        private LkPlantControllers CreateSample()
        {
            return new LkPlantControllers
            {
                IdCompanyController = new Random().Next(1000, 9999),
                IdCompany = 321,
                Controller = "John Doe",
                Email = "controller@test.com"
            };
        }
    }
}