using LimonikOne.Modules.Scale.Domain.WeightReadings;

namespace LimonikOne.Modules.Scale.Domain.WeightEvents;

public sealed class WeightMeasurement
{
    public WeightMeasurementId Id { get; private init; }
    public decimal Weight { get; private set; }
    public DateTime Timestamp { get; private set; }
    public int StableCount { get; private set; }
    public WeightReadingId SourceReadingId { get; private set; }

    private WeightMeasurement() { } // EF Core

    internal static WeightMeasurement Create(
        decimal weight,
        DateTime timestamp,
        int stableCount,
        WeightReadingId sourceReadingId
    )
    {
        return new WeightMeasurement
        {
            Id = WeightMeasurementId.New(),
            Weight = weight,
            Timestamp = timestamp,
            StableCount = stableCount,
            SourceReadingId = sourceReadingId,
        };
    }
}
