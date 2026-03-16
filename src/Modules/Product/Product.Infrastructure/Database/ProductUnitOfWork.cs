using LimonikOne.Modules.Product.Application;

namespace LimonikOne.Modules.Product.Infrastructure.Database;

internal sealed class ProductUnitOfWork(ProductDbContext dbContext) : IProductUnitOfWork
{
    private readonly ProductDbContext _dbContext = dbContext;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
