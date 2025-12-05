using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.MySQL.Repositories.LkRepositories
{
    public class PlantCompanyRepository : IPlantCompanyRepository
    {
        // Nombre fijo de la tabla
        private const string _tableName = "LK_PLANT_COMPANY";

        // Contexto que maneja la conexión Dapper
        private readonly MySQLDapperContext _context;

        public PlantCompanyRepository(MySQLDapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(LkPlantCompany entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName} 
                (IdCompany, CompanyCode, ManagementCompany, idCurrency, Company, Active, 
                 idDivision, idDivisionCompany, idSubdivision, idCountry, Location, obs, 
                 RegionalValidatorPwd, Region, RegionalValidator, RegionalValidatorEmail, 
                 ContributorPwd, ManagementCompanyBackup, RegionBackup, CountryBackup, idRegValidator)
                VALUES 
                (@IdCompany, @CompanyCode, @ManagementCompany, @IdCurrency, @Company, @Active, 
                 @IdDivision, @IdDivisionCompany, @IdSubdivision, @IdCountry, @Location, @Obs, 
                 @RegionalValidatorPwd, @Region, @RegionalValidator, @RegionalValidatorEmail, 
                 @ContributorPwd, @ManagementCompanyBackup, @RegionBackup, @CountryBackup, @IdRegValidator)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un PlantCompany en la base de datos.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<LkPlantCompany?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE IdCompany = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<LkPlantCompany>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el PlantCompany con id {id}.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<LkPlantCompany>> GetAllAsync()
        {
            const string sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<LkPlantCompany>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de PlantCompany.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(LkPlantCompany entity)
        {
            const string sql = $@"
                UPDATE {_tableName} SET 
                    CompanyCode = @CompanyCode,
                    ManagementCompany = @ManagementCompany,
                    idCurrency = @IdCurrency,
                    Company = @Company,
                    Active = @Active,
                    idDivision = @IdDivision,
                    idDivisionCompany = @IdDivisionCompany,
                    idSubdivision = @IdSubdivision,
                    idCountry = @IdCountry,
                    Location = @Location,
                    obs = @Obs,
                    RegionalValidatorPwd = @RegionalValidatorPwd,
                    Region = @Region,
                    RegionalValidator = @RegionalValidator,
                    RegionalValidatorEmail = @RegionalValidatorEmail,
                    ContributorPwd = @ContributorPwd,
                    ManagementCompanyBackup = @ManagementCompanyBackup,
                    RegionBackup = @RegionBackup,
                    CountryBackup = @CountryBackup,
                    idRegValidator = @IdRegValidator
                WHERE IdCompany = @IdCompany";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, entity);
                if (rows == 0)
                    throw new Exception($"No se encontró el PlantCompany con id {entity.IdCompany} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el PlantCompany con id {entity.IdCompany}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = $@"DELETE FROM {_tableName} WHERE IdCompany = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, new { Id = id });
                if (rows == 0)
                    throw new Exception($"No se encontró el PlantCompany con id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el PlantCompany con id {id}.", ex);
            }
        }
    }
}

