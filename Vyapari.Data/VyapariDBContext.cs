using Microsoft.EntityFrameworkCore;
using Vyapari.Data.Entities;

namespace Vyapari.Data
{
    public class VyapariDBContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        public VyapariDBContext(DbContextOptions<VyapariDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasKey(p => p.ProductId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
        }

        

    }
}