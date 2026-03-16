using LimonikOne.Modules.Product.Domain.Items;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Product.Infrastructure.Database;

internal sealed class ProductDbContext(DbContextOptions<ProductDbContext> options)
    : DbContext(options)
{
    public DbSet<Item> Items => Set<Item>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("product");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
    }
}
