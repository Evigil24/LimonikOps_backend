using LimonikOne.Modules.Reception.Domain.Weights;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Reception.Application.Weights.Ingest;

internal sealed class IngestWeightBatchHandler : ICommandHandler<IngestWeightBatchCommand>
{
    private readonly IWeightBatchRepository _weightBatchRepository;

    public IngestWeightBatchHandler(IWeightBatchRepository weightBatchRepository)
    {
        _weightBatchRepository = weightBatchRepository;
    }

    public async Task<Result> HandleAsync(
        IngestWeightBatchCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var exists = await _weightBatchRepository.ExistsByExternalBatchIdAsync(
            command.BatchId,
            cancellationToken
        );
        if (exists)
        {
            return Result.Success();
        }

        var readings = command
            .Readings.Select(r =>
                WeightReading.Create(
                    r.Weight,
                    r.Count,
                    r.FirstTimestamp,
                    r.LastTimestamp,
                    r.StableCount
                )
            )
            .ToList();

        var batch = WeightBatchEntity.Create(
            command.BatchId,
            command.DeviceId,
            command.Location,
            command.SentAt,
            readings
        );

        await _weightBatchRepository.AddAsync(batch, cancellationToken);

        return Result.Success();
    }
}
