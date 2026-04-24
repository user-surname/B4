using Xunit;
using Npgsql;
using Dapper;
using B4.Data.DataFactory;
using B4.Data.DataFactory.Repositories;
using B4.Models.Entities.LkEntities;

namespace B4.Tests.PostgreSQLTests.LK
{
    public class FasesRepositoryTests : IDisposable
    {
        private readonly FasesRepository _repo;
        private readonly List<int> _ids = new();

        public FasesRepositoryTests()
        {
            _repo = new FasesRepository(new PostgreSQLDapperContext(TestConfig.Configuration));
        }

        public void Dispose()
        {
            using var conn = new NpgsqlConnection(TestConfig.Conn);
            conn.Execute("DELETE FROM b4.lk_fases WHERE idfase = ANY(@Ids)",
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
            var entity = CreateSample();
            await _repo.AddAsync(entity);

            _ids.Add(entity.IdFase);

            var result = await _repo.GetByIdAsync(entity.IdFase);

            Assert.NotNull(result);
            Assert.Equal(entity.Fase, result!.Fase);
            Assert.Equal(entity.FaseAlias, result.FaseAlias);
        }

        [Fact]
        public async Task Update_Should_Modify_Entity()
        {
            var entity = CreateSample();
            await _repo.AddAsync(entity);
            _ids.Add(entity.IdFase);

            entity.Fase = "FActualizada";

            await _repo.UpdateAsync(entity);
            var result = await _repo.GetByIdAsync(entity.IdFase);

            Assert.Equal("FActualizada", result!.Fase);
        }

        [Fact]
        public async Task Delete_Should_Remove_Entity()
        {
            var entity = CreateSample();
            await _repo.AddAsync(entity);
            _ids.Add(entity.IdFase);

            await _repo.DeleteAsync(entity.IdFase);

            var result = await _repo.GetByIdAsync(entity.IdFase);
            Assert.Null(result);
        }

        private LkFases CreateSample()
        {
            return new LkFases
            {
                IdFase = new Random().Next(2000, 9999),
                Fase = "FaseTest",
                FaseAlias = "FT"
            };
        }
    }
}