using System;
using Microsoft.EntityFrameworkCore;
using Pos.Api.Orders.Model;
using Pos.Api.taxes.model;
using Pos.Api.reservations.model;
using Pos.Api.BusinessStaff.Models;

namespace Pos.Api.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tax> Taxes { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<OrderLineOption> OrderLineOptions { get; set; }
    public DbSet<OrderLineTax> OrderLineTaxes { get; set; }
    public DbSet<Reservation> Reservations { get; set; }


    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<ServiceConfig> Services => Set<ServiceConfig>();
    public DbSet<StaffServiceAssignment> StaffServiceAssignments => Set<StaffServiceAssignment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("point_of_sale");
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>()
            .Property(o => o.status)
            .HasConversion<string>();

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

            entity.Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(100);
        });

        // SERVICES
        modelBuilder.Entity<ServiceConfig>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Price)
                .HasColumnType("numeric(12,2)")
                .IsRequired();
        });

        // STAFF <-> SERVICES (Assignments)
        modelBuilder.Entity<StaffServiceAssignment>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Revenue)
                .HasColumnType("numeric(12,2)")
                .IsRequired();

            entity.HasOne(x => x.Staff)
                .WithMany(s => s.ServiceAssignments)
                .HasForeignKey(x => x.StaffId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Service)
                .WithMany(s => s.StaffAssignments)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new { x.StaffId, x.ServiceId })
                .IsUnique();
        });
    }
}
