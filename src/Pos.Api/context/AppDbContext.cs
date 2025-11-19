using Microsoft.EntityFrameworkCore;
using Pos.Api.taxes.model;

namespace Pos.Api.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Taxes> Taxes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("point_of_sale");
        base.OnModelCreating(modelBuilder);
    }
}
