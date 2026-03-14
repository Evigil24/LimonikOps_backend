using LimonikOne.Modules.Product.Domain.Products;
using Microsoft.EntityFrameworkCore;
using ProductAggregate = LimonikOne.Modules.Product.Domain.Products.Product;

namespace LimonikOne.Modules.Product.Infrastructure.Database;

internal sealed class ProductDbContext : DbContext
{
    public DbSet<ProductAggregate> Products => Set<ProductAggregate>();

    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("product");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
    }
}
