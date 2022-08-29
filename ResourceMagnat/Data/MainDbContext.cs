using Microsoft.EntityFrameworkCore;
using ResourceMagnat.Models;

namespace ResourceMagnat.Data;

public class MainDbContext: DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<User>()
        //    .Property(p => p.Id)
        //    .ValueGeneratedOnAdd();

        modelBuilder.Entity<User>()
            .HasIndex(p => p.Uid)
            .IsUnique();

        //modelBuilder.Entity<Building>()
        //    .Property(p => p.Id)
        //    .ValueGeneratedOnAdd();

        //modelBuilder.Entity<Building>()
        //    .HasOne(p => p.BuildingType)
        //    .WithOne()
        //    .HasForeignKey("BuildingTypeId");

        //modelBuilder.Entity<Building>()
        //    .Navigation(p => p.BuildingType)
        //    .UsePropertyAccessMode(PropertyAccessMode.Property);
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Building> Buildings { get; set; }

    public DbSet<BuildingType> BuildingTypes { get; set; }
}