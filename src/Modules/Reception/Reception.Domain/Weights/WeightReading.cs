namespace LimonikOne.Modules.Reception.Domain.Weights;

public sealed class WeightReading
{
    public WeightReadingId Id { get; private init; }
    public decimal Weight { get; private set; }
    public int Count { get; private set; }
    public DateTime FirstTimestamp { get; private set; }
    public DateTime LastTimestamp { get; private set; }
    public int StableCount { get; private set; }

    private WeightReading() { } // EF Core

    public static WeightReading Create(
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
            Weight = weight,
            Count = count,
            FirstTimestamp = firstTimestamp,
            LastTimestamp = lastTimestamp,
            StableCount = stableCount,
        };
    }
}
