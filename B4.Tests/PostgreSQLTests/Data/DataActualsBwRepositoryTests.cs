using Xunit;
using Npgsql;
using Dapper;
using Microsoft.Extensions.Configuration;
using B4.Data.PostgreSQL;
using B4.Data.PostgreSQL.Repositories.DataRepositories;
using B4.Models.Entities.DataEntities;

namespace B4.Tests.PostgreSQLTests.Data
{
    public class DataActualsBwRepositoryTests : IDisposable
    {
        private readonly DataActualsBwRepository _repo;
        private readonly PostgreSQLDapperContext _context;
        private readonly List<int> _insertedIds = new();

        public DataActualsBwRepositoryTests()
        {
            _context = new PostgreSQLDapperContext(TestConfig.Configuration);
            _repo = new DataActualsBwRepository(_context);
        }

        public void Dispose()
        {
            if (_insertedIds.Count > 0)
            {
                using var conn = _context.CreateConnection();
                conn.Execute(
                    "DELETE FROM b4.data_actuals_bw WHERE id = ANY(@Ids)",
                    new { Ids = _insertedIds.ToArray() }
                );
            }
        }

        [Fact]
        public async Task Test_Connection()
        {
            using var conn = _context.CreateConnection();
            var result = await conn.ExecuteScalarAsync<int>("SELECT 1");
            Assert.Equal(1, result);
        }

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

        [Fact]
        public async Task Update_Should_Work()
        {
            var entity = Sample();

            await _repo.AddAsync(entity);
            _insertedIds.Add(entity.Id);

            entity.Mes02 = 987m;
            await _repo.UpdateAsync(entity);

            var result = await _repo.GetByIdAsync(entity.Id);

            Assert.NotNull(result);
            Assert.Equal(987m, result!.Mes02);
        }

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

        private DataActualsBw Sample()
        {
            return new DataActualsBw
            {
                IdAPICarga = 1,
                GuidCarga = Guid.NewGuid(),
                FechaUltModif = DateTime.UtcNow,
                IdCompany = 10,
                Ejercicio = 2024,
                IdCiclo = 1,
                IdFase = 1,
                IdCurrency = 1,
                IdEpigrafe = 1,
                Mes00 = 10,
                Mes01 = 20,
                Mes02 = 30,
                Mes03 = 40,
                Mes04 = 50,
                Mes05 = 60,
                Mes06 = 70,
                Mes07 = 80,
                Mes08 = 90,
                Mes09 = 100,
                Mes10 = 110,
                Mes11 = 120,
                Mes12 = 130,
                Mes13 = 140
            };
        }
    }
}
