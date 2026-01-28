using System.Text.Json.Serialization;

namespace B4.Api.Dto.PostDto;

public sealed class DataActualsPostDto
{
    [JsonPropertyName("e")]
    public int IdEpigrafe { get; set; }

    [JsonPropertyName("m1")]  public decimal Mes01 { get; set; }
    [JsonPropertyName("m2")]  public decimal Mes02 { get; set; }
    [JsonPropertyName("m3")]  public decimal Mes03 { get; set; }
    [JsonPropertyName("m4")]  public decimal Mes04 { get; set; }
    [JsonPropertyName("m5")]  public decimal Mes05 { get; set; }
    [JsonPropertyName("m6")]  public decimal Mes06 { get; set; }
    [JsonPropertyName("m7")]  public decimal Mes07 { get; set; }
    [JsonPropertyName("m8")]  public decimal Mes08 { get; set; }
    [JsonPropertyName("m9")]  public decimal Mes09 { get; set; }
    [JsonPropertyName("m10")] public decimal Mes10 { get; set; }
    [JsonPropertyName("m11")] public decimal Mes11 { get; set; }
    [JsonPropertyName("m12")] public decimal Mes12 { get; set; }
    [JsonPropertyName("m13")] public decimal Mes13 { get; set; }
}
