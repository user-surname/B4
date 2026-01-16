namespace B4.Api.Dto.GetDto;

public sealed class DataActualsGetDto
{
    public HeadDto head { get; set; } = new();
    public List<DetailDto> detail { get; set; } = new();

    public sealed class HeadDto
    {
        public DateTime ts { get; set; }
        public string p { get; set; } = ""; // planta
        public int e { get; set; }           // ejercicio
        public int c { get; set; }           // idCiclo
        public string f { get; set; } = "";  // idFase
        public string m { get; set; } = "";  // moneda
    }

    public sealed class DetailDto
    {
        public int e { get; set; }           // idEpigrafe
        public decimal[] v { get; set; } = Array.Empty<decimal>(); // mes00..mes13
    }
}
