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
    public class PlantCurrencyRepositoryTests : IDisposable
    {
        private readonly IDbConnectionFactory _context;
        private readonly IDataQueryProvider _queryProvider;
        private readonly PlantCurrencyRepository _repo;
        private readonly List<int> _ids = new();

        public PlantCurrencyRepositoryTests()
        {
            var dataFactoryOptions = new DataFactoryOptions
            {
                Provider = "PostgreSQL",
                PostgreSqlConnectionString = TestConfig.Conn
            };

            var options = Options.Create(dataFactoryOptions);

            _context = new DbConnectionFactory(options);
            _queryProvider = new DataQueryProvider(options);
            _repo = new PlantCurrencyRepository(_context, _queryProvider);
        }

        public void Dispose()
        {
            using var conn = _context.CreateConnection();
            conn.Execute("DELETE FROM b4.lk_plant_currency WHERE idcurrency = ANY(@Ids)",
                new { Ids = _ids.ToArray() });
        }

        [Fact]
        public async Task Test_Connection()
        {
            using var conn = _context.CreateConnection();
            await conn.OpenAsync();
            Assert.Equal(System.Data.ConnectionState.Open, conn.State);
        }

        [Fact]
        public async Task Insert_And_GetById_Should_Work()
        {
            var entity = CreateSample();
            await _repo.AddAsync(entity);
            _ids.Add(entity.IdCurrency);

            var result = await _repo.GetByIdAsync(entity.IdCurrency);

            Assert.NotNull(result);
            Assert.Equal(entity.Currency, result!.Currency);
            Assert.Equal(entity.CurrencyAlias, result.CurrencyAlias);
        }

        [Fact]
        public async Task Update_Should_Work()
        {
            var entity = CreateSample();
            await _repo.AddAsync(entity);
            _ids.Add(entity.IdCurrency);

            entity.CurrencyAlias = "UPD";

            await _repo.UpdateAsync(entity);

            var result = await _repo.GetByIdAsync(entity.IdCurrency);

            Assert.Equal("UPD", result!.CurrencyAlias);
        }

        [Fact]
        public async Task Delete_Should_Work()
        {
            var entity = CreateSample();
            await _repo.AddAsync(entity);
            _ids.Add(entity.IdCurrency);

            await _repo.DeleteAsync(entity.IdCurrency);

            var result = await _repo.GetByIdAsync(entity.IdCurrency);
            Assert.Null(result);
        }

        private LkPlantCurrency CreateSample()
        {
            return new LkPlantCurrency
            {
                IdCurrency = new Random().Next(30000, 99999),
                Currency = "TestCurrency",
                CurrencyAlias = "TC"
            };
        }
    }
}