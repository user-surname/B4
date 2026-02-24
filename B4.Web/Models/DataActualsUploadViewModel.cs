namespace B4.Web.Models
{
    public class DataActualsUploadViewModel
    {
        public int Planta { get; set; }
        public int Ejercicio { get; set; }
        public int Mes { get; set; }
        public string Tipo { get; set; } = "EUR";

        public string JsonBody { get; set; } = "";
    }
}
