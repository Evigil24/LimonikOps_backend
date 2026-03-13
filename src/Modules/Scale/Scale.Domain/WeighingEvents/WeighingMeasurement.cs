using LimonikOne.Modules.Scale.Domain.Weights;

namespace LimonikOne.Modules.Scale.Domain.WeighingEvents;

public sealed class WeighingMeasurement
{
    public WeighingMeasurementId Id { get; private init; }
    public decimal Weight { get; private set; }
    public DateTime Timestamp { get; private set; }
    public int StableCount { get; private set; }
    public WeightReadingId SourceReadingId { get; private set; }

    private WeighingMeasurement() { } // EF Core

    internal static WeighingMeasurement Create(
        decimal weight,
        DateTime timestamp,
        int stableCount,
        WeightReadingId sourceReadingId
    )
    {
        return new WeighingMeasurement
        {
            Id = WeighingMeasurementId.New(),
            Weight = weight,
            Timestamp = timestamp,
            StableCount = stableCount,
            SourceReadingId = sourceReadingId,
        };
    }
}
