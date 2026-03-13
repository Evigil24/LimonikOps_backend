namespace LimonikOne.Modules.Scale.Domain.Weights;

public interface IWeightReadingRepository
{
    Task AddRangeAsync(
        IEnumerable<WeightReading> readings,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<WeightReading>> GetByBatchIdAsync(
        WeightBatchId batchId,
        CancellationToken cancellationToken = default
    );
}
