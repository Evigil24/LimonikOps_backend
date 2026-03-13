namespace LimonikOne.Modules.Scale.Domain.WeightBatches;

public interface IWeightBatchRepository
{
    Task<bool> ExistsByExternalBatchIdAsync(
        Guid externalBatchId,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(WeightBatchEntity batch, CancellationToken cancellationToken = default);
}
