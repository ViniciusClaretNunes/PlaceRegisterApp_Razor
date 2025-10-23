using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlaceRegisterApp_Razor.Data;
using PlaceRegisterApp_Razor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _env;
    public CreateModel(ApplicationDbContext db, IWebHostEnvironment env) { _db = db; _env = env; }

    [BindProperty]
    public Place Place { get; set; } = new Place();

    [BindProperty]
    [Display(Name = "Imagens")]
    public List<IFormFile> ImageFiles { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        var uploads = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploads);

        var selectedFiles = ImageFiles?
            .Where(f => f != null && f.Length > 0)
            .ToList() ?? new List<IFormFile>();

        if (selectedFiles.Count > 3)
        {
            ModelState.AddModelError("ImageFiles", "Envie no máximo 3 imagens.");
        }

        foreach (var image in selectedFiles)
        {
            if (image.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFiles", "Cada imagem deve ter no máximo 5 MB.");
                break;
            }

            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            if (!allowed.Contains(ext))
            {
                ModelState.AddModelError("ImageFiles", "Formato de imagem não suportado.");
                break;
            }
        }

        if (!ModelState.IsValid) return Page();

        var savedNames = new List<string>();
        foreach (var image in selectedFiles)
        {
            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
            var savedName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploads, savedName);
            await using var stream = System.IO.File.Create(filePath);
            await image.CopyToAsync(stream);
            savedNames.Add(savedName);
        }

        Place.SetImageFiles(savedNames);
        _db.Places.Add(Place);
        await _db.SaveChangesAsync();
        return RedirectToPage("/Index");
    }
}
