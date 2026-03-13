using LimonikOne.Modules.Scale.Domain.WeighingEvents;
using LimonikOne.Modules.Scale.Domain.Weights;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Scale.Application.Weights.Ingest;

internal sealed class IngestWeightBatchHandler : ICommandHandler<IngestWeightBatchCommand>
{
    private readonly IWeightBatchRepository _weightBatchRepository;
    private readonly IWeighingEventRepository _weighingEventRepository;
    private readonly IScaleUnitOfWork _unitOfWork;

    private const decimal WeightThreshold = 0m;

    public IngestWeightBatchHandler(
        IWeightBatchRepository weightBatchRepository,
        IWeighingEventRepository weighingEventRepository,
        IScaleUnitOfWork unitOfWork
    )
    {
        _weightBatchRepository = weightBatchRepository;
        _weighingEventRepository = weighingEventRepository;
        _unitOfWork = unitOfWork;
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

        await ClassifyReadingsIntoEventsAsync(
            command.DeviceId,
            command.Location,
            batch.Readings,
            cancellationToken
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task ClassifyReadingsIntoEventsAsync(
        string deviceId,
        string location,
        IReadOnlyList<WeightReading> readings,
        CancellationToken cancellationToken
    )
    {
        var openEvent = await _weighingEventRepository.GetOpenEventByDeviceIdAsync(
            deviceId,
            cancellationToken
        );

        foreach (var reading in readings.OrderBy(r => r.FirstTimestamp))
        {
            var isAboveThreshold = reading.Weight > WeightThreshold;

            if (openEvent is null)
            {
                if (isAboveThreshold)
                {
                    openEvent = WeighingEventEntity.Start(
                        deviceId,
                        location,
                        reading.Weight,
                        reading.FirstTimestamp,
                        reading.StableCount,
                        reading.Id
                    );
                    await _weighingEventRepository.AddAsync(openEvent, cancellationToken);
                }
            }
            else
            {
                if (isAboveThreshold)
                {
                    openEvent.AddMeasurement(
                        reading.Weight,
                        reading.FirstTimestamp,
                        reading.StableCount,
                        reading.Id
                    );
                }
                else
                {
                    openEvent.Complete(reading.FirstTimestamp);
                    openEvent = null;
                }
            }
        }
    }
}
