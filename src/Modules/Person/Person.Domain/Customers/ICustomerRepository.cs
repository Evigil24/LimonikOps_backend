namespace LimonikOne.Modules.Person.Domain.Customers;

public interface ICustomerRepository
{
    Task<CustomerEntity?> GetByIdAsync(
        CustomerId id,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<CustomerEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(CustomerEntity customer, CancellationToken cancellationToken = default);

    Task AddRangeAsync(
        IEnumerable<CustomerEntity> customers,
        CancellationToken cancellationToken = default
    );

    Task UpdateAsync(CustomerEntity customer, CancellationToken cancellationToken = default);
}
