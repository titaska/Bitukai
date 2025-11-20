using Microsoft.EntityFrameworkCore;
using Pos.Api.Orders.Model;
using Pos.Api.taxes.model;

namespace Pos.Api.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Taxes> Taxes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<OrderLineOption> OrderLineOptions { get; set; }
    public DbSet<OrderLineTax> OrderLineTaxes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("point_of_sale");
        base.OnModelCreating(modelBuilder);
    }
}
