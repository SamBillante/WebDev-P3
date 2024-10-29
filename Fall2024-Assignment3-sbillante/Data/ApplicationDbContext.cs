using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_sbillante.Models;

namespace Fall2024_Assignment3_sbillante.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Fall2024_Assignment3_sbillante.Models.Movie> Movie { get; set; } = default!;
        public DbSet<Fall2024_Assignment3_sbillante.Models.Actor> Actor { get; set; } = default!;
    }
}
