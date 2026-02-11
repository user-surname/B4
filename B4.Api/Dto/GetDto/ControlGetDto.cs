namespace B4.Api.Dto.GetDto
{
    public class ControlGetDto
    {
        public int IdControl { get; set; }
        public int Anyo { get; set; }
        public int IdCiclo { get; set; }
        public int IdFaseControl { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Final { get; set; }
        public bool? Activo { get; set; }
        public string? AddInfo { get; set; }
        public bool? SendAutEmail { get; set; }
        public bool? BWReportsMandatory { get; set; }
    }
}
