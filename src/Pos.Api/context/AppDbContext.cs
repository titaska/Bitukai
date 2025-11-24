using Microsoft.EntityFrameworkCore;
using Pos.Api.taxes.model;
using Pos.Api.BusinessStaff.Models;

namespace Pos.Api.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Taxes> Taxes { get; set; }
    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<Staff> Staff => Set<Staff>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("point_of_sale");
        base.OnModelCreating(modelBuilder);

        // BUSINESS
            modelBuilder.Entity<Business>(entity =>
            {
                entity.HasKey(b => b.RegistrationNumber);
                entity.Property(b => b.RegistrationNumber)
                      .HasMaxLength(50);

                entity.Property(b => b.VatCode)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(b => b.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(b => b.Location)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(b => b.CurrencyCode)
                      .IsRequired()
                      .HasMaxLength(3);
            });

            // STAFF
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasKey(s => s.StaffId);

                entity.HasOne(s => s.Business)
                      .WithMany(b => b.StaffMembers)
                      .HasForeignKey(s => s.RegistrationNumber)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(s => s.Status)
                      .HasConversion<string>()
                      .IsRequired();

                entity.Property(s => s.Role)
                      .HasConversion<string>()
                      .IsRequired();

                entity.Property(s => s.Email)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(s => s.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.LastName).IsRequired().HasMaxLength(100);
            });
    }
}
