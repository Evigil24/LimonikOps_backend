using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Scale.Domain.Weights;

public sealed class WeightReading : AggregateRoot<WeightReadingId>
{
    public WeightBatchId BatchId { get; private set; }
    public decimal Weight { get; private set; }
    public int Count { get; private set; }
    public DateTime FirstTimestamp { get; private set; }
    public DateTime LastTimestamp { get; private set; }
    public int StableCount { get; private set; }

    private WeightReading() { } // EF Core

    public static WeightReading Create(
        WeightBatchId batchId,
        decimal weight,
        int count,
        DateTime firstTimestamp,
        DateTime lastTimestamp,
        int stableCount
    )
    {
        return new WeightReading
        {
            Id = WeightReadingId.New(),
            BatchId = batchId,
            Weight = weight,
            Count = count,
            FirstTimestamp = firstTimestamp,
            LastTimestamp = lastTimestamp,
            StableCount = stableCount,
        };
    }
}
