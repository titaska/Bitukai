using Microsoft.EntityFrameworkCore;
using Pos.Api.Orders.Model;
using Pos.Api.taxes.model;
using Pos.Api.reservations.model;
using Pos.Api.BusinessStaff.Models;
using Pos.Api.Products.model;

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
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<ServiceStaff> ServiceStaff { get; set; } = null!;
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

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(p => p.serviceId);

            entity.Property(p => p.serviceId)
                .HasColumnName("ProductId")
                .IsRequired();

            entity.Property(p => p.registrationNumber)
                .HasColumnName("RegistrationNumber")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(p => p.type)
                .HasColumnName("Type")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(p => p.name)
                .HasColumnName("Name")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(p => p.description)
                .HasColumnName("Description")
                .HasMaxLength(1000);

            entity.Property(p => p.basePrice)
                .HasColumnName("BasePrice")
                .HasColumnType("numeric(12,2)")
                .IsRequired();

            entity.Property(p => p.taxCode)
                .HasColumnName("TaxCode")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(p => p.status)
                .HasColumnName("Status")
                .IsRequired();

            entity.Property(p => p.durationMinutes)
                .HasColumnName("DurationMinutes")
                .IsRequired();
        });

        modelBuilder.Entity<ServiceStaff>(entity =>
        {
            entity.HasKey(ps => ps.serviceStaffId);

            entity.Property(ps => ps.serviceStaffId)
                .HasColumnName("ServiceStaffId");

            entity.Property(ps => ps.serviceId)
                .HasColumnName("ProductId")
                .IsRequired();

            entity.Property(ps => ps.staffId)
                .HasColumnName("StaffId")
                .IsRequired();

            entity.Property(ps => ps.status)
                .HasColumnName("Status")
                .IsRequired();

            entity.Property(ps => ps.valideFrom)
                .HasColumnName("ValideFrom");

            entity.Property(ps => ps.valideTo)
                .HasColumnName("ValideTo");

            entity.HasOne(ps => ps.service)
                .WithMany(p => p.EligibleStaff)
                .HasForeignKey(ps => ps.serviceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ps => ps.staff)
                .WithMany(s => s.serviceAssignments)
                .HasForeignKey(ps => ps.staffId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(ps => new { ps.serviceId, ps.staffId })
                .IsUnique();
        });

        modelBuilder.Entity<Tax>(entity =>
        {
            entity.HasIndex(t => t.name).IsUnique();
        });
    }
}
