using System.Text.Json.Serialization;

namespace B4.Api.Dto.GetDto;

public sealed class DataActualsGetDto
{
    [JsonPropertyName("head")]
    public HeadDto Head { get; set; } = new();

    [JsonPropertyName("detail")]
    public List<DetailDto> Detail { get; set; } = new();

    // ======================
    // CABECERA
    // ======================
    public sealed class HeadDto
    {
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("p")]
        public string Planta { get; set; } = "";

        [JsonPropertyName("e")]
        public int Ejercicio { get; set; }

        [JsonPropertyName("c")]
        public int IdCiclo { get; set; }

        [JsonPropertyName("f")]
        public string IdFase { get; set; } = "";

        [JsonPropertyName("m")]
        public string Moneda { get; set; } = "";
    }

    // ======================
    // DETALLE
    // ======================
    public sealed class DetailDto
    {
        [JsonPropertyName("e")]
        public int IdEpigrafe { get; set; }

        [JsonPropertyName("v")]
        public decimal[] Valores { get; set; } = Array.Empty<decimal>();
    }
}
