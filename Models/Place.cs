using System.ComponentModel.DataAnnotations;

namespace PlaceRegisterApp_Razor.Models
{
    public class Place
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(250)]
        public string? Features { get; set; }

        [Range(0,5)]
        public int Rating { get; set; }

        public string? ImageFile { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
