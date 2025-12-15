using Microsoft.EntityFrameworkCore;
using Pos.Api.Orders.Model;
using Pos.Api.taxes.model;
using Pos.Api.reservations.model;
using Pos.Api.BusinessStaff.Models;
using Pos.Api.Products.model;
using Pos.Api.Products.service;

namespace Pos.Api.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tax> Taxes { get; set; } = null!;

    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderLine> OrderLines { get; set; } = null!;
    public DbSet<OrderLineOption> OrderLineOptions { get; set; } = null!;
    public DbSet<OrderLineTax> OrderLineTaxes { get; set; } = null!;
    public DbSet<Reservation> Reservations { get; set; } = null!;

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductStaff> ProductStaff { get; set; } = null!;

    public DbSet<Business> Businesses { get; set; } = null!;
    public DbSet<Staff> Staff { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("point_of_sale");
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>()
            .Property(o => o.status)
            .HasConversion<string>();

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(b => b.RegistrationNumber);

            entity.Property(b => b.RegistrationNumber)
                .HasColumnName("RegistrationNumber")
                .HasMaxLength(50);

            entity.Property(b => b.VatCode)
                .HasColumnName("VatCode")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(b => b.Name)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(b => b.Location)
                .HasColumnName("Location")
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(b => b.CurrencyCode)
                .HasColumnName("CurrencyCode")
                .IsRequired()
                .HasMaxLength(3);

            entity.Property(b => b.Type)
                .HasConversion<int>() 
                .IsRequired();

        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.ToTable("Staff");

            entity.HasKey(s => s.staffId);

            entity.Property(s => s.staffId)
                .HasColumnName("StaffId");

            entity.Property(s => s.registrationNumber)
                .HasColumnName("RegistrationNumber")
                .IsRequired();

            entity.Property(s => s.status)
                .HasColumnName("Status")
                .HasConversion<string>()
                .IsRequired();

            entity.Property(s => s.firstName)
                .HasColumnName("FirstName")
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(s => s.lastName)
                .HasColumnName("LastName")
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(s => s.email)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(s => s.phoneNumber)
                .HasColumnName("PhoneNumber")
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(s => s.Business)
                .WithMany(b => b.StaffMembers)
                .HasForeignKey(s => s.registrationNumber)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.productId);

            entity.Property(p => p.productId)
                .HasColumnName("productId")
                .IsRequired();

            entity.Property(p => p.registrationNumber)
                .HasColumnName("registrationNumber")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(p => p.type)
                .HasColumnName("type")
                .HasConversion<int>()
                .IsRequired();

            entity.Property(p => p.name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(p => p.description)
                .HasColumnName("description")
                .HasMaxLength(1000);

            entity.Property(p => p.basePrice)
                .HasColumnName("basePrice")
                .HasColumnType("numeric(12,2)")
                .IsRequired();

            entity.Property(p => p.taxCode)
                .HasColumnName("taxCode")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(p => p.status)
                .HasColumnName("status")
                .IsRequired();

            entity.Property(p => p.durationMinutes)
                .HasColumnName("durationMinutes")
                .IsRequired();
        });

        modelBuilder.Entity<ProductStaff>(entity =>
        {
            entity.HasKey(ps => ps.productStaffId);

            entity.Property(ps => ps.productStaffId)
                .HasColumnName("productStaffId");

            entity.Property(ps => ps.productId)
                .HasColumnName("productId")
                .IsRequired();

            entity.Property(ps => ps.staffId)
                .HasColumnName("staffId")
                .IsRequired();

            entity.Property(ps => ps.status)
                .HasColumnName("status")
                .IsRequired();

            entity.Property(ps => ps.valideFrom)
                .HasColumnName("valideFrom");

            entity.Property(ps => ps.valideTo)
                .HasColumnName("valideTo");

            entity.HasOne(ps => ps.product)
                .WithMany(p => p.EligibleStaff)
                .HasForeignKey(ps => ps.productId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ps => ps.staff)
                .WithMany(s => s.productAssignments)
                .HasForeignKey(ps => ps.staffId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(ps => new { ps.productId, ps.staffId })
                .IsUnique();
        });

        modelBuilder.Entity<Tax>(entity =>
        {
            entity.HasIndex(t => t.name).IsUnique();
        });
    }
}
