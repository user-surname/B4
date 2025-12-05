using Xunit;
using Npgsql;
using Dapper;
using B4.Data.PostgreSQL.Repositories;
using B4.Models.Entities.DataEtities;

namespace B4.Tests.PostgreSQLTests.Data
{
    public class DataBudgetRepositoryTests : IDisposable
    {
        private readonly DataBudgetRepository _repo;
        private readonly List<int> _ids = new();

        public DataBudgetRepositoryTests()
        {
            _repo = new DataBudgetRepository(TestConfig.Conn);
        }

        public void Dispose()
        {
            if (_ids.Count > 0)
            {
                using var conn = new NpgsqlConnection(TestConfig.Conn);
                conn.Execute(
                    "DELETE FROM b4.data_budget WHERE id = ANY(@Ids)",
                    new { Ids = _ids.ToArray() }
                );
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
            var e = SampleEntity();

            await _repo.AddAsync(e);
            _ids.Add(e.Id);

            var r = await _repo.GetByIdAsync(e.Id);

            Assert.NotNull(r);
            Assert.Equal(e.Mes01, r!.Mes01);
        }

        [Fact]
        public async Task Update_Should_Work()
        {
            var e = SampleEntity();

            await _repo.AddAsync(e);
            _ids.Add(e.Id);

            e.Mes02 = 999m;

            await _repo.UpdateAsync(e);

            var r = await _repo.GetByIdAsync(e.Id);

            Assert.NotNull(r);
            Assert.Equal(999m, r!.Mes02);
        }

        [Fact]
        public async Task Delete_Should_Work()
        {
            var e = SampleEntity();

            await _repo.AddAsync(e);
            _ids.Add(e.Id);

            await _repo.DeleteAsync(e.Id);

            Assert.Null(await _repo.GetByIdAsync(e.Id));
        }

        private DataBudget SampleEntity()
        {
            return new DataBudget
            {
                IdAPICarga = 1,
                GuidCarga = Guid.NewGuid(),
                FechaUltModif = DateTime.UtcNow,
                IdCompany = 1,
                Ejercicio = 2025,
                IdCiclo = 1,
                IdFase = 1,
                IdCurrency = 1,
                IdEpigrafe = 1,

                Mes00 = 0m,
                Mes01 = 10m,
                Mes02 = 20m,
                Mes03 = 30m,
                Mes04 = 40m,
                Mes05 = 50m,
                Mes06 = 60m,
                Mes07 = 70m,
                Mes08 = 80m,
                Mes09 = 90m,
                Mes10 = 100m,
                Mes11 = 110m,
                Mes12 = 120m,
                Mes13 = 130m
            };
        }
    }
}
