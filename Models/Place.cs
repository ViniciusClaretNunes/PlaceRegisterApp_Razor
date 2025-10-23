using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        [Range(1,5, ErrorMessage = "A avaliação deve ser um número entre 1 e 5.")]
        public int Rating { get; set; }

        public string? ImageFile { get; set; }

        [NotMapped]
        public IReadOnlyList<string> ImageFiles => string.IsNullOrWhiteSpace(ImageFile)
            ? Array.Empty<string>()
            : ImageFile
                .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();

        public void SetImageFiles(IEnumerable<string> fileNames)
        {
            var names = fileNames?
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name.Trim())
                .ToArray() ?? Array.Empty<string>();

            ImageFile = names.Length == 0 ? null : string.Join(';', names);
        }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
