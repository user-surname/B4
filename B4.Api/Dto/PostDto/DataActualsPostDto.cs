using System.Text.Json.Serialization;

namespace B4.Api.Dto.PostDto;

public sealed class DataActualsPostDto
{
    [JsonPropertyName("e")]
    public int e { get; set; }   // idEpigrafe

    [JsonPropertyName("m1")]  public decimal m1 { get; set; }
    [JsonPropertyName("m2")]  public decimal m2 { get; set; }
    [JsonPropertyName("m3")]  public decimal m3 { get; set; }
    [JsonPropertyName("m4")]  public decimal m4 { get; set; }
    [JsonPropertyName("m5")]  public decimal m5 { get; set; }
    [JsonPropertyName("m6")]  public decimal m6 { get; set; }
    [JsonPropertyName("m7")]  public decimal m7 { get; set; }
    [JsonPropertyName("m8")]  public decimal m8 { get; set; }
    [JsonPropertyName("m9")]  public decimal m9 { get; set; }
    [JsonPropertyName("m10")] public decimal m10 { get; set; }
    [JsonPropertyName("m11")] public decimal m11 { get; set; }
    [JsonPropertyName("m12")] public decimal m12 { get; set; }
    [JsonPropertyName("m13")] public decimal m13 { get; set; }
}
