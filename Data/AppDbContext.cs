using Microsoft.EntityFrameworkCore;
using UserApiTest.Models;

namespace UserApiTest.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var user = modelBuilder.Entity<User>();

        // Table name (optional – EF would do this by convention)
        user.ToTable("Users");

        // Primary key (convention already covers this, but explicit is fine)
        user.HasKey(u => u.Id);

        // Email column details (length matches attribute; index is DB-side concern)
        user.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        // Optional: prepare for uniqueness (you can enforce business rule in Day 4)
        user.HasIndex(u => u.Email)
            .IsUnique();

        // You can also fine-tune lengths if you want DB and attributes to match:
        user.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
        user.Property(u => u.LastName).HasMaxLength(100).IsRequired();
    }
}