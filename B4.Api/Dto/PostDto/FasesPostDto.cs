namespace B4.Api.Dto.PostDto
{
    public class FasesPostDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long IsActive { get; set; }
        public string Fase { get; set; }
        public string? FaseAlias { get; set; }
    }
}
