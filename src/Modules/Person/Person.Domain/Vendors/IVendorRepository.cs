namespace LimonikOne.Modules.Person.Domain.Vendors;

public interface IVendorRepository
{
    Task<VendorEntity?> GetByIdAsync(VendorId id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<VendorEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(VendorEntity vendor, CancellationToken cancellationToken = default);

    Task UpdateAsync(VendorEntity vendor, CancellationToken cancellationToken = default);
}
