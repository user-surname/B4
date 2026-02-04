using System;

namespace B4.Api.Dto.GetDto
{
    public class PlantCompanyGetDto
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long IsActive { get; set; }
    }
}
