namespace B4.Api.Dto.GetDto
{
    public class PlantCurrencyGetDto
    {
        public int IdCurrency { get; set; }
        public string Currency { get; set; }
        public string CurrencyAlias { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long IsActive { get; set; }
    }
}