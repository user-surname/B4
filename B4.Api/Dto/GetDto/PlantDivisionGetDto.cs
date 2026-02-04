using System;

namespace B4.Api.Dto.GetDto
{
    public class PlantDivisionGetDto
    {
        public int IdDivision { get; set; }
        public string Division { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long IsActive { get; set; }
    }
}
