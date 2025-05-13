using System.ComponentModel.DataAnnotations;

namespace TrueStoryAPI.DTOs
{
    public class PatchProductDto
    {
        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string? Name { get; set; }
        [Required]
        public Dictionary<string, object>? Data { get; set; }
    }
}
