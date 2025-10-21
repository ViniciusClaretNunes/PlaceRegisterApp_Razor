using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlaceRegisterApp_Razor.Data;
using PlaceRegisterApp_Razor.Models;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public IndexModel(ApplicationDbContext db) { _db = db; }
    public IList<Place> Places { get; set; } = new List<Place>();

    public async Task OnGetAsync()
    {
        Places = await _db.Places.AsNoTracking().ToListAsync();
    }
}
