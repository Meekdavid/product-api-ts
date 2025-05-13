using System.ComponentModel.DataAnnotations;

namespace TrueStoryAPI.DTOs
{
    public class UpdateProductDto
    {
        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string Name { get; set; } = null!;
        [Required]
        public Dictionary<string, object>? Data { get; set; }
    }
}
