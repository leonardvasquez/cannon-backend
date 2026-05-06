using Microsoft.EntityFrameworkCore;
using Cannon.Data.Entities;

namespace Cannon.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Towel> Towels => Set<Towel>();
    public DbSet<Box> Boxes => Set<Box>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Towel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ItemCode).IsUnique();
            entity.Property(e => e.Status).HasConversion<string>();
            entity.HasOne(e => e.Box)
                  .WithMany(b => b.Towels)
                  .HasForeignKey(e => e.BoxId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Box>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.BoxCode).IsUnique();
            entity.Property(e => e.Status).HasConversion<string>();
        });
    }
}