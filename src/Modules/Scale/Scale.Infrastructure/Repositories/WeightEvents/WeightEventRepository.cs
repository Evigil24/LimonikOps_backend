using LimonikOne.Modules.Scale.Domain.WeightEvents;
using LimonikOne.Modules.Scale.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Scale.Infrastructure.Repositories.WeightEvents;

internal sealed class WeightEventRepository(ScaleDbContext dbContext) : IWeightEventRepository
{
    private readonly ScaleDbContext _dbContext = dbContext;

    public async Task<WeightEventEntity?> GetOpenEventByDeviceIdAsync(
        string deviceId,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext
            .WeightEvents.Include(e => e.Measurements)
            .FirstOrDefaultAsync(
                e => e.DeviceId == deviceId && e.Status == WeightEventStatus.InProgress,
                cancellationToken
            );
    }

    public async Task AddAsync(
        WeightEventEntity weightEvent,
        CancellationToken cancellationToken = default
    )
    {
        await _dbContext.WeightEvents.AddAsync(weightEvent, cancellationToken);
    }

    public Task UpdateAsync(
        WeightEventEntity weightEvent,
        CancellationToken cancellationToken = default
    )
    {
        _dbContext.WeightEvents.Update(weightEvent);
        return Task.CompletedTask;
    }
}
