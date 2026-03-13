using LimonikOne.Modules.Scale.Domain.Weights;
using LimonikOne.Modules.Scale.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Scale.Infrastructure.Repositories;

internal sealed class WeightBatchRepository : IWeightBatchRepository
{
    private readonly ScaleDbContext _dbContext;

    public WeightBatchRepository(ScaleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistsByExternalBatchIdAsync(
        Guid externalBatchId,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.WeightBatches.AnyAsync(
            b => b.ExternalBatchId == externalBatchId,
            cancellationToken
        );
    }

    public async Task AddAsync(
        WeightBatchEntity batch,
        CancellationToken cancellationToken = default
    )
    {
        await _dbContext.WeightBatches.AddAsync(batch, cancellationToken);
    }
}
