using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Models.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace B4.Tests.MySQLTests
{
    public class DataTipoCambioRepositoryTest : IDisposable
    {
        private readonly MySQLDapperContext _context;
        private readonly DataTipoCambioRepository _repository;
        private readonly List<int> _insertedIds = new();

        public DataTipoCambioRepositoryTest()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _context = new MySQLDapperContext(config);
            _repository = new DataTipoCambioRepository(_context);
        }

        public void Dispose()
        {
            if (_insertedIds.Count == 0) return;

            using var conn = _context.CreateConnection();
            conn.Execute("DELETE FROM DATA_Tipo_Cambio WHERE id IN @ids", new { ids = _insertedIds });
            _insertedIds.Clear();
        }

        // -------------------------------------------------
        // TEST: ADD
        // -------------------------------------------------
        [Fact]
        public async Task AddAsync_ShouldInsertRecord()
        {
            var record = new DataTipoCambio
            {
                IdAPICarga = 1,
                GuidCarga = Guid.NewGuid(),
                FechaUltModif = DateTime.UtcNow,
                Ejercicio = 2025,
                IdCurrency = 1,
                CalendarDay = DateTime.UtcNow.Date,
                Mes = 11,
                P = 100,
                FC = 105,
                FB = 110,
                IdCarga = null,
                IdCargaSTGBW = null,
                IdHoja = null
            };

            await _repository.AddAsync(record);

            int newId;
            using (var conn = _context.CreateConnection())
            {
                newId = conn.ExecuteScalar<int>("SELECT id FROM DATA_Tipo_Cambio ORDER BY id DESC LIMIT 1;");
            }

            _insertedIds.Add(newId);

            using var conn2 = _context.CreateConnection();
            var result = conn2.QuerySingleOrDefault<DataTipoCambio>("SELECT * FROM DATA_Tipo_Cambio WHERE id = @id", new { id = newId });

            Assert.NotNull(result);
            Assert.Equal(100, result.P);
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
                    INSERT INTO DATA_Tipo_Cambio 
                    (idAPICarga, guidCarga, FechaUltModif, ejercicio, idCurrency, CalendarDay, mes, P, FC, FB, idCarga, idCargaSTGBW, idHoja)
                    VALUES
                    (1, UUID(), NOW(), 2025, 1, CURDATE(), 11, 100, 105, 110, NULL, NULL, NULL);
                    SELECT LAST_INSERT_ID();
                ");
            }

            _insertedIds.Add(newId);

            var result = await _repository.GetByIdAsync(newId);

            Assert.NotNull(result);
            Assert.Equal(100, result.P);
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
                    INSERT INTO DATA_Tipo_Cambio 
                    (idAPICarga, guidCarga, FechaUltModif, ejercicio, idCurrency, CalendarDay, mes, P, FC, FB, idCarga, idCargaSTGBW, idHoja)
                    VALUES (1, UUID(), NOW(), 2025, 1, CURDATE(), 11, 100, 105, 110, NULL, NULL, NULL);
                    SELECT LAST_INSERT_ID();
                ");
                id2 = conn.ExecuteScalar<int>(@"
                    INSERT INTO DATA_Tipo_Cambio 
                    (idAPICarga, guidCarga, FechaUltModif, ejercicio, idCurrency, CalendarDay, mes, P, FC, FB, idCarga, idCargaSTGBW, idHoja)
                    VALUES (1, UUID(), NOW(), 2025, 2, CURDATE(), 11, 200, 210, 220, NULL, NULL, NULL);
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
                    INSERT INTO DATA_Tipo_Cambio 
                    (idAPICarga, guidCarga, FechaUltModif, ejercicio, idCurrency, CalendarDay, mes, P, FC, FB, idCarga, idCargaSTGBW, idHoja)
                    VALUES (1, UUID(), NOW(), 2025, 1, CURDATE(), 11, 100, 105, 110, NULL, NULL, NULL);
                    SELECT LAST_INSERT_ID();
                ");
            }

            _insertedIds.Add(newId);

            var updated = new DataTipoCambio
            {
                Id = newId,
                P = 999,
                FC = 1000
            };

            await _repository.UpdateAsync(updated);

            using var conn2 = _context.CreateConnection();
            var result = conn2.QuerySingle<DataTipoCambio>("SELECT * FROM DATA_Tipo_Cambio WHERE id = @id", new { id = newId });

            Assert.Equal(999, result.P);
            Assert.Equal(1000, result.FC);
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
                    INSERT INTO DATA_Tipo_Cambio 
                    (idAPICarga, guidCarga, FechaUltModif, ejercicio, idCurrency, CalendarDay, mes, P, FC, FB, idCarga, idCargaSTGBW, idHoja)
                    VALUES (1, UUID(), NOW(), 2025, 1, CURDATE(), 11, 100, 105, 110, NULL, NULL, NULL);
                    SELECT LAST_INSERT_ID();
                ");
            }

            _insertedIds.Add(newId);

            await _repository.DeleteAsync(newId);

            using var conn2 = _context.CreateConnection();
            var result = conn2.QuerySingleOrDefault<DataTipoCambio>("SELECT * FROM DATA_Tipo_Cambio WHERE id = @id", new { id = newId });

            Assert.Null(result);
        }
    }
}

