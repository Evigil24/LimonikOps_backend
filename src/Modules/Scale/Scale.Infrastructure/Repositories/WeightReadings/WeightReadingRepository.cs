using LimonikOne.Modules.Scale.Domain.WeightBatches;
using LimonikOne.Modules.Scale.Domain.WeightReadings;
using LimonikOne.Modules.Scale.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Scale.Infrastructure.Repositories.WeightReadings;

internal sealed class WeightReadingRepository(ScaleDbContext dbContext) : IWeightReadingRepository
{
    private readonly ScaleDbContext _dbContext = dbContext;

    public Task AddRangeAsync(
        IEnumerable<WeightReading> readings,
        CancellationToken cancellationToken = default
    )
    {
        return _dbContext.WeightReadings.AddRangeAsync(readings.ToList(), cancellationToken);
    }

    public async Task<IReadOnlyList<WeightReading>> GetByBatchIdAsync(
        WeightBatchId batchId,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext
            .WeightReadings.Where(r => r.BatchId == batchId)
            .OrderBy(r => r.FirstTimestamp)
            .ToListAsync(cancellationToken);
    }
}
