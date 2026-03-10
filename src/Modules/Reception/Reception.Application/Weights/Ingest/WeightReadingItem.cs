namespace LimonikOne.Modules.Reception.Application.Weights.Ingest;

public sealed record WeightReadingItem(
    decimal Weight,
    int Count,
    DateTime FirstTimestamp,
    DateTime LastTimestamp,
    int StableCount
);
