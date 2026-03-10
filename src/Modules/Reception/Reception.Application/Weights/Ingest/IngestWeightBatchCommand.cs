using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Reception.Application.Weights.Ingest;

public sealed record IngestWeightBatchCommand(
    Guid BatchId,
    string DeviceId,
    string Location,
    DateTime SentAt,
    List<WeightReadingItem> Readings
) : ICommand;
