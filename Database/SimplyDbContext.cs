using Microsoft.EntityFrameworkCore;
using ProductGuard.Models;

namespace ProductGuard.Database
{
    public class SimplyDbContext : DbContext
    {

        public SimplyDbContext(DbContextOptions<SimplyDbContext> options) : base(options)
        {
        }

        public DbSet<Products> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify the MySQL-specific configurations here if needed
            base.OnModelCreating(modelBuilder);
        }

    }
}
