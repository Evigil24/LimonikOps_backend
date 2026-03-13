using LimonikOne.Modules.Scale.Domain.WeightReadings;
using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Scale.Domain.WeighingEvents;

public sealed class WeighingEventEntity : AggregateRoot<WeighingEventId>
{
    public string DeviceId { get; private set; } = null!;
    public string Location { get; private set; } = null!;
    public WeighingEventStatus Status { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }
    public decimal PeakWeight { get; private set; }

    private readonly List<WeighingMeasurement> _measurements = [];
    public IReadOnlyList<WeighingMeasurement> Measurements => _measurements.AsReadOnly();

    private WeighingEventEntity() { } // EF Core

    public static WeighingEventEntity Start(
        string deviceId,
        string location,
        decimal weight,
        DateTime timestamp,
        int stableCount,
        WeightReadingId sourceReadingId
    )
    {
        var weighingEvent = new WeighingEventEntity
        {
            Id = WeighingEventId.New(),
            DeviceId = deviceId,
            Location = location,
            Status = WeighingEventStatus.InProgress,
            StartedAt = timestamp,
            PeakWeight = weight,
        };

        weighingEvent._measurements.Add(
            WeighingMeasurement.Create(weight, timestamp, stableCount, sourceReadingId)
        );

        return weighingEvent;
    }

    public void AddMeasurement(
        decimal weight,
        DateTime timestamp,
        int stableCount,
        WeightReadingId sourceReadingId
    )
    {
        if (Status == WeighingEventStatus.Completed)
        {
            throw new InvalidOperationException(
                "Cannot add measurements to a completed weighing event."
            );
        }

        _measurements.Add(
            WeighingMeasurement.Create(weight, timestamp, stableCount, sourceReadingId)
        );

        if (weight > PeakWeight)
        {
            PeakWeight = weight;
        }
    }

    public void Complete(DateTime endedAt)
    {
        if (Status == WeighingEventStatus.Completed)
        {
            throw new InvalidOperationException("Weighing event is already completed.");
        }

        Status = WeighingEventStatus.Completed;
        EndedAt = endedAt;
    }
}
