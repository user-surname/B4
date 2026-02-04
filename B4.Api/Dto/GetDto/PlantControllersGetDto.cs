namespace B4.Api.Dto.GetDto
{
    public class PlantControllersGetDto
    {
        public int IdCompanyController { get; set; }
        public int IdCompany { get; set; }
        public string Controller { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long IsActive { get; set; }
    }
}