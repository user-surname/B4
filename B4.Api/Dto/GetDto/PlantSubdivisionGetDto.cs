using System;

namespace B4.Api.Dto.GetDto
{
    public class PlantSubdivisionGetDto
    {
        public int IdSubdivision { get; set; }
        public string Subdivision { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long IsActive { get; set; }
    }
}