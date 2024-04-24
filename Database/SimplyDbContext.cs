using Microsoft.EntityFrameworkCore;
using ProductGuard.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProductGuard.Database
{
    public class SimplyDbContext : DbContext
    {

        public DbSet<CPU> CPUs { get; set; }
        public DbSet<GPU> GPUs { get; set; }
        public DbSet<Motherboard> Motherboards { get; set; }
        public DbSet<RAM> RAMs { get; set; }
        public DbSet<StorageDevice> StorageDevices { get; set; }


        public SimplyDbContext(DbContextOptions<SimplyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify the MySQL-specific configurations here if needed
            base.OnModelCreating(modelBuilder);
        }

    }
}
