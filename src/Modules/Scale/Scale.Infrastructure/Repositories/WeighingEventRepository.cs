using LimonikOne.Modules.Scale.Domain.WeighingEvents;
using LimonikOne.Modules.Scale.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Scale.Infrastructure.Repositories;

internal sealed class WeighingEventRepository : IWeighingEventRepository
{
    private readonly ScaleDbContext _dbContext;

    public WeighingEventRepository(ScaleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WeighingEventEntity?> GetOpenEventByDeviceIdAsync(
        string deviceId,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext
            .WeighingEvents.Include(e => e.Measurements)
            .FirstOrDefaultAsync(
                e => e.DeviceId == deviceId && e.Status == WeighingEventStatus.InProgress,
                cancellationToken
            );
    }

    public async Task AddAsync(
        WeighingEventEntity weighingEvent,
        CancellationToken cancellationToken = default
    )
    {
        await _dbContext.WeighingEvents.AddAsync(weighingEvent, cancellationToken);
    }

    public Task UpdateAsync(
        WeighingEventEntity weighingEvent,
        CancellationToken cancellationToken = default
    )
    {
        _dbContext.WeighingEvents.Update(weighingEvent);
        return Task.CompletedTask;
    }
}
