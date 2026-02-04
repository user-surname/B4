namespace B4.Api.Dto.GetDto
{
    public class PlantillasBotonesPasosTiposGetDto
    {
        public int IdPasoTipo { get; set; }
        public string Pasotipo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long IsActive { get; set; }
    }
}