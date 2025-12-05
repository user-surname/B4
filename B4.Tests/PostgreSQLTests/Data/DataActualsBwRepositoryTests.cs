using Xunit;
using Npgsql;
using Dapper;
using B4.Data.PostgreSQL.Repositories;
using B4.Models.Entities.DataEtities;

namespace B4.Tests.PostgreSQLTests.Data
{
    public class DataActualsBwRepositoryTests : IDisposable
    {
        private readonly DataActualsBwRepository _repo;
        private readonly List<int> _insertedIds = new();

        public DataActualsBwRepositoryTests()
        {
            _repo = new DataActualsBwRepository(TestConfig.Conn);
        }

        public void Dispose()
        {
            if (_insertedIds.Count > 0)
            {
                using var conn = new NpgsqlConnection(TestConfig.Conn);
                conn.Execute("DELETE FROM b4.data_actuals_bw WHERE id = ANY(@Ids)",
                    new { Ids = _insertedIds.ToArray() });
            }
        }

        [Fact]
        public async Task Test_Connection()
        {
            using var conn = new NpgsqlConnection(TestConfig.Conn);
            await conn.OpenAsync();
            Assert.Equal(System.Data.ConnectionState.Open, conn.State);
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
                Mes13 = 140,
                IdCarga = null,
                IdCargaSTGBW = null,
                IdHoja = null
            };
        }
    }
}
