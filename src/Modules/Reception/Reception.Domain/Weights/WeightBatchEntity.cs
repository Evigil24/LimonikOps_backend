using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Reception.Domain.Weights;

public sealed class WeightBatchEntity : AggregateRoot<WeightBatchId>
{
    public Guid ExternalBatchId { get; private set; }
    public string DeviceId { get; private set; } = null!;
    public string Location { get; private set; } = null!;
    public DateTime SentAt { get; private set; }
    public DateTime ReceivedAt { get; private set; }

    private readonly List<WeightReading> _readings = [];
    public IReadOnlyList<WeightReading> Readings => _readings.AsReadOnly();

    private WeightBatchEntity() { } // EF Core

    public static WeightBatchEntity Create(
        Guid externalBatchId,
        string deviceId,
        string location,
        DateTime sentAt,
        List<WeightReading> readings
    )
    {
        var batch = new WeightBatchEntity
        {
            Id = WeightBatchId.New(),
            ExternalBatchId = externalBatchId,
            DeviceId = deviceId,
            Location = location,
            SentAt = sentAt,
            ReceivedAt = DateTime.UtcNow,
        };

        batch._readings.AddRange(readings);

        return batch;
    }
}
