using Xunit;
using Npgsql;
using Dapper;
using B4.Models.Entities;
using B4.Data.PostgreSQL.Repositories;

namespace B4.Tests.PostgreSQLTests.Data
{
    public class DataForecastBwRepositoryTests : IDisposable
    {
        private readonly DataForecastBwRepository _repo;
        private readonly List<int> _insertedIds = new();

        public DataForecastBwRepositoryTests()
        {
            _repo = new DataForecastBwRepository(TestConfig.Conn);
        }

        public void Dispose()
        {
            if (_insertedIds.Count > 0)
            {
                using var conn = new NpgsqlConnection(TestConfig.Conn);
                conn.Execute("DELETE FROM b4.data_forecast_bw WHERE id = ANY(@Ids)",
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

            entity.Mes02 = 777m;

            await _repo.UpdateAsync(entity);

            var result = await _repo.GetByIdAsync(entity.Id);

            Assert.NotNull(result);
            Assert.Equal(777m, result!.Mes02);
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

        private DataForecastBw Sample()
        {
            return new DataForecastBw
            {
                IdAPICarga = 1,
                GuidCarga = Guid.NewGuid(),
                FechaUltModif = DateTime.UtcNow,
                IdCompany = 20,
                Ejercicio = 2024,
                IdCiclo = 1,
                IdFase = 1,
                IdCurrency = 1,
                IdEpigrafe = 1,
                Mes00 = 10,
                Mes01 = 11,
                Mes02 = 12,
                Mes03 = 13,
                Mes04 = 14,
                Mes05 = 15,
                Mes06 = 16,
                Mes07 = 17,
                Mes08 = 18,
                Mes09 = 19,
                Mes10 = 20,
                Mes11 = 21,
                Mes12 = 22,
                Mes13 = 23,
                IdCarga = null,
                IdCargaSTGBW = null,
                IdHoja = null
            };
        }
    }
}
