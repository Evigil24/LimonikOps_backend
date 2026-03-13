using LimonikOne.Modules.Scale.Domain.WeightReadings;
using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Scale.Domain.WeightEvents;

public sealed class WeightEventEntity : AggregateRoot<WeightEventId>
{
    public string DeviceId { get; private set; } = null!;
    public string Location { get; private set; } = null!;
    public WeightEventStatus Status { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }
    public decimal PeakWeight { get; private set; }

    private readonly List<WeightMeasurement> _measurements = [];
    public IReadOnlyList<WeightMeasurement> Measurements => _measurements.AsReadOnly();

    private WeightEventEntity() { } // EF Core

    public static WeightEventEntity Start(
        string deviceId,
        string location,
        decimal weight,
        DateTime timestamp,
        int stableCount,
        WeightReadingId sourceReadingId
    )
    {
        var weightEvent = new WeightEventEntity
        {
            Id = WeightEventId.New(),
            DeviceId = deviceId,
            Location = location,
            Status = WeightEventStatus.InProgress,
            StartedAt = timestamp,
            PeakWeight = weight,
        };

        weightEvent._measurements.Add(
            WeightMeasurement.Create(weight, timestamp, stableCount, sourceReadingId)
        );

        return weightEvent;
    }

    public void AddMeasurement(
        decimal weight,
        DateTime timestamp,
        int stableCount,
        WeightReadingId sourceReadingId
    )
    {
        if (Status == WeightEventStatus.Completed)
        {
            throw new InvalidOperationException(
                "Cannot add measurements to a completed weight event."
            );
        }

        _measurements.Add(
            WeightMeasurement.Create(weight, timestamp, stableCount, sourceReadingId)
        );

        if (weight > PeakWeight)
        {
            PeakWeight = weight;
        }
    }

    public void Complete(DateTime endedAt)
    {
        if (Status == WeightEventStatus.Completed)
        {
            throw new InvalidOperationException("Weight event is already completed.");
        }

        Status = WeightEventStatus.Completed;
        EndedAt = endedAt;
    }
}
