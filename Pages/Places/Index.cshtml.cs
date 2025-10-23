using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlaceRegisterApp_Razor.Data;
using PlaceRegisterApp_Razor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PlacesIndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public PlacesIndexModel(ApplicationDbContext db) { _db = db; }

    public IList<Place> Places { get; private set; } = new List<Place>();

    public async Task OnGetAsync()
    {
        Places = await _db.Places.AsNoTracking().ToListAsync();
    }
}
