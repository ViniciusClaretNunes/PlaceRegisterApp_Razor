using Microsoft.EntityFrameworkCore;
using PlaceRegisterApp_Razor.Models;

namespace PlaceRegisterApp_Razor.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Place> Places { get; set; } = null!;
    }
}
