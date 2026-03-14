using LimonikOne.Modules.Product.Domain.Products;
using LimonikOne.Modules.Product.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using ProductAggregate = LimonikOne.Modules.Product.Domain.Products.Product;

namespace LimonikOne.Modules.Product.Infrastructure.Repositories.Products;

internal sealed class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _dbContext;

    public ProductRepository(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProductAggregate?> GetByIdAsync(
        ProductId id,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ProductAggregate>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.Products.ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByItemNumberAsync(
        string itemNumber,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.Products.AnyAsync(
            p => p.ItemNumber == itemNumber,
            cancellationToken
        );
    }

    public async Task AddAsync(
        ProductAggregate product,
        CancellationToken cancellationToken = default
    )
    {
        await _dbContext.Products.AddAsync(product, cancellationToken);
    }
}
