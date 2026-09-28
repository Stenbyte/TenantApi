using TenantApi.Models;
using Microsoft.EntityFrameworkCore;

public class TenantDbContext : DbContext
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options) { }

    public DbSet<UserPg> Users { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Building> Buildings { get; set; }
    public DbSet<UserProperty> UserProperties { get; set; }
    public DbSet<Machine> Machines { get; set; }
    public DbSet<BookingPg> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Property>(e =>
        {
            e.HasOne(p => p.Building)
             .WithMany(b => b.Units)
             .HasForeignKey(p => p.BuildingId)
             .OnDelete(DeleteBehavior.Cascade);
        });


        modelBuilder.Entity<UserProperty>(e =>
        {
            e.HasKey(up => new { up.UserId, up.PropertyId });

            e.HasOne(up => up.User).WithMany(u => u.UserProperties).HasForeignKey(up => up.UserId).OnDelete(DeleteBehavior.Cascade);

            e.HasOne(up => up.Property).WithMany(p => p.UserProperty).HasForeignKey(up => up.PropertyId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserPg>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Machine>(e =>
        {
            e.HasOne(m => m.Building)
                .WithMany()
                .HasForeignKey(m => m.BuildingId)
                .OnDelete(DeleteBehavior.Cascade);

            e.Property(m => m.Name).HasConversion<string>().HasMaxLength(32);
            e.Property(m => m.Status).HasConversion<string>().HasMaxLength(32);

            e.HasIndex(m => new { m.BuildingId, m.Name });
        });

        modelBuilder.Entity<BookingPg>(e =>
        {
            e.HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(b => b.Building)
                .WithMany()
                .HasForeignKey(b => b.BuildingId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(b => b.Machine)
                .WithMany(m => m.Bookings)
                .HasForeignKey(b => b.MachineId)
                .OnDelete(DeleteBehavior.Cascade);

            // Same machine + same slot start = double booking. Fixed TIME_SLOTS make start enough.
            e.HasIndex(b => new { b.MachineId, b.StartTime }).IsUnique();

            e.HasIndex(b => new { b.BuildingId, b.StartTime });
            e.HasIndex(b => new { b.UserId, b.StartTime });
        });

    }
}





