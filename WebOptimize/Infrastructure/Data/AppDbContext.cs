using Microsoft.EntityFrameworkCore;
using WebOptimize.Domain.Entities;

namespace WebOptimize.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Warehouse> Warehouses { get; set; } = null!;
        public DbSet<PickupPoint> PickupPoints { get; set; } = null!;
        public DbSet<Shipment> Shipments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Warehouse>()
                .OwnsOne(w => w.Location, owned =>
                {
                    owned.Property(l => l.Latitude).HasColumnName("Latitude");
                    owned.Property(l => l.Longitude).HasColumnName("Longitude");
                });

            modelBuilder.Entity<PickupPoint>()
                .OwnsOne(p => p.Location, owned =>
                {
                    owned.Property(l => l.Latitude).HasColumnName("Latitude");
                    owned.Property(l => l.Longitude).HasColumnName("Longitude");
                });

            modelBuilder.Entity<Warehouse>().Ignore(w => w.Stock);
            modelBuilder.Entity<PickupPoint>().Ignore(p => p.Demand);
        }
    }
}
