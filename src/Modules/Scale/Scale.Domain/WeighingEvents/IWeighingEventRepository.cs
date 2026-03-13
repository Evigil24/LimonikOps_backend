namespace LimonikOne.Modules.Scale.Domain.WeighingEvents;

public interface IWeighingEventRepository
{
    Task<WeighingEventEntity?> GetOpenEventByDeviceIdAsync(
        string deviceId,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(WeighingEventEntity weighingEvent, CancellationToken cancellationToken = default);

    Task UpdateAsync(
        WeighingEventEntity weighingEvent,
        CancellationToken cancellationToken = default
    );
}
