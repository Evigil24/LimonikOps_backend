namespace LimonikOne.Modules.Product.Domain.Products;

public interface IProductRepository
{
    Task<bool> ExistsByItemNumberAsync(
        string itemNumber,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
}
