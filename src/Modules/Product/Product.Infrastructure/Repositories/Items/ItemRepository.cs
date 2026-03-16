using LimonikOne.Modules.Product.Domain.Items;
using LimonikOne.Modules.Product.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Product.Infrastructure.Repositories.Items;

internal sealed class ItemRepository(ProductDbContext dbContext) : IItemRepository
{
    private readonly ProductDbContext _dbContext = dbContext;

    public async Task<Item?> GetByIdAsync(ItemId id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Items.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Item>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.Items.ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByItemNumberAsync(
        string itemNumber,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.Items.AnyAsync(p => p.ItemNumber == itemNumber, cancellationToken);
    }

    public async Task AddAsync(Item item, CancellationToken cancellationToken = default)
    {
        await _dbContext.Items.AddAsync(item, cancellationToken);
    }
}
