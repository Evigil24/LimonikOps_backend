namespace LimonikOne.Modules.Reception.Domain.Weights;

public interface IWeightBatchRepository
{
    Task<bool> ExistsByExternalBatchIdAsync(
        Guid externalBatchId,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(WeightBatchEntity batch, CancellationToken cancellationToken = default);
}
