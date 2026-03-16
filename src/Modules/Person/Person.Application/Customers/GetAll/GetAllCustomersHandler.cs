using LimonikOne.Modules.Person.Domain.Customers;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Person.Application.Customers.GetAll;

internal sealed class GetAllCustomersHandler(ICustomerRepository repository)
    : IQueryHandler<GetAllCustomersQuery, IReadOnlyList<CustomerDto>>
{
    private readonly ICustomerRepository _repository = repository;

    public async Task<Result<IReadOnlyList<CustomerDto>>> HandleAsync(
        GetAllCustomersQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var customers = await _repository.GetAllAsync(cancellationToken);

        var dtos = customers.Select(CustomerDto.FromEntity).ToList();

        return Result.Success<IReadOnlyList<CustomerDto>>(dtos);
    }
}
