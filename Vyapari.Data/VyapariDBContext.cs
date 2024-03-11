using Microsoft.EntityFrameworkCore;
using Vyapari.Data.Entities;

namespace Vyapari.Data
{
    public class VyapariDBContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<BlackListRoute> BlackListRoutes { get; set; }

        public DbSet<WhiteListRoute> WhiteListRoutes { get; set; }

        public DbSet<Role> Roles { get; set; }

        public VyapariDBContext(DbContextOptions<VyapariDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasKey(p => p.ProductId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<BlackListRoute>().HasKey(b => b.Id);
            modelBuilder.Entity<WhiteListRoute>().HasKey(w => w.Id);
            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();
                    modelBuilder.Entity<BlackListRoute>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Route).IsRequired();

            entity.HasMany(e => e.AllowedRoles)
                .WithMany(r => r.BlackListRoutes)
                .UsingEntity<Dictionary<string, object>>(
                    "BlackListRouteRole",
                    j => j.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                    j => j.HasOne<BlackListRoute>().WithMany().HasForeignKey("BlackListRouteId")
                );
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
        });
            

        }



    }
}