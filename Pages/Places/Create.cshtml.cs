using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlaceRegisterApp_Razor.Data;
using PlaceRegisterApp_Razor.Models;
using System.ComponentModel.DataAnnotations;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _env;
    public CreateModel(ApplicationDbContext db, IWebHostEnvironment env) { _db = db; _env = env; }

    [BindProperty]
    public Place Place { get; set; } = new Place();

    [BindProperty]
    [Display(Name = "Imagem")]
    public IFormFile? ImageFile { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        string? savedName = null;
        if (ImageFile != null && ImageFile.Length > 0)
        {
            if (ImageFile.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFile", "A imagem deve ter no máximo 5 MB.");
                return Page();
            }

            var ext = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            if (!allowed.Contains(ext))
            {
                ModelState.AddModelError("ImageFile", "Formato de imagem não suportado.");
                return Page();
            }

            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploads);
            savedName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploads, savedName);
            await using var stream = System.IO.File.Create(filePath);
            await ImageFile.CopyToAsync(stream);
        }

        Place.ImageFile = savedName;
        _db.Places.Add(Place);
        await _db.SaveChangesAsync();
        return RedirectToPage("/Index");
    }
}
