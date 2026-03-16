using LimonikOne.Modules.Scale.Domain.VehicleScaleRecords;
using LimonikOne.Modules.Scale.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Scale.Infrastructure.Repositories.VehicleScaleRecords;

internal sealed class VehicleScaleRecordRepository(ScaleDbContext dbContext)
    : IVehicleScaleRecordRepository
{
    private readonly ScaleDbContext _dbContext = dbContext;

    public Task AddAsync(
        VehicleScaleRecordEntity record,
        CancellationToken cancellationToken = default
    )
    {
        return _dbContext.VehicleScaleRecords.AddAsync(record, cancellationToken).AsTask();
    }

    public Task UpdateAsync(
        VehicleScaleRecordEntity record,
        CancellationToken cancellationToken = default
    )
    {
        _dbContext.VehicleScaleRecords.Update(record);
        return Task.CompletedTask;
    }

    public async Task<VehicleScaleRecordEntity?> GetByIdAsync(
        VehicleScaleRecordId id,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.VehicleScaleRecords.FirstOrDefaultAsync(
            record => record.Id == id,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<VehicleScaleRecordEntity>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.VehicleScaleRecords.ToListAsync(cancellationToken);
    }
}
