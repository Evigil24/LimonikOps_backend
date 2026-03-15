namespace LimonikOne.Modules.Scale.Domain.VehicleScaleRecords;

public interface IVehicleScaleRecordRepository
{
    Task<VehicleScaleRecordEntity?> GetByIdAsync(
        VehicleScaleRecordId id,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<VehicleScaleRecordEntity>> GetAllAsync(
        CancellationToken cancellationToken = default
    );

    Task AddAsync(VehicleScaleRecordEntity record, CancellationToken cancellationToken = default);

    Task UpdateAsync(
        VehicleScaleRecordEntity record,
        CancellationToken cancellationToken = default
    );
}
