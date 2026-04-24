using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using B4.Data.DataFactory;
using B4.Data.DataFactory.Repositories;
using B4.Models.Entities.DataEntities;
using Dapper;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace B4.Tests.MySQLTests.Data
{
    public class StgDataBridgesMonthRepositoryTest : IDisposable
    {
        private readonly MySQLDapperContext _context;
        private readonly StgDataBridgesMonthRepository _repository;
        private readonly List<int> _insertedIds = new();

        public StgDataBridgesMonthRepositoryTest()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _context = new MySQLDapperContext(config);
            _repository = new StgDataBridgesMonthRepository(_context);
        }

        public void Dispose()
        {
            if (_insertedIds.Count == 0) return;

            using var conn = _context.CreateConnection();
            conn.Execute("DELETE FROM STG_DATA_BridgesMonth WHERE Id IN @ids", new { ids = _insertedIds });
            _insertedIds.Clear();
        }

        // -------------------------------------------------
        // TEST: ADD
        // -------------------------------------------------
        [Fact]
        public async Task AddAsync_ShouldInsertRecord()
        {
            var record = new StgDataBridgesMonth
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
                Actuals = 100,
                PctActuals = 10,
                Budget = 50,
                PctBudget = 5,
                Variance = 20,
                Volume = 200,
                InventoryChange = 15,
                Mix = 10,
                New = 5,
                EconomicsInflation = 2,
                EconomicsInflationClaim = 1,
                EconomicsPricingIssues = 3,
                EconomicsLTAS = 4,
                EconomicsBPs = 2,
                EconomicsComponentEffect = 1,
                StockValuation = 10,
                QuickSavings = 5,
                CurrencyMix = 2,
                ExchangeRate = 1,
                RawMaterial = 50,
                Scrap = 3,
                PerformanceGSD = 1,
                PerformanceQuality = 2,
                PerformanceOther = 1,
                PerformanceLaunchingCost = 0,
                ProtoTooling = 2,
                AccruralOthers = 1,
                OneTimeEffectOthers = 0,
                Other = 1,
                Comments = "Test add"
            };

            await _repository.AddAsync(record);

            int newId;
            using (var conn = _context.CreateConnection())
            {
                newId = conn.ExecuteScalar<int>("SELECT Id FROM STG_DATA_BridgesMonth ORDER BY Id DESC LIMIT 1;");
            }

            _insertedIds.Add(newId);

            using var conn2 = _context.CreateConnection();
            var result = conn2.QuerySingleOrDefault<StgDataBridgesMonth>("SELECT * FROM STG_DATA_BridgesMonth WHERE Id = @id", new { id = newId });

            Assert.NotNull(result);
            Assert.Equal(200, result.Volume);
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
                    INSERT INTO STG_DATA_BridgesMonth 
                    (IdAPICarga, GuidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                     Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, `New`,
                     EconomicsInflation, EconomicsInflationClaim, EconomicsPricingIssues, EconomicsLTAS, EconomicsBPs, EconomicsComponentEffect,
                     StockValuation, QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap,
                     PerformanceGSD, PerformanceQuality, PerformanceOther, PerformanceLaunchingCost,
                     ProtoTooling, AccruralOthers, OneTimeEffectOthers, Other, Comments)
                    VALUES
                    (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                     100, 10, 50, 5, 20, 200, 15, 10, 5,
                     2, 1, 3, 4, 2, 1,
                     10, 5, 2, 1, 50, 3,
                     1, 2, 1, 0,
                     2, 1, 0, 1, 'Test getbyid');
                    SELECT LAST_INSERT_ID();
                ");
            }

            _insertedIds.Add(newId);

            var result = await _repository.GetByIdAsync(newId);

            Assert.NotNull(result);
            Assert.Equal(200, result.Volume);
        }

        // -------------------------------------------------
        // TEST: GET ALL
        // -------------------------------------------------
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllRecords()
        {
            int id1, id2;
            using (var conn = _context.CreateConnection())
            {
                id1 = conn.ExecuteScalar<int>(@"
                    INSERT INTO STG_DATA_BridgesMonth 
                    (IdAPICarga, GuidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                     Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, `New`,
                     EconomicsInflation, EconomicsInflationClaim, EconomicsPricingIssues, EconomicsLTAS, EconomicsBPs, EconomicsComponentEffect,
                     StockValuation, QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap,
                     PerformanceGSD, PerformanceQuality, PerformanceOther, PerformanceLaunchingCost,
                     ProtoTooling, AccruralOthers, OneTimeEffectOthers, Other, Comments)
                    VALUES
                    (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                     100, 10, 50, 5, 20, 200, 15, 10, 5,
                     2, 1, 3, 4, 2, 1,
                     10, 5, 2, 1, 50, 3,
                     1, 2, 1, 0,
                     2, 1, 0, 1, 'Test A');
                    SELECT LAST_INSERT_ID();
                ");

                id2 = conn.ExecuteScalar<int>(@"
                    INSERT INTO STG_DATA_BridgesMonth 
                    (IdAPICarga, GuidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                     Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, `New`,
                     EconomicsInflation, EconomicsInflationClaim, EconomicsPricingIssues, EconomicsLTAS, EconomicsBPs, EconomicsComponentEffect,
                     StockValuation, QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap,
                     PerformanceGSD, PerformanceQuality, PerformanceOther, PerformanceLaunchingCost,
                     ProtoTooling, AccruralOthers, OneTimeEffectOthers, Other, Comments)
                    VALUES
                    (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                     200, 20, 100, 10, 40, 400, 30, 20, 10,
                     4, 2, 6, 8, 4, 2,
                     20, 10, 4, 2, 100, 6,
                     2, 4, 2, 0,
                     4, 2, 0, 2, 'Test B');
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
            int newId;
            using (var conn = _context.CreateConnection())
            {
                newId = conn.ExecuteScalar<int>(@"
                    INSERT INTO STG_DATA_BridgesMonth 
                    (IdAPICarga, GuidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                     Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, `New`,
                     EconomicsInflation, EconomicsInflationClaim, EconomicsPricingIssues, EconomicsLTAS, EconomicsBPs, EconomicsComponentEffect,
                     StockValuation, QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap,
                     PerformanceGSD, PerformanceQuality, PerformanceOther, PerformanceLaunchingCost,
                     ProtoTooling, AccruralOthers, OneTimeEffectOthers, Other, Comments)
                    VALUES
                    (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                     100, 10, 50, 5, 20, 200, 15, 10, 5,
                     2, 1, 3, 4, 2, 1,
                     10, 5, 2, 1, 50, 3,
                     1, 2, 1, 0,
                     2, 1, 0, 1, 'Test update');
                    SELECT LAST_INSERT_ID();
                ");
            }

            _insertedIds.Add(newId);

            var updated = new StgDataBridgesMonth
            {
                Id = newId,
                Volume = 999,
                Comments = "Updated"
            };

            await _repository.UpdateAsync(updated);

            using var conn2 = _context.CreateConnection();
            var result = conn2.QuerySingle<StgDataBridgesMonth>("SELECT * FROM STG_DATA_BridgesMonth WHERE Id = @id", new { id = newId });

            Assert.Equal(999, result.Volume);
            Assert.Equal("Updated", result.Comments);
        }

        // -------------------------------------------------
        // TEST: DELETE
        // -------------------------------------------------
        [Fact]
        public async Task DeleteAsync_ShouldRemoveRecord()
        {
            int newId;
            using (var conn = _context.CreateConnection())
            {
                newId = conn.ExecuteScalar<int>(@"
                    INSERT INTO STG_DATA_BridgesMonth 
                    (IdAPICarga, GuidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                     Actuals, PctActuals, Budget, PctBudget, Variance, Volume, InventoryChange, Mix, `New`,
                     EconomicsInflation, EconomicsInflationClaim, EconomicsPricingIssues, EconomicsLTAS, EconomicsBPs, EconomicsComponentEffect,
                     StockValuation, QuickSavings, CurrencyMix, ExchangeRate, RawMaterial, Scrap,
                     PerformanceGSD, PerformanceQuality, PerformanceOther, PerformanceLaunchingCost,
                     ProtoTooling, AccruralOthers, OneTimeEffectOthers, Other, Comments)
                    VALUES
                    (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                     100, 10, 50, 5, 20, 200, 15, 10, 5,
                     2, 1, 3, 4, 2, 1,
                     10, 5, 2, 1, 50, 3,
                     1, 2, 1, 0,
                     2, 1, 0, 1, 'Test delete');
                    SELECT LAST_INSERT_ID();
                ");
            }

            _insertedIds.Add(newId);

            await _repository.DeleteAsync(newId);

            using var conn2 = _context.CreateConnection();
            var result = conn2.QuerySingleOrDefault<StgDataBridgesMonth>("SELECT * FROM STG_DATA_BridgesMonth WHERE Id = @id", new { id = newId });

            Assert.Null(result);
        }
    }
}

