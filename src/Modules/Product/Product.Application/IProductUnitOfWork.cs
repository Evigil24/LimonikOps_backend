namespace LimonikOne.Modules.Product.Application;

public interface IProductUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
