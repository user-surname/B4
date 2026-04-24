using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using B4.Data.DataFactory;
using B4.Data.DataFactory.Repositories;
using B4.Models.Entities.DataEntities;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace B4.Tests.MySQLTests.Data
{
    public class DataActualsRepositoryTest : IDisposable
    {
        private readonly MySQLDapperContext _context;
        private readonly DataActualsRepository _repository;
        private readonly List<int> _insertedIds = new();

        public DataActualsRepositoryTest()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _context = new MySQLDapperContext(config);
            _repository = new DataActualsRepository(_context);
        }

        public void Dispose()
        {
            if (_insertedIds.Count == 0) return;

            using var conn = _context.CreateConnection();
            conn.Execute("DELETE FROM DATA_Actuals WHERE id IN @ids", new { ids = _insertedIds });
            _insertedIds.Clear();
        }

        // -------------------------------------------------
        // TEST: ADD
        // -------------------------------------------------
        [Fact]
        public async Task AddAsync_ShouldInsertRecord()
        {
            var record = new DataActuals
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

            await _repository.AddAsync(record);

            int newId;
            using (var conn = _context.CreateConnection())
            {
                newId = conn.ExecuteScalar<int>("SELECT id FROM DATA_Actuals ORDER BY id DESC LIMIT 1;");
            }

            _insertedIds.Add(newId);

            using var conn2 = _context.CreateConnection();
            var result = conn2.QuerySingleOrDefault<DataActuals>("SELECT * FROM DATA_Actuals WHERE id = @id", new { id = newId });

            Assert.NotNull(result);
            Assert.Equal(10, result.Mes00);
            Assert.Equal(140, result.Mes13);
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
                    INSERT INTO DATA_Actuals
                    (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                     mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                    VALUES
                    (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                     10,20,30,40,50,60,70,80,90,100,110,120,130,140);
                    SELECT LAST_INSERT_ID();
                ");
            }

            _insertedIds.Add(newId);

            var result = await _repository.GetByIdAsync(newId);

            Assert.NotNull(result);
            Assert.Equal(10, result.Mes00);
            Assert.Equal(140, result.Mes13);
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
                    INSERT INTO DATA_Actuals
                    (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                     mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                    VALUES
                    (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                     10,20,30,40,50,60,70,80,90,100,110,120,130,140);
                    SELECT LAST_INSERT_ID();
                ");

                id2 = conn.ExecuteScalar<int>(@"
                    INSERT INTO DATA_Actuals
                    (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                     mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                    VALUES
                    (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                     11,21,31,41,51,61,71,81,91,101,111,121,131,141);
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
                    INSERT INTO DATA_Actuals
                    (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                     mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                    VALUES
                    (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                     10,20,30,40,50,60,70,80,90,100,110,120,130,140);
                    SELECT LAST_INSERT_ID();
                ");
            }

            _insertedIds.Add(newId);

            var updated = new DataActuals
            {
                Id = newId,
                Mes00 = 999,
                Mes13 = 888
            };

            await _repository.UpdateAsync(updated);

            using var conn2 = _context.CreateConnection();
            var result = conn2.QuerySingleOrDefault<DataActuals>("SELECT * FROM DATA_Actuals WHERE id = @id", new { id = newId });

            Assert.Equal(999, result.Mes00);
            Assert.Equal(888, result.Mes13);
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
                    INSERT INTO DATA_Actuals
                    (idAPICarga, guidCarga, FechaUltModif, idCompany, ejercicio, idCiclo, idFase, idCurrency, idEpigrafe,
                     mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07, mes08, mes09, mes10, mes11, mes12, mes13)
                    VALUES
                    (1, UUID(), NOW(), 1, 2025, 1, 1, 1, 1,
                     10,20,30,40,50,60,70,80,90,100,110,120,130,140);
                    SELECT LAST_INSERT_ID();
                ");
            }

            _insertedIds.Add(newId);

            await _repository.DeleteAsync(newId);

            using var conn2 = _context.CreateConnection();
            var result = conn2.QuerySingleOrDefault<DataActuals>("SELECT * FROM DATA_Actuals WHERE id = @id", new { id = newId });

            Assert.Null(result);
        }
    }
}

