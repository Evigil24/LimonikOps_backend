namespace LimonikOne.Modules.Person.Domain.VendorClassifications;

public interface IVendorClassificationRepository
{
    Task<VendorClassificationEntity?> GetByIdAsync(
        VendorClassificationId id,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<VendorClassificationEntity>> GetAllAsync(
        CancellationToken cancellationToken = default
    );

    Task AddAsync(
        VendorClassificationEntity classification,
        CancellationToken cancellationToken = default
    );

    Task UpdateAsync(
        VendorClassificationEntity classification,
        CancellationToken cancellationToken = default
    );
}
