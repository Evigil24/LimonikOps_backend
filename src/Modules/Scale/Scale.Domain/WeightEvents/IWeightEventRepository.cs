namespace LimonikOne.Modules.Scale.Domain.WeightEvents;

public interface IWeightEventRepository
{
    Task<WeightEventEntity?> GetOpenEventByDeviceIdAsync(
        string deviceId,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(WeightEventEntity weightEvent, CancellationToken cancellationToken = default);

    Task UpdateAsync(WeightEventEntity weightEvent, CancellationToken cancellationToken = default);
}
