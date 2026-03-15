namespace LimonikOne.Modules.Product.Domain.Items;

public interface IItemRepository
{
    Task<Item?> GetByIdAsync(ItemId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Item>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByItemNumberAsync(
        string itemNumber,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Item item, CancellationToken cancellationToken = default);
}
