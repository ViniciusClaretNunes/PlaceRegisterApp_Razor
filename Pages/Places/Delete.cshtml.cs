using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlaceRegisterApp_Razor.Data;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _env;
    public DeleteModel(ApplicationDbContext db, IWebHostEnvironment env) { _db = db; _env = env; }

    [BindProperty]
    public PlaceRegisterApp_Razor.Models.Place Place { get; set; } = new PlaceRegisterApp_Razor.Models.Place();

    public async Task OnGetAsync(int id)
    {
        var p = await _db.Places.FindAsync(id);
        if (p == null) return;
        Place = p;
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var p = await _db.Places.FindAsync(id);
        if (p == null) return RedirectToPage("/Index");

        // delete image
        if (!string.IsNullOrEmpty(p.ImageFile))
        {
            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            var path = Path.Combine(uploads, p.ImageFile);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        }

        _db.Places.Remove(p);
        await _db.SaveChangesAsync();
        return RedirectToPage("/Index");
    }
}
