using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Scale.Application.WeightBatches.Ingest;

public sealed record IngestWeightBatchCommand(
    Guid BatchId,
    string DeviceId,
    string Location,
    DateTime SentAt,
    List<WeightReadingItem> Readings
) : ICommand;
