namespace B4.Models.Entities.DataEntities;

public class LogActividad
{
    public int Id { get; set; }
    public DateTime? Timestamp { get; set; }
    public string? Nivel { get; set; }
    public string? Modulo { get; set; }
    public string? Entrada { get; set; }
    public int? IdFicheroExcel { get; set; }
    public string? NombreFicheroExcel { get; set; }
    public int IDCarga { get; set; }
    public int DCR_ID_JOB { get; set; }
}
