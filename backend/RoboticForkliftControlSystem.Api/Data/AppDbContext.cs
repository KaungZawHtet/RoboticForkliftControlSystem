using Microsoft.EntityFrameworkCore;
using RoboticForkliftControlSystem.Api.Entities;

namespace RoboticForkliftControlSystem.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Forklift> Forklifts => Set<Forklift>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Forklift>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.ModelNumber).HasMaxLength(50).IsRequired();
            e.Property(x => x.ManufacturingDate).HasColumnType("date");
        });
    }
}
