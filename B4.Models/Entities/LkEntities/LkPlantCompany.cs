using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.LkEntities
{
    // LK_PLANT_COMPANY
    public class LkPlantCompany : LkBase
    {
        public int IdCompany { get; set; }
        public string CompanyCode { get; set; }
        public string ManagementCompany { get; set; }
        public int IdCurrency { get; set; }
        public string Company { get; set; }
        public bool? Active { get; set; }
        public int IdDivision { get; set; }
        public int IdDivisionCompany { get; set; }
        public int IdSubdivision { get; set; }
        public int IdCountry { get; set; }
        public string Location { get; set; }
        public string? Obs { get; set; }
        public string? RegionalValidatorPwd { get; set; }
        public string? Region { get; set; }
        public string? RegionalValidator { get; set; }
        public string? RegionalValidatorEmail { get; set; }
        public string? ContributorPwd { get; set; }
        public string? ManagementCompanyBackup { get; set; }
        public string? RegionBackup { get; set; }
        public string? CountryBackup { get; set; }
        public int? IdRegValidator { get; set; }

        public LkPlantCompany() { }

        public LkPlantCompany(int idCompany, string companyCode, string managementCompany, int idCurrency, string company,
            bool? active, int idDivision, int idDivisionCompany, int idSubdivision, int idCountry, string location,
            string? obs, string? regionalValidatorPwd, string? region, string? regionalValidator,
            string? regionalValidatorEmail, string? contributorPwd, string? managementCompanyBackup,
            string? regionBackup, string? countryBackup, int? idRegValidator,
            DateTime createdAt, DateTime updatedAt, long isActive) : base(createdAt, updatedAt, isActive)
        {
            IdCompany = idCompany;
            CompanyCode = companyCode;
            ManagementCompany = managementCompany;
            IdCurrency = idCurrency;
            Company = company;
            Active = active;
            IdDivision = idDivision;
            IdDivisionCompany = idDivisionCompany;
            IdSubdivision = idSubdivision;
            IdCountry = idCountry;
            Location = location;
            Obs = obs;
            RegionalValidatorPwd = regionalValidatorPwd;
            Region = region;
            RegionalValidator = regionalValidator;
            RegionalValidatorEmail = regionalValidatorEmail;
            ContributorPwd = contributorPwd;
            ManagementCompanyBackup = managementCompanyBackup;
            RegionBackup = regionBackup;
            CountryBackup = countryBackup;
            IdRegValidator = idRegValidator;
        }
    }
}

