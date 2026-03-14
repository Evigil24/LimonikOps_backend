using LimonikOne.Modules.Scale.Domain.WeightBatches;
using LimonikOne.Modules.Scale.Domain.WeightEvents;
using LimonikOne.Modules.Scale.Domain.WeightReadings;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Scale.Application.WeightBatches.Ingest;

internal sealed class IngestWeightBatchHandler : ICommandHandler<IngestWeightBatchCommand>
{
    private readonly IWeightBatchRepository _weightBatchRepository;
    private readonly IWeightReadingRepository _weightReadingRepository;
    private readonly IWeightEventRepository _weightEventRepository;
    private readonly IScaleUnitOfWork _unitOfWork;

    private const decimal WeightThreshold = 0m;

    public IngestWeightBatchHandler(
        IWeightBatchRepository weightBatchRepository,
        IWeightReadingRepository weightReadingRepository,
        IWeightEventRepository weightEventRepository,
        IScaleUnitOfWork unitOfWork
    )
    {
        _weightBatchRepository = weightBatchRepository;
        _weightReadingRepository = weightReadingRepository;
        _weightEventRepository = weightEventRepository;
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

        var batch = WeightBatchEntity.Create(
            command.BatchId,
            command.DeviceId,
            command.Location,
            command.SentAt
        );

        await _weightBatchRepository.AddAsync(batch, cancellationToken);

        var readings = command
            .Readings.Select(r =>
                WeightReading.Create(
                    batch.Id,
                    r.Weight,
                    r.Count,
                    r.FirstTimestamp,
                    r.LastTimestamp,
                    r.StableCount
                )
            )
            .ToList();

        await _weightReadingRepository.AddRangeAsync(readings, cancellationToken);

        await ClassifyReadingsIntoEventsAsync(
            command.DeviceId,
            command.Location,
            readings,
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
        var openEvent = await _weightEventRepository.GetOpenEventByDeviceIdAsync(
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
                    openEvent = WeightEventEntity.Start(
                        deviceId,
                        location,
                        reading.Weight,
                        reading.FirstTimestamp,
                        reading.StableCount,
                        reading.Id
                    );
                    await _weightEventRepository.AddAsync(openEvent, cancellationToken);
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
