using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlaceRegisterApp_Razor.Data;
using PlaceRegisterApp_Razor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _env;
    public EditModel(ApplicationDbContext db, IWebHostEnvironment env) { _db = db; _env = env; }

    [BindProperty]
    public Place Place { get; set; } = new Place();

    [BindProperty]
    public List<IFormFile> ImageFiles { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var p = await _db.Places.FindAsync(id);
        if (p == null) return RedirectToPage("/Index");
        Place = p;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var p = await _db.Places.FindAsync(Place.Id);
        if (p == null) return NotFound();

        p.Name = Place.Name;
        p.Address = Place.Address;
        p.Description = Place.Description;
        p.Features = Place.Features;
        p.Rating = Place.Rating;

        var uploads = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploads);

        var selectedFiles = ImageFiles?
            .Where(f => f != null && f.Length > 0)
            .ToList() ?? new List<IFormFile>();

        if (selectedFiles.Count > 3)
        {
            ModelState.AddModelError("ImageFiles", "Envie no máximo 3 imagens.");
        }

        foreach (var file in selectedFiles)
        {
            if (file.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFiles", "Cada imagem deve ter no máximo 5 MB.");
                break;
            }

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            if (!allowed.Contains(ext))
            {
                ModelState.AddModelError("ImageFiles", "Formato de imagem não suportado.");
                break;
            }
        }

        if (!ModelState.IsValid) return Page();

        if (selectedFiles.Count > 0)
        {
            foreach (var existing in p.ImageFiles)
            {
                var oldPath = Path.Combine(uploads, existing);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }

            var savedNames = new List<string>();
            foreach (var file in selectedFiles)
            {
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                var savedName = $"{Guid.NewGuid()}{ext}";
                var path = Path.Combine(uploads, savedName);
                await using var stream = System.IO.File.Create(path);
                await file.CopyToAsync(stream);
                savedNames.Add(savedName);
            }

            p.SetImageFiles(savedNames);
        }

        await _db.SaveChangesAsync();
        return RedirectToPage("/Index");
    }
}
