using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlaceRegisterApp_Razor.Data;
using PlaceRegisterApp_Razor.Models;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _env;
    public EditModel(ApplicationDbContext db, IWebHostEnvironment env) { _db = db; _env = env; }

    [BindProperty]
    public Place Place { get; set; } = new Place();

    [BindProperty]
    public IFormFile? ImageFile { get; set; }

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

        if (ImageFile != null && ImageFile.Length > 0)
        {
            var ext = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            if (!allowed.Contains(ext))
            {
                ModelState.AddModelError("ImageFile", "Formato de imagem não suportado.");
                return Page();
            }
            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploads);
            var savedName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploads, savedName);
            await using var stream = System.IO.File.Create(filePath);
            await ImageFile.CopyToAsync(stream);

            // delete old file if exists
            if (!string.IsNullOrEmpty(p.ImageFile))
            {
                var oldPath = Path.Combine(uploads, p.ImageFile);
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }
            p.ImageFile = savedName;
        }

        await _db.SaveChangesAsync();
        return RedirectToPage("/Index");
    }
}
