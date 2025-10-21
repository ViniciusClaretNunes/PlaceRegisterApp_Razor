using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlaceRegisterApp_Razor.Data;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public DetailsModel(ApplicationDbContext db) { _db = db; }

    public PlaceRegisterApp_Razor.Models.Place Place { get; set; } = new PlaceRegisterApp_Razor.Models.Place();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var p = await _db.Places.FindAsync(id);
        if (p == null) return RedirectToPage("/Index");
        Place = p;
        return Page();
    }
}
