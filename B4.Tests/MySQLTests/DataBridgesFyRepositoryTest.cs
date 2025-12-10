using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace B4.Tests.MySQLTests
{
    public class DataBridgesFyRepositoryTest : IDisposable
    {
        private readonly MySQLDapperContext _context;
        private readonly DataBridgesFyRepository _repository;

        private readonly List<int> _insertedIds = new();

        public DataBridgesFyRepositoryTest()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _context = new MySQLDapperContext(config);
            _repository = new DataBridgesFyRepository(_context);
        }

        public void Dispose()
        {
            if (_insertedIds.Count == 0) return;

            using var conn = _context.CreateConnection();
            conn.Execute("DELETE FROM DATA_BridgesFY WHERE Id IN @ids", new { ids = _insertedIds });
        }

        // -------------------------------------------------
        // TEST: ADD
        // -------------------------------------------------
        [Fact]
        public async Task AddAsync_ShouldInsertRecord()
        {
            var entity = new DataBridgesFy
            {
                IdAPICarga = 1,
                GuidCarga = Guid.NewGuid(),
                FechaUltModif = DateTime.Now,
                IdCompany = 1,
                Ejercicio = 2025,
                IdCiclo = 1,
                IdFase = 1,
                IdCurrency = 1,
                IdEpigrafe = 1,

                Actuals = 100,
                PctActuals = 10,
                Budget = 90,
                PctBudget = 9,
                Variance = 10,
                Volume = 5,
                InventoryChange = 2,
                Mix = 1,
                New = 3,
                Economics = 4,
                QuickSavings = 2,
                CurrencyMix = 1,
                ExchangeRate = 1,
                RawMaterial = 3,
                Scrap = 2,
                IndustrialPerformance = 4,
                ProtoTooling = 1,
                Other = 2,
                Check = 1,
                Comments = "Test add"
            };

            await _repository.AddAsync(entity);

            using var conn = _context.CreateConnection();
            int newId = conn.ExecuteScalar<int>(
                "SELECT Id FROM DATA_BridgesFY ORDER BY Id DESC LIMIT 1");

            _insertedIds.Add(newId);

            var result = conn.QuerySingle<DataBridgesFy>(
                "SELECT * FROM DATA_BridgesFY WHERE Id=@id", new { id = newId });

            Assert.NotNull(result);
            Assert.Equal(100, result.Actuals);
        }

        // -------------------------------------------------
        // TEST: GET BY ID
        // -------------------------------------------------
        [Fact]
        public async Task GetByIdAsync_ShouldReturnRecord()
        {
            int newId;

            using (var conn = _context.CreateConnection())
            {
                newId = conn.ExecuteScalar<int>(@"
                    INSERT INTO DATA_BridgesFY
                    (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                     Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, New,
                     Economics, QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap,
                     IndustrialPerformance, ProtoTooling, Other, `Check`, Comments)
                    VALUES
                    (1, UUID(), NOW(), 1, 2025, 1,1,1,1,
                     200, 20, 180, 18, 20, 5,2,1,3,
                     4,2,1,1,3,2,
                     4,1,2,1,'Test byId');
                    SELECT LAST_INSERT_ID();
                ");
            }

            _insertedIds.Add(newId);

            var result = await _repository.GetByIdAsync(newId);

            Assert.NotNull(result);
            Assert.Equal(200, result!.Actuals);
        }

        // -------------------------------------------------
        // TEST: GET ALL
        // -------------------------------------------------
        [Fact]
        public async Task GetAllAsync_ShouldReturnRecords()
        {
            int id1, id2;

            using (var conn = _context.CreateConnection())
            {
                id1 = conn.ExecuteScalar<int>(@"
                    INSERT INTO DATA_BridgesFY
                    (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                     Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, New,
                     Economics, QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap,
                     IndustrialPerformance, ProtoTooling, Other, `Check`, Comments)
                    VALUES
                    (1, UUID(), NOW(), 1,2025,1,1,1,1,
                     150,15,100,10,50,5,2,1,3,
                     4,2,1,1,3,2,
                     4,1,2,1,'TestA');
                    SELECT LAST_INSERT_ID();
                ");

                id2 = conn.ExecuteScalar<int>(@"
                    INSERT INTO DATA_BridgesFY
                    (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                     Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, New,
                     Economics, QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap,
                     IndustrialPerformance, ProtoTooling, Other, `Check`, Comments)
                    VALUES
                    (2, UUID(), NOW(), 1,2025,1,1,1,1,
                     250,25,200,20,50,5,2,1,3,
                     4,2,1,1,3,2,
                     4,1,2,1,'TestB');
                    SELECT LAST_INSERT_ID();
                ");
            }

            _insertedIds.AddRange(new[] { id1, id2 });

            var result = await _repository.GetAllAsync();

            Assert.NotNull(result);
            Assert.True(result.Count() >= 2);
        }

        // -------------------------------------------------
        // TEST: UPDATE
        // -------------------------------------------------
        [Fact]
        public async Task UpdateAsync_ShouldModifyRecord()
        {
            int id;

            using (var conn = _context.CreateConnection())
            {
                id = conn.ExecuteScalar<int>(@"
                    INSERT INTO DATA_BridgesFY
                    (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                     Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, New,
                     Economics, QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap,
                     IndustrialPerformance, ProtoTooling, Other, `Check`, Comments)
                    VALUES
                    (1, UUID(), NOW(), 1,2025,1,1,1,1,
                     300,30,200,20,100,5,2,1,3,
                     4,2,1,1,3,2,
                     4,1,2,1,'Test update');
                    SELECT LAST_INSERT_ID();
                ");
            }

            _insertedIds.Add(id);

            var updated = await _repository.GetByIdAsync(id);
            updated!.Actuals = 999;
            updated.Comments = "Updated";

            await _repository.UpdateAsync(updated);

            using var conn2 = _context.CreateConnection();
            var result = conn2.QuerySingle<DataBridgesFy>(
                "SELECT * FROM DATA_BridgesFY WHERE Id=@id", new { id });

            Assert.Equal(999, result.Actuals);
            Assert.Equal("Updated", result.Comments);
        }

        // -------------------------------------------------
        // TEST: DELETE
        // -------------------------------------------------
        [Fact]
        public async Task DeleteAsync_ShouldRemoveRecord()
        {
            int id;

            using (var conn = _context.CreateConnection())
            {
                id = conn.ExecuteScalar<int>(@"
                    INSERT INTO DATA_BridgesFY
                    (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                     Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, New,
                     Economics, QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap,
                     IndustrialPerformance, ProtoTooling, Other, `Check`, Comments)
                    VALUES
                    (1, UUID(), NOW(), 1,2025,1,1,1,1,
                     777,7,666,6,111,5,2,1,3,
                     4,2,1,1,3,2,
                     4,1,2,0,'Test delete');
                    SELECT LAST_INSERT_ID();
                ");
            }

            _insertedIds.Add(id);

            await _repository.DeleteAsync(id);

            using var conn2 = _context.CreateConnection();
            var result = conn2.QuerySingleOrDefault<DataBridgesFy>(
                "SELECT * FROM DATA_BridgesFY WHERE Id=@id", new { id });

            Assert.Null(result);
        }
    }
}
