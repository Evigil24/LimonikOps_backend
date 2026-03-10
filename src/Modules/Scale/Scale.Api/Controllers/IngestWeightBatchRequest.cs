namespace LimonikOne.Modules.Scale.Api.Controllers;

public sealed record IngestWeightBatchRequest(
    Guid BatchId,
    string DeviceId,
    string Location,
    DateTime SentAt,
    List<IngestWeightReadingRequest> Readings
);

public sealed record IngestWeightReadingRequest(
    decimal Weight,
    int Count,
    DateTime FirstTimestamp,
    DateTime LastTimestamp,
    int StableCount
);
