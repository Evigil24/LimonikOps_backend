using LimonikOne.Modules.Reception.Domain.Weights;
using LimonikOne.Modules.Reception.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Reception.Infrastructure.Repositories;

internal sealed class WeightBatchRepository : IWeightBatchRepository
{
    private readonly ReceptionDbContext _dbContext;

    public WeightBatchRepository(ReceptionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistsByExternalBatchIdAsync(Guid externalBatchId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.WeightBatches
            .AnyAsync(b => b.ExternalBatchId == externalBatchId, cancellationToken);
    }

    public async Task AddAsync(WeightBatchEntity batch, CancellationToken cancellationToken = default)
    {
        await _dbContext.WeightBatches.AddAsync(batch, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
