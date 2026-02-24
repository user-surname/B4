namespace B4.Web.Models
{
    public class DemoUploadViewModel
    {
        public string Email { get; set; } = "admin@ejemplo.com";
        public string Password { get; set; } = "1234";

        public int Planta { get; set; } = 10;
        public int Ejercicio { get; set; } = 2024;
        public int Mes { get; set; } = 1;
        public string Tipo { get; set; } = "EUR";

        public string JsonBody { get; set; } =
@"[
  {
    ""idEpigrafe"": 100,
    ""mes01"": 1, ""mes02"": 2, ""mes03"": 3, ""mes04"": 4, ""mes05"": 5, ""mes06"": 6,
    ""mes07"": 7, ""mes08"": 8, ""mes09"": 9, ""mes10"": 10, ""mes11"": 11, ""mes12"": 12, ""mes13"": 13
  }
]";
    }
}
