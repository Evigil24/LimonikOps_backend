using LimonikOne.Modules.Product.Domain.Items;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Product.Infrastructure.Database;

internal sealed class ProductDbContext : DbContext
{
    public DbSet<Item> Items => Set<Item>();

    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("product");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
    }
}
