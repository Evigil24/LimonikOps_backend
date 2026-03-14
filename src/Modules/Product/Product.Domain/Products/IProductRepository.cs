namespace LimonikOne.Modules.Product.Domain.Products;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByItemNumberAsync(
        string itemNumber,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
}
