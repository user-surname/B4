namespace B4.Api.Dto.GetDto
{
    public class CiclosGetDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long IsActive { get; set; }
        public int IdCiclo { get; set; }
        public string Ciclo { get; set; }
        public string Descripcion { get; set; }
    }
}
