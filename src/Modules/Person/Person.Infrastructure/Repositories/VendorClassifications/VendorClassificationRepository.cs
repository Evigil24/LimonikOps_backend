using LimonikOne.Modules.Person.Domain.VendorClassifications;
using LimonikOne.Modules.Person.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Person.Infrastructure.Repositories.VendorClassifications;

internal sealed class VendorClassificationRepository : IVendorClassificationRepository
{
    private readonly PersonDbContext _dbContext;

    public VendorClassificationRepository(PersonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<VendorClassificationEntity?> GetByIdAsync(
        VendorClassificationId id,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.VendorClassifications.FirstOrDefaultAsync(
            vc => vc.Id == id,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<VendorClassificationEntity>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.VendorClassifications.ToListAsync(cancellationToken);
    }

    public Task AddAsync(
        VendorClassificationEntity classification,
        CancellationToken cancellationToken = default
    )
    {
        return _dbContext
            .VendorClassifications.AddAsync(classification, cancellationToken)
            .AsTask();
    }

    public Task UpdateAsync(
        VendorClassificationEntity classification,
        CancellationToken cancellationToken = default
    )
    {
        _dbContext.VendorClassifications.Update(classification);
        return Task.CompletedTask;
    }
}
