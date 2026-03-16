using LimonikOne.Modules.Person.Domain.Vendors;
using LimonikOne.Modules.Person.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Person.Infrastructure.Repositories.Vendors;

internal sealed class VendorRepository : IVendorRepository
{
    private readonly PersonDbContext _dbContext;

    public VendorRepository(PersonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<VendorEntity?> GetByIdAsync(
        VendorId id,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.Vendors.FirstOrDefaultAsync(
            vendor => vendor.Id == id,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<VendorEntity>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.Vendors.ToListAsync(cancellationToken);
    }

    public Task AddAsync(VendorEntity vendor, CancellationToken cancellationToken = default)
    {
        return _dbContext.Vendors.AddAsync(vendor, cancellationToken).AsTask();
    }

    public async Task AddRangeAsync(
        IEnumerable<VendorEntity> vendors,
        CancellationToken cancellationToken = default
    )
    {
        await _dbContext.Vendors.AddRangeAsync(vendors, cancellationToken);
    }

    public Task UpdateAsync(VendorEntity vendor, CancellationToken cancellationToken = default)
    {
        _dbContext.Vendors.Update(vendor);
        return Task.CompletedTask;
    }
}
