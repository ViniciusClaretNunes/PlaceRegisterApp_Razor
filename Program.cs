using Microsoft.EntityFrameworkCore;
using PlaceRegisterApp_Razor.Data;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite($"Data Source={Path.Combine(builder.Environment.ContentRootPath, "data", "places.db")}"));

var app = builder.Build();

var dataDir = Path.Combine(app.Environment.ContentRootPath, "data");
var uploadsDir = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "uploads");
Directory.CreateDirectory(dataDir);
Directory.CreateDirectory(uploadsDir);

// Ensure DB created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();

app.Run();
