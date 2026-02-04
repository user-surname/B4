namespace B4.Api.Dto.GetDto
{
    public class FasesGetDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long IsActive { get; set; }
        public int IdFase { get; set; }
        public string Fase { get; set; }
        public string? FaseAlias { get; set; }


    }
}
