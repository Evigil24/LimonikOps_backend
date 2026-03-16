using LimonikOne.Modules.Person.Domain.Customers;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Person.Application.Customers.GetById;

internal sealed class GetCustomerByIdHandler(ICustomerRepository repository)
    : IQueryHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly ICustomerRepository _repository = repository;

    public async Task<Result<CustomerDto>> HandleAsync(
        GetCustomerByIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var customer = await _repository.GetByIdAsync(CustomerId.From(query.Id), cancellationToken);

        if (customer is null)
        {
            return Result.Failure<CustomerDto>(CustomerErrors.NotFound(query.Id));
        }

        return Result.Success(CustomerDto.FromEntity(customer));
    }
}
