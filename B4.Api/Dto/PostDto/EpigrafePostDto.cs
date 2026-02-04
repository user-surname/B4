namespace B4.Api.Dto.PostDto
{
    public class EpigrafePostDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long IsActive { get; set; }
        public int IdPlantilla { get; set; }
        public int IdHoja { get; set; }
        public string PreEpigrafe { get; set; }
        public string Epigrafe { get; set; }
        public string EpigrafeFull { get; set; }
    }
}
