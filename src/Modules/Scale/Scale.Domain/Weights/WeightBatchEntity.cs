using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Scale.Domain.Weights;

public sealed class WeightBatchEntity : AggregateRoot<WeightBatchId>
{
    public Guid ExternalBatchId { get; private set; }
    public string DeviceId { get; private set; } = null!;
    public string Location { get; private set; } = null!;
    public DateTime SentAt { get; private set; }
    public DateTime ReceivedAt { get; private set; }

    private WeightBatchEntity() { } // EF Core

    public static WeightBatchEntity Create(
        Guid externalBatchId,
        string deviceId,
        string location,
        DateTime sentAt
    )
    {
        return new WeightBatchEntity
        {
            Id = WeightBatchId.New(),
            ExternalBatchId = externalBatchId,
            DeviceId = deviceId,
            Location = location,
            SentAt = sentAt,
            ReceivedAt = DateTime.UtcNow,
        };
    }
}
