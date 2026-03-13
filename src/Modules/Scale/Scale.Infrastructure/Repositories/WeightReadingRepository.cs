using LimonikOne.Modules.Scale.Domain.Weights;
using LimonikOne.Modules.Scale.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Scale.Infrastructure.Repositories;

internal sealed class WeightReadingRepository : IWeightReadingRepository
{
    private readonly ScaleDbContext _dbContext;

    public WeightReadingRepository(ScaleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

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
