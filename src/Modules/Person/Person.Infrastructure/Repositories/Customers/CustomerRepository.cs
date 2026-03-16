using LimonikOne.Modules.Person.Domain.Customers;
using LimonikOne.Modules.Person.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Person.Infrastructure.Repositories.Customers;

internal sealed class CustomerRepository(PersonDbContext dbContext) : ICustomerRepository
{
    private readonly PersonDbContext _dbContext = dbContext;

    public async Task<CustomerEntity?> GetByIdAsync(
        CustomerId id,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.Customers.FirstOrDefaultAsync(
            customer => customer.Id == id,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<CustomerEntity>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.Customers.ToListAsync(cancellationToken);
    }

    public Task AddAsync(CustomerEntity customer, CancellationToken cancellationToken = default)
    {
        return _dbContext.Customers.AddAsync(customer, cancellationToken).AsTask();
    }

    public async Task AddRangeAsync(
        IEnumerable<CustomerEntity> customers,
        CancellationToken cancellationToken = default
    )
    {
        await _dbContext.Customers.AddRangeAsync(customers, cancellationToken);
    }

    public Task UpdateAsync(CustomerEntity customer, CancellationToken cancellationToken = default)
    {
        _dbContext.Customers.Update(customer);
        return Task.CompletedTask;
    }
}
