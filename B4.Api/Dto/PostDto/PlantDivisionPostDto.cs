using System.ComponentModel.DataAnnotations;

namespace B4.Api.Dto.PostDto
{
    public class PlantDivisionPostDto
    {
        [Required]
        [StringLength(100)]
        public string Division { get; set; } = string.Empty;
    }
}
