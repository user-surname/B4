using Xunit;
using Npgsql;
using Dapper;
using B4.Data.PostgreSQL;
using B4.Data.PostgreSQL.Repositories;
using B4.Models.Entities.LkEntities;

namespace B4.Tests.PostgreSQLTests.LK
{
    public class PlantControllersRepositoryTests : IDisposable
    {
        private readonly PlantControllersRepository _repo;
        private readonly List<int> _ids = new();

        public PlantControllersRepositoryTests()
        {
            _repo = new PlantControllersRepository(new PostgreSQLDapperContext(TestConfig.Configuration));
        }

        public void Dispose()
        {
            using var conn = new NpgsqlConnection(TestConfig.Conn);
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