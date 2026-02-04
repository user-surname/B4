namespace B4.Api.Dto.GetDto
{
    public class PlantDivisionCompanyGetDto
    {
        public int IdDivisionCompany { get; set; }
        public string? DivisionCompany { get; set; }
        public long IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}