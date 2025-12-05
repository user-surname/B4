namespace B4.Tests.MySQLTests.Data
{
    using B4.Data.MySQL;
    using Microsoft.Extensions.Configuration;
    using Dapper;
    using B4.Data.MySQL.Repositories.DataRepositories;
    using B4.Models.Entities.DataEtities;

    public class DataBridgesMonthBwRepositoryTest : IDisposable
    {
        private readonly MySQLDapperContext _context;
        private readonly DataBridgesMonthBwRepository _repository;

        private readonly List<int> _insertedIds = new();

        public DataBridgesMonthBwRepositoryTest()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _context = new MySQLDapperContext(config);
            _repository = new DataBridgesMonthBwRepository(_context);
        }

        public void Dispose()
        {
            if (_insertedIds.Count == 0) return;

            using var conn = _context.CreateConnection();
            conn.Execute("DELETE FROM DATA_BridgesMonth_BW WHERE Id IN @ids", new { ids = _insertedIds });
            _insertedIds.Clear();
        }

        // -------------------------------------------------
        // TEST: ADD
        // -------------------------------------------------
        [Fact]
        public async Task AddAsync_ShouldInsertRecord()
        {
            var record = new DataBridgesMonthBw
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
                Volume = 100,
                InventoryChange = 10,
                Mix = 5,
                New = 2,
                Economics = 50,
                QuickSavings = 3,
                CurrencyMix = 1,
                ExchangeRate = 1,
                RawMaterial = 20,
                Scrap = 1,
                IndustrialPerformance = 5,
                ProtoTooling = 2,
                Other = 1,
                Check = 0,
                Comments = "Test add",
                IdCarga = null,
                IdCargaSTGBW = null,
                IdHoja = null
            };

            await _repository.AddAsync(record);

            int newId;
            using (var conn = _context.CreateConnection())
            {
                newId = conn.ExecuteScalar<int>("SELECT Id FROM DATA_BridgesMonth_BW ORDER BY Id DESC LIMIT 1;");
            }

            _insertedIds.Add(newId);

            using var conn2 = _context.CreateConnection();
            var result = conn2.QuerySingleOrDefault<DataBridgesMonthBw>("SELECT * FROM DATA_BridgesMonth_BW WHERE Id = @id", new { id = newId });

            Assert.NotNull(result);
            Assert.Equal(100, result.Volume);
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
                    INSERT INTO DATA_BridgesMonth_BW 
                    (idAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                     Volume, InventoryChange, Mix, `New`, Economics, QuickSavings, CurrencyMix, ExchangeRate,
                     RawMaterial, Scrap, IndustrialPerformance, ProtoTooling, Other, `Check`, Comments,
                     idCarga, idCargaSTGBW, idHoja)
                    VALUES
                    (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                     100, 10, 5, 2, 50, 3, 1, 1,
                     20, 1, 5, 2, 1, 0, 'Test getbyid', NULL, NULL, NULL);
                    SELECT LAST_INSERT_ID();
                ");
            }

            _insertedIds.Add(newId);

            var result = await _repository.GetByIdAsync(newId);

            Assert.NotNull(result);
            Assert.Equal(100, result.Volume);
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
                    INSERT INTO DATA_BridgesMonth_BW 
                    (idAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                     Volume, InventoryChange, Mix, `New`, Economics, QuickSavings, CurrencyMix, ExchangeRate,
                     RawMaterial, Scrap, IndustrialPerformance, ProtoTooling, Other, `Check`, Comments,
                     idCarga, idCargaSTGBW, idHoja)
                    VALUES
                    (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                     100, 10, 5, 2, 50, 3, 1, 1,
                     20, 1, 5, 2, 1, 0, 'Test A', NULL, NULL, NULL);
                    SELECT LAST_INSERT_ID();
                ");

                id2 = conn.ExecuteScalar<int>(@"
                    INSERT INTO DATA_BridgesMonth_BW 
                    (idAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                     Volume, InventoryChange, Mix, `New`, Economics, QuickSavings, CurrencyMix, ExchangeRate,
                     RawMaterial, Scrap, IndustrialPerformance, ProtoTooling, Other, `Check`, Comments,
                     idCarga, idCargaSTGBW, idHoja)
                    VALUES
                    (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                     200, 20, 10, 4, 60, 6, 2, 2,
                     40, 2, 10, 4, 2, 0, 'Test B', NULL, NULL, NULL);
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
                    INSERT INTO DATA_BridgesMonth_BW 
                    (idAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                     Volume, InventoryChange, Mix, `New`, Economics, QuickSavings, CurrencyMix, ExchangeRate,
                     RawMaterial, Scrap, IndustrialPerformance, ProtoTooling, Other, `Check`, Comments,
                     idCarga, idCargaSTGBW, idHoja)
                    VALUES
                    (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                     100, 10, 5, 2, 50, 3, 1, 1,
                     20, 1, 5, 2, 1, 0, 'Test update', NULL, NULL, NULL);
                    SELECT LAST_INSERT_ID();
                ");
            }

            _insertedIds.Add(newId);

            var updated = new DataBridgesMonthBw
            {
                Id = newId,
                Volume = 999,
                Comments = "Updated"
                // Puedes rellenar otros campos según necesites
            };

            await _repository.UpdateAsync(updated);

            using var conn2 = _context.CreateConnection();
            var result = conn2.QuerySingle<DataBridgesMonthBw>("SELECT * FROM DATA_BridgesMonth_BW WHERE Id = @id", new { id = newId });

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
                    INSERT INTO DATA_BridgesMonth_BW 
                    (idAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase, IdCurrency, IdEpigrafe,
                     Volume, InventoryChange, Mix, `New`, Economics, QuickSavings, CurrencyMix, ExchangeRate,
                     RawMaterial, Scrap, IndustrialPerformance, ProtoTooling, Other, `Check`, Comments,
                     idCarga, idCargaSTGBW, idHoja)
                    VALUES
                    (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                     100, 10, 5, 2, 50, 3, 1, 1,
                     20, 1, 5, 2, 1, 0, 'Test delete', NULL, NULL, NULL);
                    SELECT LAST_INSERT_ID();
                ");
            }

            _insertedIds.Add(newId);

            await _repository.DeleteAsync(newId);

            using var conn2 = _context.CreateConnection();
            var result = conn2.QuerySingleOrDefault<DataBridgesMonthBw>("SELECT * FROM DATA_BridgesMonth_BW WHERE Id = @id", new { id = newId });

            Assert.Null(result);
        }
    }
}

