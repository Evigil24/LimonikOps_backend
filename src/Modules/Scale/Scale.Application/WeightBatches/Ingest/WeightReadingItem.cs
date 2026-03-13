namespace LimonikOne.Modules.Scale.Application.WeightBatches.Ingest;

public sealed record WeightReadingItem(
    decimal Weight,
    int Count,
    DateTime FirstTimestamp,
    DateTime LastTimestamp,
    int StableCount
);
