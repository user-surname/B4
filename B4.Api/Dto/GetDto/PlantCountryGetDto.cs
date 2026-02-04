namespace B4.Api.Dto.GetDto
{
    public class PlantCountryGetDto
    {
        public int IdCountry { get; set; }
        public string Country { get; set; } = string.Empty;
        public long IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}